using _Scripts.General;

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