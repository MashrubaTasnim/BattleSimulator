using BattleSimulator.Core;

namespace BattleSimulator.Units;

/// <summary>Fragile long-range unit that deals bonus damage to Infantry.</summary>
public sealed class Artillery : Unit
{
    public Artillery(Team team, Position start) : base(team, maxHealth: 60, start)
    {
    }

    public override char Symbol => 'A';
    public override int AttackPower => 25;
    public override int Range => 5;
    public override int MoveSpeed => 1;

    public override int CalculateDamage(Unit target) =>
        target is Infantry ? AttackPower * 3 / 2 : AttackPower;
}
