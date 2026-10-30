using BattleSimulator.Core;
using BattleSimulator.Simulation;
using BattleSimulator.Units;

namespace BattleSimulator.Tests;

public class BattleEngineTests
{
    private static BattleResult RunBattle(int seed, IBattleLogger logger, int maxTurns = 100)
    {
        var rng = new Random(seed);
        var field = new Battlefield(20, 10);
        ArmyFactory.Deploy(field, Team.Alpha, rng);
        ArmyFactory.Deploy(field, Team.Bravo, rng);

        return new BattleEngine(field, logger, maxTurns).Run();
    }

    [Fact]
    public void SameSeed_GivesSameResult()
    {
        var first = RunBattle(42, new RecordingLogger());
        var second = RunBattle(42, new RecordingLogger());

        Assert.Equal(first, second);
    }

    [Fact]
    public void Run_CallsStartAndEndExactlyOnce()
    {
        var logger = new RecordingLogger();

        RunBattle(42, logger);

        Assert.Equal(1, logger.Started);
        Assert.Equal(1, logger.Ended);
    }

    [Fact]
    public void Run_ReportsEveryAttackAndDestroyedUnit()
    {
        var logger = new RecordingLogger();

        var result = RunBattle(42, logger);

        Assert.True(logger.Attacks > 0);
        // 11 units per army: every unit that is not a survivor was destroyed.
        Assert.Equal(22 - result.AlphaSurvivors - result.BravoSurvivors, logger.Destroyed);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(42)]
    [InlineData(123)]
    public void Run_EndsWithOneSideDestroyedOrADraw(int seed)
    {
        var result = RunBattle(seed, new RecordingLogger());

        if (result.Winner is null)
        {
            Assert.Equal(100, result.TurnsPlayed);
        }
        else if (result.Winner == Team.Alpha)
        {
            Assert.True(result.AlphaSurvivors > 0);
            Assert.Equal(0, result.BravoSurvivors);
        }
        else
        {
            Assert.True(result.BravoSurvivors > 0);
            Assert.Equal(0, result.AlphaSurvivors);
        }
    }

    [Fact]
    public void Run_StopsAtTheTurnLimit()
    {
        var result = RunBattle(42, new RecordingLogger(), maxTurns: 1);

        Assert.Equal(1, result.TurnsPlayed);
        Assert.Null(result.Winner);
    }

    // A fake logger that just counts events, so the tests can check what the engine reported.
    private sealed class RecordingLogger : IBattleLogger
    {
        public int Started { get; private set; }
        public int Ended { get; private set; }
        public int Attacks { get; private set; }
        public int Destroyed { get; private set; }

        public void BattleStarted(Battlefield field) => Started++;

        public void UnitMoved(int turn, Unit unit, Position from, Position to)
        {
        }

        public void UnitAttacked(int turn, Unit attacker, Unit target, int damage) => Attacks++;

        public void UnitDestroyed(int turn, Unit unit) => Destroyed++;

        public void TurnEnded(int turn, Battlefield field)
        {
        }

        public void BattleEnded(BattleResult result) => Ended++;
    }
}
