namespace BattleSimulator.Core;

/// <summary>Anything that can move across the battlefield.</summary>
public interface IMovable
{
    int MoveSpeed { get; }
    Position Position { get; }
    void MoveTo(Position destination);
}
