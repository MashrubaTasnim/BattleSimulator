using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>Sends every battle event to several loggers (e.g. console and MongoDB at the same time).</summary>
public sealed class CompositeBattleLogger : IBattleLogger
{
    private readonly IReadOnlyList<IBattleLogger> _loggers;

    public CompositeBattleLogger(IEnumerable<IBattleLogger> loggers)
    {
        _loggers = loggers.ToList();
    }

    public void BattleStarted(Battlefield field)
    {
        foreach (var logger in _loggers)
        {
            logger.BattleStarted(field);
        }
    }

    public void UnitMoved(int turn, Unit unit, Position from, Position to)
    {
        foreach (var logger in _loggers)
        {
            logger.UnitMoved(turn, unit, from, to);
        }
    }

    public void UnitAttacked(int turn, Unit attacker, Unit target, int damage)
    {
        foreach (var logger in _loggers)
        {
            logger.UnitAttacked(turn, attacker, target, damage);
        }
    }

    public void UnitDestroyed(int turn, Unit unit)
    {
        foreach (var logger in _loggers)
        {
            logger.UnitDestroyed(turn, unit);
        }
    }

    public void TurnEnded(int turn, Battlefield field)
    {
        foreach (var logger in _loggers)
        {
            logger.TurnEnded(turn, field);
        }
    }

    public void BattleEnded(BattleResult result)
    {
        foreach (var logger in _loggers)
        {
            logger.BattleEnded(result);
        }
    }
}
