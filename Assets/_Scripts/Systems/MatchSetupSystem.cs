using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    [SerializeField]
    private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
        DrawCardGA drawCardGa = new(5);
        ActionSystem.Instance.Perform(drawCardGa);
    }
}