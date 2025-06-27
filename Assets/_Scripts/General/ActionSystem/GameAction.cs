using System.Collections.Generic;

namespace _Scripts.General.ActionSystem
{
    public abstract class GameAction
    {
        public List<GameAction> PrevActions { get; private set; } = new();
        public List<GameAction> PerformActions { get; private set; } = new();
        public List<GameAction> PostActions { get; private set; } = new();
    }
}