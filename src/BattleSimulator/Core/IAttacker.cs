using BattleSimulator.Units;

namespace BattleSimulator.Core;

/// <summary>Anything that can attack another unit.</summary>
public interface IAttacker
{
    int AttackPower { get; }
    int Range { get; }
    bool CanAttack(Unit target);

    /// <summary>Attacks the target and returns the damage actually dealt.</summary>
    int Attack(Unit target);
}
