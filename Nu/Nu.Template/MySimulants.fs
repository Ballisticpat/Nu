﻿namespace MyGame
open Nu

[<RequireQualifiedAccess>]
module Simulants =

    // the handle for the game
    let Game = Default.Game

    // same as above, but for the splash screen
    let Splash = Screen "Splash"

    // same as above, but for the title screen and its children
    let Title = Screen "Title"
    let TitleGui = Title / "Gui"
    let TitlePlay = TitleGui / "Play"
    let TitleCredits = TitleGui / "Credits"
    let TitleExit = TitleGui / "Exit"

    // credits screen handles
    let Credits = Screen "Credits"
    let CreditsGui = Credits / "Gui"
    let CreditsBack = CreditsGui / "Back"

    // gameplay screen handles
    let Gameplay = Default.Screen
    let Level = Gameplay / "Level"
    let Scene = Gameplay / "Scene"
    let Player = Scene / "Player"
    let Back = Scene / "Back"