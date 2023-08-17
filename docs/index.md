﻿# Trapped In A Dungeon - Documentation

## Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Entities](#entities)
3. [Player](#player)
5. [Items & Player Inventory](#items)
6. [Weapons, Skills & Projectiles](#weapons-skills--consumables)
6. [Enemies](#enemies)
7. [Loot](#loot)
8. [Level Generation](#level-generation)
9. [Game Manager & Save System](#game-manager--save-system)

## Architecture Overview
This is an high level overview of the various (scripting) components of the game.

1. **Core** - _Core systems / framework of the game. <br/>_
   Most other scripts depends on scripts inside this folder. <br/>
   1. **Enemies** - _Scripts for enemy states and behaviours_
   2. **Entities** - _General scripts relating and affecting entities such as status effects, bodies etc._
   3. **Item** - _General scripts for the items_
   4. **Save** - _Scripts for saving and loading_
   5. **Sound** - _Scripts for playing sound effects_
   6. **UI** - _General scripts for User Interfaces_
   7. **Utils** - _Utility scripts that makes my life easier and reduce some boilerplate code_
2. **Enemies** - _Scripts for implementation of the enemy behaviours_
3. **Entities** - _Scripts for cool effects on entities such as damage numbers and death animations_
4. **Item** - _Scripts for controlling item prefabs / connecting the Core/Item scripts to the Unity_ 
5. **Level** - _Scripts relating to the level generation_
6. **Loot** - _Scripts for controlling loot drops_
7. **PlayerScripts** - _Scripts related to the player such as movement and inventory_
8. **Projectile** - _Scripts relating to projectiles_
9. **UI** - _Various scripts for specific implementation of the UI_
10. **Weapon** - _Scripts for controlling weapon prefabs / Connecting the Core/Weapon scripts to the Unity_
11. **Consumable** - _Scripts controlling to consumable items_
12. **GameManager.cs** - _This is the GameManager which controls the flow of the game_
13. **GameControls.cs** - _Please ignore, this is an autogenerated script generated by the Unity's new input system_

## Entities
## Player
## Items
## Weapons, Skills & Consumables
## Enemies
## Loot
## Level Generation
## Game Manager & Save System