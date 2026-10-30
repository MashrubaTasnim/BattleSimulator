using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BattleSimulator.Persistence;

/// <summary>One saved battle (stored as one MongoDB document).</summary>
public sealed class BattleRecord
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int Seed { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public string Winner { get; set; } = "";
    public int TurnsPlayed { get; set; }
    public int AlphaSurvivors { get; set; }
    public int BravoSurvivors { get; set; }
    public List<BattleEventRecord> Events { get; set; } = new();
}

/// <summary>One event inside a saved battle (an attack or a destroyed unit).</summary>
public sealed class BattleEventRecord
{
    public int Turn { get; set; }
    public string Type { get; set; } = "";
    public string Actor { get; set; } = "";
    public string? Target { get; set; }
    public int Damage { get; set; }
}
