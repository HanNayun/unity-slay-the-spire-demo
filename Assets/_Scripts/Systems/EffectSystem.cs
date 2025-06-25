using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.GameActions;
using UnityEngine;

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
        var gameAction = effectGameAction.Effect.GetGameAction(effectGameAction.Targets);
        ActionSystem.Instance.AddReaction(gameAction);
        yield return null;
    }
}