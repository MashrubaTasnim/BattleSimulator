using BattleSimulator.Core;

namespace BattleSimulator.Units;

/// <summary>Balanced front-line unit.</summary>
public sealed class Infantry : Unit
{
    public Infantry(Team team, Position start) : base(team, maxHealth: 100, start)
    {
    }

    public override char Symbol => 'I';
    public override int AttackPower => 12;
    public override int Range => 2;
    public override int MoveSpeed => 1;
}
