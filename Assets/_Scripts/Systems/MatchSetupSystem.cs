using System.Collections.Generic;
using System.Linq;
using _Scripts.Data;
using _Scripts.GameActions;
using _Scripts.General;
using _Scripts.General.ActionSystem;
using _Scripts.Models;
using UnityEngine;

namespace _Scripts.Systems
{
public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    [SerializeField]
    private HeroData heroData;

    [SerializeField]
    private List<EnemyData> enemyDatas;

    [SerializeField]
    private List<PerkData> perkDatas;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        CardSystem.Instance.Setup(heroData.Deck);
        EnemySystem.Instance.Setup(enemyDatas);
        PerkSystem.Instance.AddPerk(perkDatas.Select(perk => new Perk(perk)).ToList());

        ActionSystem.Instance.Perform(new RefillManaGA(),
            () => { ActionSystem.Instance.Perform(new DrawCardGA(CardSystem.BASIC_DRAW_AMOUNT)); }
        );
    }
}
}