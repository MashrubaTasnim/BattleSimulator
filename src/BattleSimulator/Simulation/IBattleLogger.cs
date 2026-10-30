using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>
/// The engine reports what happens through this interface, so the output can go anywhere
/// (console now, a database later) without changing the engine.
/// </summary>
public interface IBattleLogger
{
    void BattleStarted(Battlefield field);
    void UnitMoved(int turn, Unit unit, Position from, Position to);
    void UnitAttacked(int turn, Unit attacker, Unit target, int damage);
    void UnitDestroyed(int turn, Unit unit);
    void TurnEnded(int turn, Battlefield field);
    void BattleEnded(BattleResult result);
}
