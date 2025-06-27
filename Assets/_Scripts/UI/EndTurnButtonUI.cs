using UnityEngine;

public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        var action = new EnemyTurnGA();
        ActionSystem.Instance.Perform(action);
    }
}