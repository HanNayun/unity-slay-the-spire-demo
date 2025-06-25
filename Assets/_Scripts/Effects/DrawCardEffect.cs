using UnityEngine;

public class DrawCardEffect : Effect
{
    [SerializeField]
    private int amount;

    public override GameAction GetGameAction()
    {
        DrawCardGA drawCardGA = new(amount);
        return drawCardGA;
    }
}