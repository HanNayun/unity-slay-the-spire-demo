using System;
using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    [SerializeField]
    private HeroData heroData;

    [SerializeField]
    private List<EnemyData> enemyDatas;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        CardSystem.Instance.Setup(heroData.Deck);
        EnemySystem.Instance.Setup(enemyDatas);
        
        ActionSystem.Instance.Perform(new RefillManaGA(),
            () => { ActionSystem.Instance.Perform(new DrawCardGA(CardSystem.BASIC_DRAW_AMOUNT)); }
        );
    }
}