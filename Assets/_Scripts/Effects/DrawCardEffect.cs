using System.Collections.Generic;
using _Scripts.Models;
using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField]
    private int amount;

    public override GameAction GetGameAction(List<CombatantView> targets)
    {
        DrawCardGA drawCardGA = new(amount);
        return drawCardGA;
    }
}