using BattleSimulator.Core;
using BattleSimulator.Simulation;
using BattleSimulator.Units;

namespace BattleSimulator.Tests;

public class BattlefieldTests
{
    [Fact]
    public void Add_ToOccupiedCell_Throws()
    {
        var field = new Battlefield(10, 10);
        field.Add(new Infantry(Team.Alpha, new Position(1, 1)));

        Assert.Throws<InvalidOperationException>(
            () => field.Add(new Infantry(Team.Bravo, new Position(1, 1))));
    }

    [Fact]
    public void Add_OutsideTheGrid_Throws()
    {
        var field = new Battlefield(10, 10);

        Assert.Throws<InvalidOperationException>(
            () => field.Add(new Infantry(Team.Alpha, new Position(10, 0))));
    }

    [Fact]
    public void MoveTowards_InfantryMovesOneCell()
    {
        var field = new Battlefield(20, 10);
        var infantry = new Infantry(Team.Alpha, new Position(0, 0));
        field.Add(infantry);

        var moved = field.MoveTowards(infantry, new Position(10, 0));

        Assert.True(moved);
        Assert.Equal(new Position(1, 0), infantry.Position);
    }

    [Fact]
    public void MoveTowards_ScoutMovesUpToThreeCells()
    {
        var field = new Battlefield(20, 10);
        var scout = new Scout(Team.Alpha, new Position(0, 0));
        field.Add(scout);

        field.MoveTowards(scout, new Position(10, 0));

        Assert.Equal(new Position(3, 0), scout.Position);
    }

    [Fact]
    public void MoveTowards_StopsOnceTargetIsInRange()
    {
        var field = new Battlefield(20, 10);
        var infantry = new Infantry(Team.Alpha, new Position(0, 0)); // range 2
        field.Add(infantry);

        var moved = field.MoveTowards(infantry, new Position(2, 0));

        Assert.False(moved);
        Assert.Equal(new Position(0, 0), infantry.Position);
    }

    [Fact]
    public void MoveTowards_DoesNotMoveIntoAnOccupiedCell()
    {
        var field = new Battlefield(20, 10);
        var infantry = new Infantry(Team.Alpha, new Position(0, 0));
        var blocker = new Infantry(Team.Alpha, new Position(1, 0));
        field.Add(infantry);
        field.Add(blocker);

        var moved = field.MoveTowards(infantry, new Position(10, 0));

        Assert.False(moved);
        Assert.Equal(new Position(0, 0), infantry.Position);
    }

    [Fact]
    public void NearestEnemy_ReturnsTheClosestEnemy()
    {
        var field = new Battlefield(20, 10);
        var me = new Infantry(Team.Alpha, new Position(0, 0));
        var far = new Infantry(Team.Bravo, new Position(8, 0));
        var near = new Infantry(Team.Bravo, new Position(3, 0));
        var friend = new Infantry(Team.Alpha, new Position(1, 0));
        field.Add(me);
        field.Add(far);
        field.Add(near);
        field.Add(friend);

        Assert.Same(near, field.NearestEnemy(me));
    }

    [Fact]
    public void CountAlive_IgnoresDestroyedUnits()
    {
        var field = new Battlefield(20, 10);
        var a = new Infantry(Team.Alpha, new Position(0, 0));
        var b = new Infantry(Team.Alpha, new Position(0, 1));
        field.Add(a);
        field.Add(b);

        b.TakeDamage(1000);

        Assert.Equal(1, field.CountAlive(Team.Alpha));
    }
}
