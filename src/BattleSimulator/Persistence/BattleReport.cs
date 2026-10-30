using MongoDB.Driver;

namespace BattleSimulator.Persistence;

/// <summary>Reads saved battles from MongoDB and prints simple statistics.</summary>
public static class BattleReport
{
    public static void Print(MongoBattleStore store)
    {
        try
        {
            var battles = store.Battles.Find(_ => true).ToList();
            if (battles.Count == 0)
            {
                Console.WriteLine("No battles saved yet. Run: dotnet run -- 7 --mongo");
                return;
            }

            Console.WriteLine($"Battles saved: {battles.Count}");

            foreach (var group in battles.GroupBy(b => b.Winner).OrderBy(g => g.Key))
            {
                Console.WriteLine($"  {group.Key}: {group.Count()} win(s)");
            }

            Console.WriteLine($"Average turns per battle: {battles.Average(b => b.TurnsPlayed):F1}");

            var longest = battles.OrderByDescending(b => b.TurnsPlayed).First();
            Console.WriteLine($"Longest battle: seed {longest.Seed}, {longest.TurnsPlayed} turns");

            var topAttacker = battles
                .SelectMany(b => b.Events)
                .Where(e => e.Type == "attack")
                .GroupBy(e => e.Actor)
                .Select(g => new { Name = g.Key, Damage = g.Sum(e => e.Damage) })
                .OrderByDescending(x => x.Damage)
                .FirstOrDefault();

            if (topAttacker is not null)
            {
                Console.WriteLine($"Top damage dealer overall: {topAttacker.Name} ({topAttacker.Damage} damage)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not read from MongoDB (is it running?): {ex.Message}");
        }
    }
}
