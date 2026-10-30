# Battle Simulator

A turn-based strategy battle simulation written in **C# (.NET 10)** to practise and demonstrate object-oriented design.
Two armies (Alpha and Bravo) fight on a grid until one side is destroyed or the turn limit is reached.
Battles can be saved to **MongoDB** and analysed with a small report. Core logic is covered by **xUnit** tests.

## OOP concepts used

| Concept | Where |
|---|---|
| **Abstraction / abstract class** | `Unit` defines the shared behaviour; each unit type supplies its own stats |
| **Inheritance** | `Infantry`, `Tank`, `Artillery`, `Scout` inherit from `Unit` |
| **Polymorphism** | `CalculateDamage` and `TakeDamage` are overridden (Artillery vs Infantry bonus, Scout vs Artillery bonus, Tank armour) |
| **Interfaces** | `IAttacker`, `IMovable`, `IBattleLogger` |
| **Encapsulation** | Health and position can only change through methods (`TakeDamage`, `MoveTo`) |
| **Dependency injection** | `BattleEngine` receives a `Battlefield` and an `IBattleLogger` through its constructor |
| **Composite pattern** | `CompositeBattleLogger` forwards events to several loggers (console + MongoDB) |
| **Factory** | `ArmyFactory` builds and deploys an army |

## Unit types

| Unit | HP | Attack | Range | Speed | Special |
|---|---|---|---|---|---|
| Infantry (I) | 100 | 12 | 2 | 1 | Balanced |
| Tank (T) | 160 | 20 | 2 | 2 | Armour reduces each hit by 5 |
| Artillery (A) | 60 | 25 | 5 | 1 | +50% damage vs Infantry |
| Scout (S) | 50 | 8 | 1 | 3 | x2 damage vs Artillery |

## Run

```bash
cd src/BattleSimulator
dotnet run                  # default seed (42), console output
dotnet run -- 7             # another seed = different starting positions
dotnet run -- 7 --mongo     # also save the battle to MongoDB
dotnet run -- --report      # statistics of all saved battles
```

MongoDB is optional. By default it connects to `mongodb://localhost:27017`
(set the `MONGO_URI` environment variable to use another server).
Battles are stored in the `battle_simulator` database, `battles` collection.

## Tests

```bash
dotnet test
```

## Project structure

```
src/BattleSimulator/
  Core/          Team, Position, IAttacker, IMovable
  Units/         Unit (abstract) + Infantry, Tank, Artillery, Scout
  Simulation/    Battlefield, BattleEngine, ArmyFactory, loggers
  Persistence/   MongoDB store, logger and report
tests/BattleSimulator.Tests/   xUnit tests
```

## Author

Mashruba Tasnem - [GitHub](https://github.com/MashrubaTasnim)
