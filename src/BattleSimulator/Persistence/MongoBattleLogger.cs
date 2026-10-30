using BattleSimulator.Core;
using BattleSimulator.Simulation;
using BattleSimulator.Units;

namespace BattleSimulator.Persistence;

/// <summary>An IBattleLogger that collects the events of a battle and saves them to MongoDB at the end.</summary>
public sealed class MongoBattleLogger : IBattleLogger
{
    private readonly MongoBattleStore _store;
    private readonly BattleRecord _record;

    public MongoBattleLogger(MongoBattleStore store, int seed)
    {
        _store = store;
        _record = new BattleRecord { Seed = seed, StartedAtUtc = DateTime.UtcNow };
    }

    public void BattleStarted(Battlefield field)
    {
        _record.StartedAtUtc = DateTime.UtcNow;
    }

    // Movement is not stored to keep documents small.
    public void UnitMoved(int turn, Unit unit, Position from, Position to)
    {
    }

    public void UnitAttacked(int turn, Unit attacker, Unit target, int damage) =>
        _record.Events.Add(new BattleEventRecord
        {
            Turn = turn,
            Type = "attack",
            Actor = attacker.Name,
            Target = target.Name,
            Damage = damage
        });

    public void UnitDestroyed(int turn, Unit unit) =>
        _record.Events.Add(new BattleEventRecord
        {
            Turn = turn,
            Type = "destroyed",
            Actor = unit.Name
        });

    public void TurnEnded(int turn, Battlefield field)
    {
    }

    public void BattleEnded(BattleResult result)
    {
        _record.Winner = result.Winner?.ToString() ?? "Draw";
        _record.TurnsPlayed = result.TurnsPlayed;
        _record.AlphaSurvivors = result.AlphaSurvivors;
        _record.BravoSurvivors = result.BravoSurvivors;

        try
        {
            _store.Battles.InsertOne(_record);
            Console.WriteLine($"Battle saved to MongoDB (id: {_record.Id}).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not save to MongoDB (is it running?): {ex.Message}");
        }
    }
}
