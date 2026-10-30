using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>Prints battle events to the console.</summary>
public sealed class ConsoleBattleLogger : IBattleLogger
{
    private readonly int _boardEvery;

    public ConsoleBattleLogger(int boardEvery = 5)
    {
        _boardEvery = boardEvery;
    }

    public void BattleStarted(Battlefield field)
    {
        Console.WriteLine("=== BATTLE START ===");
        Console.WriteLine("Uppercase = Alpha, lowercase = Bravo | I=Infantry T=Tank A=Artillery S=Scout");
        Console.WriteLine($"Alpha: {field.CountAlive(Team.Alpha)} units | Bravo: {field.CountAlive(Team.Bravo)} units");
        Console.WriteLine();
        Console.WriteLine(field.Render());
    }

    // Movement is not printed to keep the output readable.
    public void UnitMoved(int turn, Unit unit, Position from, Position to)
    {
    }

    public void UnitAttacked(int turn, Unit attacker, Unit target, int damage) =>
        Console.WriteLine(
            $"[Turn {turn:D2}] {attacker.Name} hits {target.Name} for {damage} ({target.Health}/{target.MaxHealth} HP left)");

    public void UnitDestroyed(int turn, Unit unit) =>
        Console.WriteLine($"[Turn {turn:D2}] *** {unit.Name} destroyed ***");

    public void TurnEnded(int turn, Battlefield field)
    {
        if (turn % _boardEvery == 0)
        {
            Console.WriteLine();
            Console.WriteLine($"--- Board after turn {turn} ---");
            Console.WriteLine(field.Render());
        }
    }

    public void BattleEnded(BattleResult result)
    {
        Console.WriteLine();
        Console.WriteLine("=== BATTLE OVER ===");
        Console.WriteLine(result.Winner is null
            ? "Result: DRAW (turn limit reached)"
            : $"Winner: {result.Winner}");
        Console.WriteLine($"Turns played: {result.TurnsPlayed}");
        Console.WriteLine($"Survivors -> Alpha: {result.AlphaSurvivors}, Bravo: {result.BravoSurvivors}");
    }
}
