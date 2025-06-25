using System;
using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    private const int MAX_MANA = 3;

    [SerializeField]
    private ManaUI manaUI;

    private int curMana;

    private int CurMana
    {
        get => curMana;
        set
        {
            curMana = value;
            manaUI.UpdateMana(CurMana);
        }
    }

    public bool HasEnoughMana(int amount) => CurMana >= amount;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.Post);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.Post);
    }

    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        CurMana -= spendManaGA.Amount;
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        CurMana = MAX_MANA;
        yield return null;
    }

    private void EnemyTurnPostReaction(EnemyTurnGA ga)
    {
        ActionSystem.Instance.AddReaction(new RefillManaGA());
    }
}