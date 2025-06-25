using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    [SerializeField]
    private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);

        ActionSystem.Instance.Perform(new RefillManaGA(),
            () => { ActionSystem.Instance.Perform(new DrawCardGA(CardSystem.BASIC_DRAW_AMOUNT)); }
        );
    }
}