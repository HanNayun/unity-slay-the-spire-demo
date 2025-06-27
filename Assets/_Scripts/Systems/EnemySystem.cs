using System.Collections;
using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.General;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField]
    private EnemyBoardView enemyBoardView;

    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
        ActionSystem.AttachPerformer<AttackHeroGA>(EnemyAttackPerformer);
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (EnemyData enemyData in enemyDatas) enemyBoardView.AddEnemy(enemyData);
    }

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGa)
    {
        foreach (EnemyView enemy in enemyBoardView.EnemyViews)
        {
            AttackHeroGA attackGA = new(enemy);
            ActionSystem.Instance.AddReaction(attackGA);
        }

        yield return null;
    }

    private IEnumerator EnemyAttackPerformer(AttackHeroGA attackGA)
    {
        EnemyView attacker = attackGA.Attacker;
        TweenerCore<Vector3, Vector3, VectorOptions> tween =
            attacker.transform.DOMoveX(attacker.transform.position.x - 1f, .15f);
        yield return tween.WaitForCompletion();
        tween = attacker.transform.DOMoveX(attacker.transform.position.x + 1f, .15f);
        yield return tween.WaitForCompletion();

        DealDamageGA dealDamageGA = new(attacker.AttackPower, new List<CombatantView> { HeroSystem.Instance.HeroView });
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }

    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.Target);
    }
}