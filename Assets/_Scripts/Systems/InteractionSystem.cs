using _Scripts.General;
using _Scripts.General.ActionSystem;

namespace _Scripts.Systems
{
public class InteractionSystem : Singleton<InteractionSystem>
{
    public bool IsPlayerDragging { get; set; }

    public bool CanPlayerInteract()
    {
        return !ActionSystem.Instance.IsPerforming;
    }

    public bool CanPlayerHover()
    {
        return !IsPlayerDragging;
    }
}
}