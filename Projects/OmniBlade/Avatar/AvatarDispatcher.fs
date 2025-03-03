﻿// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2020.

namespace OmniBlade
open System
open System.Numerics
open Prime
open Nu
open Nu.Declarative
open OmniBlade

[<AutoOpen>]
module AvatarDispatcher =

    type [<NoComparison>] AvatarMessage =
        | Update
        | PostUpdate
        | BodyCollision of BodyCollisionData
        | BodySeparation of BodySeparationData
        | TryFace of Direction
        | Nil
        interface Message

    type [<NoComparison>] AvatarCommand =
        | TryTravel of Vector3
        interface Command

    type Entity with
        member this.GetAvatar world = this.GetModelGeneric<Avatar> world
        member this.SetAvatar value world = this.SetModelGeneric<Avatar> value world
        member this.Avatar = this.ModelGeneric<Avatar> ()

    type AvatarDispatcher () =
        inherit EntityDispatcher2d<Avatar, AvatarMessage, AvatarCommand>
            (false, true, Avatar.make (box3 v3Zero Constants.Gameplay.CharacterSize) Assets.Field.JinnAnimationSheet Downward)

        static let getSpriteInset (entity : Entity) world =
            let avatar = entity.GetAvatar world
            let inset = Avatar.getAnimationInset (World.getUpdateTime world) avatar
            inset

        static let isIntersectedProp collider collidee (avatar : Avatar) world =
            if (collider.BodyShapeId = avatar.CoreShapeId &&
                collidee.Entity.Exists world &&
                collidee.Entity.Is<PropDispatcher> world &&
                match (collidee.Entity.GetPropPlus world).Prop.PropData with
                | Portal _ -> true
                | Sensor _ -> true
                | _ -> false) then
                true
            elif (collider.BodyShapeId = avatar.SensorShapeId &&
                  collidee.Entity.Exists world &&
                  collidee.Entity.Is<PropDispatcher> world &&
                  match (collidee.Entity.GetPropPlus world).Prop.PropData with
                  | Portal _ -> false
                  | Sensor _ -> false
                  | _ -> true) then
                true
            else false

        static member Facets =
            [typeof<RigidBodyFacet>]

        override this.Initialize (avatar, entity) =
            let bodyShape =
                BodyShapes
                    [BodySphere { Radius = 0.160f; Center = v3 -0.016f -0.365f 0.0f; PropertiesOpt = Some { BodyShapeProperties.empty with BodyShapeId = avatar.CoreShapeId }}
                     BodySphere { Radius = 0.320f; Center = v3 -0.016f -0.365f 0.0f; PropertiesOpt = Some { BodyShapeProperties.empty with BodyShapeId = avatar.SensorShapeId; SensorOpt = Some true }}]
            [Entity.Perimeter := avatar.Perimeter
             Entity.Presence == Omnipresent
             Entity.FixedRotation == true
             Entity.GravityScale == 0.0f
             Entity.BodyShape := bodyShape
             Entity.UpdateEvent => Update
             Entity.BodyCollisionEvent =|> fun evt -> BodyCollision evt.Data |> signal
             Entity.BodySeparationEvent =|> fun evt -> BodySeparation evt.Data |> signal
             entity.Group.PostUpdateEvent => PostUpdate]

        override this.Message (avatar, message, entity, world) =
            let time = World.getUpdateTime world
            match message with
            | Update ->

                // update animation generally
                let velocity = entity.GetLinearVelocity world
                let speed = velocity.Magnitude
                let direction = Direction.ofVector3Biased velocity
                let avatar =
                    if speed > Constants.Field.AvatarIdleSpeedMax then
                        if direction <> avatar.Direction || avatar.CharacterAnimationType = IdleAnimation then
                            let avatar = Avatar.updateDirection (constant direction) avatar
                            Avatar.animate time WalkAnimation avatar
                        else avatar
                    else Avatar.animate time IdleAnimation avatar
                just avatar

            | PostUpdate ->

                // clear all temporary body shapes
                let avatar = Avatar.updateCollidedPropIds (constant []) avatar
                let avatar = Avatar.updateSeparatedPropIds (constant []) avatar
                just avatar

            | TryFace direction ->

                // update facing if enabled, speed is low, and direction pressed
                let avatar =
                    if  not (World.isSelectedScreenTransitioning world) &&
                        entity.GetEnabled world then
                        let velocity = entity.GetLinearVelocity world
                        let speed = velocity.Magnitude
                        if speed <= Constants.Field.AvatarIdleSpeedMax
                        then Avatar.updateDirection (constant direction) avatar
                        else avatar
                    else avatar
                just avatar

            | BodyCollision collision ->

                // add collided body shape
                let avatar =
                    if isIntersectedProp collision.BodyCollider collision.BodyCollidee avatar world then
                        let avatar = Avatar.updateCollidedPropIds (List.cons (collision.BodyCollidee.Entity.GetPropPlus world).Prop.PropId) avatar
                        let avatar = Avatar.updateIntersectedPropIds (List.cons (collision.BodyCollidee.Entity.GetPropPlus world).Prop.PropId) avatar
                        avatar
                    else avatar
                just avatar

            | BodySeparation separation ->

                // add separated body shape
                match separation with
                | BodySeparationImplicit physicsId ->
                    let entityOpt =
                        world |>
                        World.getEntities Simulants.FieldScene |>
                        Seq.filter (fun entity -> entity.Is<PropDispatcher> world && entity.GetPhysicsId world = physicsId) |>
                        Seq.tryHead
                    match entityOpt with
                    | Some entity ->
                        let propId = (entity.GetPropPlus world).Prop.PropId
                        let (separatedPropIds, intersectedPropIds) = List.split ((=) propId) avatar.IntersectedPropIds
                        let avatar = Avatar.updateIntersectedPropIds (constant intersectedPropIds) avatar
                        let avatar = Avatar.updateSeparatedPropIds ((@) separatedPropIds) avatar
                        just avatar
                    | None -> just avatar
                | BodySeparationExplicit explicit ->
                    if isIntersectedProp explicit.BodySeparator explicit.BodySeparatee avatar world then
                        let avatar = Avatar.updateSeparatedPropIds (List.cons (explicit.BodySeparatee.Entity.GetPropPlus world).Prop.PropId) avatar
                        let avatar = Avatar.updateIntersectedPropIds (List.remove ((=) (explicit.BodySeparatee.Entity.GetPropPlus world).Prop.PropId)) avatar
                        just avatar
                    else just avatar

            | Nil ->

                // nothing to do
                just avatar

        override this.Command (_, command, entity, world) =
            match command with
            | TryTravel force ->
                if  not (World.isSelectedScreenTransitioning world) &&
                    force <> v3Zero &&
                    entity.GetEnabled world then
                    let physicsId = Simulants.FieldSceneAvatar.GetPhysicsId world
                    let world = World.applyBodyForce force physicsId world
                    just world
                else just world

        override this.Physics (position, _, _, _, avatar, _, _) =
            let avatar = Avatar.updatePosition (constant position) avatar
            just avatar

        override this.View (avatar, entity, world) =
            if entity.GetVisible world then
                let mutable transform = entity.GetTransform world
                Render2d (transform.Elevation, transform.Position.Y, AssetTag.generalize avatar.AnimationSheet,
                    SpriteDescriptor
                        { Transform = transform
                          InsetOpt = ValueSome (getSpriteInset entity world)
                          Image = avatar.AnimationSheet
                          Color = Color.One
                          Blend = Transparent
                          Glow = Color.Zero
                          Flip = FlipNone })
            else View.empty