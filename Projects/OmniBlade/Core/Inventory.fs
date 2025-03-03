﻿// Nu Game Engine.
// Copyright (C) Bryan Edds, 2013-2020.

namespace OmniBlade
open System
open Prime
open Nu
open OmniBlade

type Advent =
    | DebugSwitch
    | DebugSwitch2
    | Opened of Guid
    | ShadeRecruited
    | MaelRecruited
    | RiainRecruited
    | PericRecruited
    | MadTrixterDefeated
    | HeavyArmorosDefeated
    | AraneaImplicitumDefeated
    | TombSealed
    | ForestSealed
    | FactorySealed
    | MountainSealed
    | DeadSeaSealed
    | RuinsSealed
    | DesertSealed
    | DarkCastleSealed
    | SeasonsSealed
    | VolcanoSealed
    | KylaAdvent of int

[<RequireQualifiedAccess>]
module Advents =

    let empty =
        Set.empty<Advent>

    let initial =
        Set.ofList
            [TombSealed
             ForestSealed
             FactorySealed
             MountainSealed
             DeadSeaSealed
             RuinsSealed
             DesertSealed
             DarkCastleSealed
             SeasonsSealed
             VolcanoSealed
             KylaAdvent 0]

type WeaponType =
    | Bare
    | ShortSword
    | Dagger
    | OakRod
    | OakBow
    | Paws
    | BronzeSword
    | BronzeKatana
    | BronzeRod
    | LightBow
    | Claws
    | IronSword
    | IronKatana
    | SightedBow
    | IvoryRod
    | Fangs

type ArmorType =
    | MicroFur
    | TinMail
    | CottonVest
    | CottonRobe
    | ThinFur
    | BronzeMail
    | LeatherVest
    | LeatherRobe
    | ThickFur
    | IronMail
    | RubberVest
    | SilkRobe
    | ToughHide
    | SteelMail
    | HuntingVest
    | FeatherRobe
    | StoneHide

type AccessoryType =
    | IronBrace
    | SnakeCharm
    | SilverRing
    | SilverWatch

type WeaponSubtype =
    | Melee
    | Sword
    | Knife
    | Rod
    | Bow

type ArmorSubtype =
    | Robe
    | Vest
    | Mail
    | Pelt

type EquipmentType =
    | WeaponType of WeaponType
    | ArmorType of ArmorType
    | AccessoryType of AccessoryType

type ConsumableType =
    | GreenHerb
    | RedHerb
    | GoldHerb
    | Remedy
    | Ether
    | HighEther
    | TurboEther
    | Revive

type KeyItemType =
    | BrassKey
    | IronKey
    | CopperKey
    | AluminumKey
    | PewterKey
    | SteelKey

type ItemType =
    | Consumable of ConsumableType
    | Equipment of EquipmentType
    | KeyItem of KeyItemType
    | Stash of int

    static member getName item =
        match item with
        | Consumable ty -> scstringm ty
        | Equipment ty -> match ty with WeaponType ty -> scstringm ty | ArmorType ty -> scstringm ty | AccessoryType ty -> scstringm ty
        | KeyItem ty -> scstringm ty
        | Stash gold -> string gold + "G"

    static member sortItems items =
        Seq.sortBy
            (function
             | (Consumable _, _) -> 0
             | (Equipment _, _) -> 1
             | (KeyItem _, _) -> 2
             | (Stash _, _) -> 3)
            items

    static member filterSellableItems items =
        Seq.choose
            (function
             | (Equipment _, _) | (Consumable _, _) as item -> Some item
             | (KeyItem _, _) | (Stash _, _) -> None)
            items

type [<NoComparison>] PrizePool =
    { Consequents : Advent Set
      Items : ItemType list
      Gold : int
      Exp : int }

    static member empty =
        { Consequents = Set.empty; Items = []; Gold = 0; Exp = 0 }

// TODO: make this abstract due to item limit constraints.
type [<NoComparison>] Inventory =
    { Items : Map<ItemType, int>
      Gold : int }

    static member getKeyItems inv =
        inv.Items |>
        Map.toArray |>
        Array.map (function (KeyItem keyItemType, count) -> Some (keyItemType, count) | _ -> None) |>
        Array.definitize |>
        Map.ofArray
        
    static member getConsumables inv =
        inv.Items |>
        Map.toArray |>
        Array.map (function (Consumable consumableType, count) -> Some (consumableType, count) | _ -> None) |>
        Array.definitize |>
        Map.ofArray

    static member containsItem item inventory =
        match Map.tryFind item inventory.Items with
        | Some itemCount when itemCount > 0 -> true
        | _ -> false

    static member containsItems items inventory =
        List.forall (flip Inventory.containsItem inventory) items

    static member canAddItem item inventory =
        match item with
        | Equipment _ | Consumable _ | KeyItem _ ->
            match Map.tryFind item inventory.Items with
            | Some itemCount -> itemCount < Constants.Gameplay.ItemLimit
            | None -> true
        | Stash _ -> true

    static member tryAddItem item inventory =
        match item with
        | Equipment _ | Consumable _ | KeyItem _ ->
            match Map.tryFind item inventory.Items with
            | Some itemCount ->
                if itemCount < Constants.Gameplay.ItemLimit then
                    let inventory = { inventory with Items = Map.add item (inc itemCount) inventory.Items }
                    (true, inventory)
                else (false, inventory)
            | None -> (true, { inventory with Items = Map.add item 1 inventory.Items })
        | Stash gold -> (true, { inventory with Gold = inventory.Gold + gold })

    static member tryAddItems items inventory =
        List.foldBack (fun item (failures, inventory) ->
            match Inventory.tryAddItem item inventory with
            | (true, inventory) -> (failures, inventory)
            | (false, inventory) -> (Some item :: failures, inventory))
            items ([], inventory)

    static member tryRemoveItem item inventory =
        match Map.tryFind item inventory.Items with
        | Some itemCount when itemCount > 1 ->
            (true, { inventory with Items = Map.add item (dec itemCount) inventory.Items })
        | Some itemCount when itemCount = 1 ->
            (true, { inventory with Items = Map.remove item inventory.Items })
        | _ -> (false, inventory)

    static member indexItems (inventory : Inventory) =
        inventory.Items |>
        Map.toSeq |>
        Seq.map (fun (ty, ct) -> List.init ct (fun _ -> ty)) |>
        Seq.concat |>
        Seq.indexed

    static member tryIndexItem index inventory =
        let items = Inventory.indexItems inventory
        let tail = Seq.trySkip index items
        Seq.tryHead tail

    static member getItemCount itemType inventory =
        match Map.tryFind itemType inventory.Items with
        | Some count -> count
        | None -> 0

    static member updateGold updater (inventory : Inventory) =
        { inventory with Gold = updater inventory.Gold }

    static member empty =
        { Items = Map.empty; Gold = 0 }

    static member initial =
        { Items = Map.singleton (Consumable GreenHerb) 3; Gold = 0 }