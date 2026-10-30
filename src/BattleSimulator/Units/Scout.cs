using BattleSimulator.Core;

namespace BattleSimulator.Units;

/// <summary>Fast, weak flanker that deals double damage to Artillery.</summary>
public sealed class Scout : Unit
{
    public Scout(Team team, Position start) : base(team, maxHealth: 50, start)
    {
    }

    public override char Symbol => 'S';
    public override int AttackPower => 8;
    public override int Range => 1;
    public override int MoveSpeed => 3;

    public override int CalculateDamage(Unit target) =>
        target is Artillery ? AttackPower * 2 : AttackPower;
}
