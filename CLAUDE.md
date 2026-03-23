# LeidenRooftopRun

A 2D Unity platformer game built with Unity (URP, 2D packages).

## Project Structure

- `Assets/Scripts/` — all C# game scripts
- `Assets/Scenes/` — game levels (`Level1.unity`, more to come)

## Scripts Overview

| Script | Purpose |
|---|---|
| `PlayerMovement.cs` | Horizontal movement + space-to-jump; dies on Enemy tag, loads scene 0 |
| `EnemyMove.cs` | Patrol enemy bouncing between `leftLimit` and `rightLimit` |
| `CameraFollow.cs` | Camera tracks player position each frame |
| `Finish.cs` | Trigger zone that loads scene 1 (next level) on Player tag |

## Key Tags

- `Ground` — surfaces the player can jump from
- `Enemy` — kills/restarts the player on contact
- `Player` — used by the Finish trigger

## Unity Version & Packages

- Unity 6 (URP 17.4.0)
- Input System 1.19.0 (legacy `Input.GetAxis` still used in PlayerMovement)
- 2D Animation, Aseprite importer, Tilemap, SpriteShape
- App UI 2.1.7 (UI Toolkit framework)

## Conventions

- Scripts use `MonoBehaviour` with standard Unity lifecycle methods
- Scene loading uses index-based `SceneManager.LoadScene(int)`
- Physics via `Rigidbody2D.linearVelocity` (Unity 6 API)
