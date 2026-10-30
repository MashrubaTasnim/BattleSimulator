using BattleSimulator.Core;
using BattleSimulator.Units;

namespace BattleSimulator.Simulation;

/// <summary>Creates a standard army and places it on its own side of the battlefield.</summary>
public static class ArmyFactory
{
    private const int InfantryCount = 5;
    private const int TankCount = 2;
    private const int ArtilleryCount = 2;
    private const int ScoutCount = 2;

    public static void Deploy(Battlefield field, Team team, Random rng)
    {
        for (var i = 0; i < InfantryCount; i++)
        {
            field.Add(new Infantry(team, FindFreeCell(field, team, rng)));
        }

        for (var i = 0; i < TankCount; i++)
        {
            field.Add(new Tank(team, FindFreeCell(field, team, rng)));
        }

        for (var i = 0; i < ArtilleryCount; i++)
        {
            field.Add(new Artillery(team, FindFreeCell(field, team, rng)));
        }

        for (var i = 0; i < ScoutCount; i++)
        {
            field.Add(new Scout(team, FindFreeCell(field, team, rng)));
        }
    }

    // Alpha starts on the left 3 columns, Bravo on the right 3 columns.
    private static Position FindFreeCell(Battlefield field, Team team, Random rng)
    {
        while (true)
        {
            var depth = rng.Next(0, 3);
            var x = team == Team.Alpha ? depth : field.Width - 1 - depth;
            var candidate = new Position(x, rng.Next(0, field.Height));

            if (field.IsFree(candidate))
            {
                return candidate;
            }
        }
    }
}
