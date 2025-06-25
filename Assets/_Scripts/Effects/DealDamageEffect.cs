using System.Collections.Generic;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField]
    private int damageAmount;

    public override GameAction GetGameAction()
    {
        return new DealDamageGA(damageAmount, new List<CombatantView>(EnemySystem.Instance.Enemies));
    }
}