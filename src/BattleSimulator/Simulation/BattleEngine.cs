using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>Runs the battle turn by turn until one side is destroyed or the turn limit is reached.</summary>
public sealed class BattleEngine
{
    private readonly Battlefield _field;
    private readonly IBattleLogger _logger;
    private readonly int _maxTurns;

    public BattleEngine(Battlefield field, IBattleLogger logger, int maxTurns = 100)
    {
        _field = field;
        _logger = logger;
        _maxTurns = maxTurns;
    }

    public BattleResult Run()
    {
        _logger.BattleStarted(_field);

        var turn = 0;
        while (turn < _maxTurns
               && _field.CountAlive(Team.Alpha) > 0
               && _field.CountAlive(Team.Bravo) > 0)
        {
            turn++;
            PlayTurn(turn);
            _logger.TurnEnded(turn, _field);
        }

        var result = BuildResult(turn);
        _logger.BattleEnded(result);
        return result;
    }

    private void PlayTurn(int turn)
    {
        // The side that acts first alternates every turn, so neither side has a fixed advantage.
        var first = turn % 2 == 1 ? Team.Alpha : Team.Bravo;

        var order = _field.AliveUnits
            .OrderBy(u => u.Team == first ? 0 : 1)
            .ThenBy(u => u.Id)
            .ToList();

        foreach (var unit in order)
        {
            if (!unit.IsAlive)
            {
                continue; // destroyed earlier in this same turn
            }

            var enemy = _field.NearestEnemy(unit);
            if (enemy is null)
            {
                return;
            }

            if (unit.CanAttack(enemy))
            {
                var damage = unit.Attack(enemy);
                _logger.UnitAttacked(turn, unit, enemy, damage);

                if (!enemy.IsAlive)
                {
                    _logger.UnitDestroyed(turn, enemy);
                }
            }
            else
            {
                var from = unit.Position;
                if (_field.MoveTowards(unit, enemy.Position))
                {
                    _logger.UnitMoved(turn, unit, from, unit.Position);
                }
            }
        }
    }

    private BattleResult BuildResult(int turnsPlayed)
    {
        var alpha = _field.CountAlive(Team.Alpha);
        var bravo = _field.CountAlive(Team.Bravo);

        Team? winner = null;
        if (alpha > 0 && bravo == 0)
        {
            winner = Team.Alpha;
        }
        else if (bravo > 0 && alpha == 0)
        {
            winner = Team.Bravo;
        }

        return new BattleResult(winner, turnsPlayed, alpha, bravo);
    }
}
