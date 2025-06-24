using System.Collections.Generic;

public abstract class GameAction
{
    public List<GameAction> PrevActions { get; private set; } = new();
    public List<GameAction> PerformActions { get; private set; } = new();
    public List<GameAction> PostActions { get; private set; } = new();
}