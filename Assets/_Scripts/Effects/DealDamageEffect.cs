using System.Collections.Generic;
using _Scripts.Models;
using UnityEngine;

public class DealDamageEffect : Effect
{
    [SerializeField]
    private int damageAmount;

    public override GameAction GetGameAction(List<CombatantView> targets)
    {
        return new DealDamageGA(damageAmount, targets);
    }
}