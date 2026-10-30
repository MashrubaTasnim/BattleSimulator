using BattleSimulator.Core;
using BattleSimulator.Persistence;
using BattleSimulator.Simulation;

// Usage:
//   dotnet run                   run a battle (seed 42), console output only
//   dotnet run -- 7              use seed 7 (different starting positions)
//   dotnet run -- 7 --mongo      also save the battle to MongoDB
//   dotnet run -- --report       show statistics of the battles saved in MongoDB
var seed = 42;
foreach (var arg in args)
{
    if (int.TryParse(arg, out var parsed))
    {
        seed = parsed;
    }
}

var saveToMongo = args.Contains("--mongo");
var showReport = args.Contains("--report");

if (showReport)
{
    BattleReport.Print(MongoBattleStore.FromEnvironment());
    return;
}

var rng = new Random(seed);
var field = new Battlefield(width: 20, height: 10);
ArmyFactory.Deploy(field, Team.Alpha, rng);
ArmyFactory.Deploy(field, Team.Bravo, rng);

var loggers = new List<IBattleLogger> { new ConsoleBattleLogger() };
if (saveToMongo)
{
    loggers.Add(new MongoBattleLogger(MongoBattleStore.FromEnvironment(), seed));
}

var engine = new BattleEngine(field, new CompositeBattleLogger(loggers));
engine.Run();
