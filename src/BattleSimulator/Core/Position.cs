namespace BattleSimulator.Core;

/// <summary>A cell on the battlefield grid.</summary>
public readonly record struct Position(int X, int Y)
{
    /// <summary>Chebyshev distance: a diagonal step counts as one step.</summary>
    public int DistanceTo(Position other) =>
        Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));

    public override string ToString() => $"({X},{Y})";
}
