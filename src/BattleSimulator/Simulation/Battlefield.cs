using System.Text;
using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>A rectangular grid that holds all units and knows the movement/targeting rules.</summary>
public sealed class Battlefield
{
    private readonly List<Unit> _units = new();

    public Battlefield(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }
    public int Height { get; }

    public IReadOnlyList<Unit> Units => _units;
    public IEnumerable<Unit> AliveUnits => _units.Where(u => u.IsAlive);

    public bool InBounds(Position p) => p.X >= 0 && p.X < Width && p.Y >= 0 && p.Y < Height;

    public bool IsFree(Position p) => InBounds(p) && !AliveUnits.Any(u => u.Position == p);

    public void Add(Unit unit)
    {
        if (!IsFree(unit.Position))
        {
            throw new InvalidOperationException($"Cell {unit.Position} is not available.");
        }

        _units.Add(unit);
    }

    public int CountAlive(Team team) => AliveUnits.Count(u => u.Team == team);

    public Unit? NearestEnemy(Unit unit) =>
        AliveUnits
            .Where(u => u.Team != unit.Team)
            .OrderBy(u => u.Position.DistanceTo(unit.Position))
            .ThenBy(u => u.Id)
            .FirstOrDefault();

    /// <summary>Moves the unit towards the destination (up to its MoveSpeed). Returns true if it moved.</summary>
    public bool MoveTowards(Unit unit, Position destination)
    {
        var moved = false;

        for (var step = 0; step < unit.MoveSpeed; step++)
        {
            // Already close enough to attack: stop.
            if (unit.Position.DistanceTo(destination) <= unit.Range)
            {
                break;
            }

            var next = NextStep(unit.Position, destination);
            if (next is null)
            {
                break;
            }

            unit.MoveTo(next.Value);
            moved = true;
        }

        return moved;
    }

    private Position? NextStep(Position from, Position to)
    {
        var dx = Math.Sign(to.X - from.X);
        var dy = Math.Sign(to.Y - from.Y);

        // Prefer the diagonal step, then straight along each axis.
        var candidates = new[]
        {
            new Position(from.X + dx, from.Y + dy),
            new Position(from.X + dx, from.Y),
            new Position(from.X, from.Y + dy)
        };

        foreach (var candidate in candidates)
        {
            if (candidate != from && IsFree(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>Text picture of the field. Uppercase = Alpha, lowercase = Bravo.</summary>
    public string Render()
    {
        var grid = new char[Height, Width];
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                grid[y, x] = '.';
            }
        }

        foreach (var unit in AliveUnits)
        {
            grid[unit.Position.Y, unit.Position.X] = unit.Team == Team.Alpha
                ? char.ToUpperInvariant(unit.Symbol)
                : char.ToLowerInvariant(unit.Symbol);
        }

        var sb = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                sb.Append(grid[y, x]).Append(' ');
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
