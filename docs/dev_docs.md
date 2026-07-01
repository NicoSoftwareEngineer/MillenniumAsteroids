# Millennium Asteroids

## About the Game
Millennium Asteroids is a 2D arcade space shooter inspired by the classic game Asteroids. The player pilots the space ship and must navigate through space, shooting down incoming enemies to survive and accumulate points. The game features standard arcade mechanics, including a lives system, scoring, a start menu, and a game over state.

## Technologies Used
* **Game Engine:** Godot Engine
* **Programming Language:** C# (.NET)
* **Architecture:** Scene-based architecture using Godot's node system (.tscn files for the main game, HUD, menus, and individual entities like the player ship and lasers).
* **Physics and Collisions:** 2D physics system utilizing custom collision shapes for different enemy variants (small and big TIE Fighters).
* **Assets:** Custom 2D sprites for ships and projectiles, and integrated custom typography (Star Jedi fonts).

## Folder Structure
```text
MillenniumAsteroids/    # Root directory of the project
|-- assets/             # Contains all game assets (sprites, images)
|-- scenes/             # Godot scene files (.tscn) for levels and entities
|-- scripts/            # C# scripts containing game logic and mechanics
```

## Millennium Physics (`scripts/Millennium.cs`)
* Operates as a `CharacterBody2D` using `MoveAndSlide()`.
* **Rotation:** Directly modifies `Rotation` based on steering input.
* **Acceleration:** Adds thrust along the local X-axis.
* **Speed Cap:** Restricts maximum velocity using `LimitLength()`.
* **Friction:** Applies `MoveToward()` to decelerate when idle.
* **Bounds:** Teleports across screen edges via `ScreenWrap()`.


## Tie Fighter Physics (`scripts/TieFighter.cs`)
* Operates as an `Area2D` with manual position updates.
* **Movement:** Travels continuously forward based on rotation.
* **Speed:** Randomly calculated upon spawn based on size.
* **Bounds:** Uses manual `ScreenWrap()` to loop across screen edges.
* **Collisions:** Uses `OnBodyEntered` signals to detect and kill the player.

## Central Management (`scripts/Game.cs`)
* Acts as the main event hub listening to child and UI signals.
* Updates HUD elements directly, bypassing signals.

## Player (`scripts/Millennium.cs`)
* **`LaserShot`:** Emits when firing; `Game.cs` listens to add the laser to the scene.
* **`Died`:** Emits on death; `Game.cs` handles lives and the game over state.

## Enemy (`scripts/TieFighter.cs`)
* **`Exploded`:** Emits on destruction; `Game.cs` listens to update the score and spawn debris.
* **No attack signals:** Uses `OnBodyEntered` to detect the player and directly calls `Die()`.

## Projectile (`scripts/Laser.cs`)
* **No signals:** Uses engine collisions (`LaserEntered`) to detect enemies and directly calls `Explode()`.

## UI (`scripts/StartMenu.cs`, `scripts/GameOverMenu.cs`)
* **`StartGame` / `RestartGame`:** Emitted on button presses; `Game.cs` listens to initialize or reset the match.