using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using UnityEngine;

namespace _Scripts.UI
{
public class EndTurnButtonUI : MonoBehaviour
{
    public void OnClick()
    {
        var action = new EnemyTurnGA();
        ActionSystem.Instance.Perform(action);
    }
}
}