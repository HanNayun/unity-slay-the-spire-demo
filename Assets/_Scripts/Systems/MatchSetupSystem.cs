using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    [SerializeField]
    private HeroData heroData;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        CardSystem.Instance.Setup(heroData.Deck);
        ActionSystem.Instance.Perform(new RefillManaGA(),
            () => { ActionSystem.Instance.Perform(new DrawCardGA(CardSystem.BASIC_DRAW_AMOUNT)); }
        );
    }
}