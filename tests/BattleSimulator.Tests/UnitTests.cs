using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Tests;

public class UnitTests
{
    private static readonly Position Origin = new(0, 0);

    [Fact]
    public void NewUnit_StartsAtFullHealth()
    {
        var unit = new Infantry(Team.Alpha, Origin);

        Assert.Equal(100, unit.Health);
        Assert.True(unit.IsAlive);
    }

    [Fact]
    public void TakeDamage_NeverGoesBelowZero()
    {
        var scout = new Scout(Team.Alpha, Origin); // 50 HP

        var taken = scout.TakeDamage(500);

        Assert.Equal(50, taken);
        Assert.Equal(0, scout.Health);
        Assert.False(scout.IsAlive);
    }

    [Fact]
    public void Tank_ArmourAbsorbsFivePointsOfDamage()
    {
        var tank = new Tank(Team.Alpha, Origin);

        var taken = tank.TakeDamage(20);

        Assert.Equal(15, taken);
        Assert.Equal(145, tank.Health);
    }

    [Fact]
    public void Tank_AlwaysTakesAtLeastOneDamage()
    {
        var tank = new Tank(Team.Alpha, Origin);

        Assert.Equal(1, tank.TakeDamage(2));
    }

    [Fact]
    public void Artillery_DealsBonusDamageToInfantryOnly()
    {
        var artillery = new Artillery(Team.Alpha, Origin);

        Assert.Equal(37, artillery.CalculateDamage(new Infantry(Team.Bravo, Origin)));
        Assert.Equal(25, artillery.CalculateDamage(new Tank(Team.Bravo, Origin)));
    }

    [Fact]
    public void Scout_DealsDoubleDamageToArtilleryOnly()
    {
        var scout = new Scout(Team.Alpha, Origin);

        Assert.Equal(16, scout.CalculateDamage(new Artillery(Team.Bravo, Origin)));
        Assert.Equal(8, scout.CalculateDamage(new Infantry(Team.Bravo, Origin)));
    }

    [Fact]
    public void Polymorphism_SameCallGivesDifferentDamagePerUnitType()
    {
        Unit target = new Infantry(Team.Bravo, Origin);
        var attackers = new List<Unit>
        {
            new Infantry(Team.Alpha, Origin),
            new Tank(Team.Alpha, Origin),
            new Artillery(Team.Alpha, Origin),
            new Scout(Team.Alpha, Origin)
        };

        var damages = attackers.Select(a => a.CalculateDamage(target)).ToList();

        Assert.Equal(new[] { 12, 20, 37, 8 }, damages);
    }

    [Fact]
    public void CanAttack_TrueInsideRange_FalseOutsideRange()
    {
        var attacker = new Infantry(Team.Alpha, new Position(0, 0)); // range 2
        var near = new Infantry(Team.Bravo, new Position(2, 0));
        var far = new Infantry(Team.Bravo, new Position(3, 0));

        Assert.True(attacker.CanAttack(near));
        Assert.False(attacker.CanAttack(far));
    }

    [Fact]
    public void CanAttack_FalseAgainstOwnTeam()
    {
        var attacker = new Infantry(Team.Alpha, new Position(0, 0));
        var friend = new Infantry(Team.Alpha, new Position(1, 0));

        Assert.False(attacker.CanAttack(friend));
    }

    [Fact]
    public void CanAttack_FalseAgainstDeadTarget()
    {
        var attacker = new Infantry(Team.Alpha, new Position(0, 0));
        var target = new Scout(Team.Bravo, new Position(1, 0));
        target.TakeDamage(1000);

        Assert.False(attacker.CanAttack(target));
    }

    [Fact]
    public void Attack_OutOfRange_DealsNoDamage()
    {
        var attacker = new Infantry(Team.Alpha, new Position(0, 0));
        var target = new Infantry(Team.Bravo, new Position(10, 0));

        var damage = attacker.Attack(target);

        Assert.Equal(0, damage);
        Assert.Equal(100, target.Health);
    }
}
