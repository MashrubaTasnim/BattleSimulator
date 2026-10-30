using MongoDB.Driver;

namespace BattleSimulator.Persistence;

/// <summary>Gives access to the MongoDB collection that stores battles.</summary>
public sealed class MongoBattleStore
{
    public const string DefaultUri = "mongodb://localhost:27017";

    public MongoBattleStore(
        string connectionString,
        string database = "battle_simulator",
        string collection = "battles")
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(3);

        var client = new MongoClient(settings);
        Battles = client.GetDatabase(database).GetCollection<BattleRecord>(collection);
    }

    public IMongoCollection<BattleRecord> Battles { get; }

    /// <summary>Uses the MONGO_URI environment variable if set, otherwise local MongoDB.</summary>
    public static MongoBattleStore FromEnvironment() =>
        new(Environment.GetEnvironmentVariable("MONGO_URI") ?? DefaultUri);
}
