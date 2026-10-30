using BattleSimulator.Core;

namespace BattleSimulator.Units;

/// <summary>
/// Base class for every unit. Concrete unit types (Infantry, Tank, ...) inherit from it
/// and override the parts that make them different (stats, damage rules, armour).
/// </summary>
public abstract class Unit : IAttacker, IMovable
{
    private static int _nextId;

    protected Unit(Team team, int maxHealth, Position start)
    {
        Id = Interlocked.Increment(ref _nextId);
        Team = team;
        MaxHealth = maxHealth;
        Health = maxHealth;
        Position = start;
    }

    public int Id { get; }
    public Team Team { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    public Position Position { get; private set; }

    public bool IsAlive => Health > 0;
    public string Kind => GetType().Name;
    public string Name => $"{Team} {Kind} #{Id}";

    // Each subclass must define its own stats.
    public abstract char Symbol { get; }
    public abstract int AttackPower { get; }
    public abstract int Range { get; }
    public abstract int MoveSpeed { get; }

    /// <summary>Damage this unit deals to the target. Subclasses can override (polymorphism).</summary>
    public virtual int CalculateDamage(Unit target) => AttackPower;

    /// <summary>Applies damage and returns the amount actually taken. Subclasses can override (e.g. armour).</summary>
    public virtual int TakeDamage(int amount)
    {
        var actual = Math.Clamp(amount, 0, Health);
        Health -= actual;
        return actual;
    }

    public bool CanAttack(Unit target) =>
        IsAlive
        && target.IsAlive
        && target.Team != Team
        && Position.DistanceTo(target.Position) <= Range;

    public int Attack(Unit target)
    {
        if (!CanAttack(target))
        {
            return 0;
        }

        return target.TakeDamage(CalculateDamage(target));
    }

    public void MoveTo(Position destination) => Position = destination;

    public override string ToString() => $"{Name} [{Health}/{MaxHealth} HP @ {Position}]";
}
