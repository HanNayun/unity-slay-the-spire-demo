using System.Collections;
using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using UnityEngine;

namespace _Scripts.Systems
{
public class EffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }

    private IEnumerator PerformEffectPerformer(PerformEffectGA effectGameAction)
    {
        GameAction gameAction =
            effectGameAction.Effect.GetGameAction(effectGameAction.Targets, HeroSystem.Instance.HeroView);
        ActionSystem.Instance.AddReaction(gameAction);
        yield return null;
    }
}
}