using BattleSimulator.Core;

namespace BattleSimulator.Units;

/// <summary>Heavy unit: lots of health, hits hard, and armour reduces incoming damage.</summary>
public sealed class Tank : Unit
{
    private const int Armor = 5;

    public Tank(Team team, Position start) : base(team, maxHealth: 160, start)
    {
    }

    public override char Symbol => 'T';
    public override int AttackPower => 20;
    public override int Range => 2;
    public override int MoveSpeed => 2;

    // Armour: every hit does at least 1 damage, but Armor points are absorbed.
    public override int TakeDamage(int amount) => base.TakeDamage(Math.Max(1, amount - Armor));
}
