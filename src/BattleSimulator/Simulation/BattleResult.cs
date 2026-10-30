using BattleSimulator.Core;

namespace BattleSimulator.Simulation;

/// <summary>Outcome of a battle. Winner is null when the battle ends in a draw.</summary>
public sealed record BattleResult(Team? Winner, int TurnsPlayed, int AlphaSurvivors, int BravoSurvivors);
