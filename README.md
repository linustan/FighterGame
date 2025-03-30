# Fighter Game - Code Overview

This document provides an overview of the C# codebase structure for the Fighter Game.

## Project Architecture

The architecture enforces a clear separation of concerns between:

1. **Game Logic** - Core gameplay mechanics, state management, and business logic.
2. **Unity Behaviors** - MonoBehaviour components that connect game logic to Unity's rendering and input systems.

This separation allows for:
- Better testability of game logic independent of Unity.
- Cleaner code organization and maintenance.
- Potential reuse of game logic across different implementations, for example in a hypothetical backend.

## Key Components

### Game Logic (com.terallite.gamelogic)

The game logic layer contains the core mechanics and is organized into:

- **World** - Orchestrates the updates and spawning of Entities.
- **Entity System** - Provides a unified interface for all objects in the game world, including Fighters and projectiles.
- **Collision System** - Implements simple bounding-box collision between game entities.

### Unity Integration

Unity MonoBehaviours serve as the view/controller layer:
- Connect game logic to Unity's rendering pipeline.
- Handle Unity-specific lifecycle events.
- Process input through Unity's Input System.

## Design Patterns

The codebase implements several design patterns:

1. **Model-View-Controller (MVC)** - Separating game logic (model) from Unity behaviors (view/controller).
2. **State Pattern** - For game flow management.
5. **Observer Pattern** - For event handling and communication between systems.

## Getting Started

1. Open `FighterGame.sln` in your preferred IDE.
2. Explore the `Assets` folder for Unity MonoBehaviours.
3. The `com.terallite.gamelogic` project contains the core game systems.

## Future Improvements

- Further decoupling of systems for better modularity.
- More data-driven design for game entity properties.

## License

This is a personal educational project subject to the terms in the [LICENSE](LICENSE) file.
Personal use is permitted, but redistribution is prohibited. All original content is 
copyrighted by Terallite Kaihatsu, with derivative works used under fair use principles.
