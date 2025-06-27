using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Data;
using _Scripts.GameActions;
using _Scripts.General;
using _Scripts.Models;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace _Scripts.Systems
{
    public class CardSystem : Singleton<CardSystem>
    {
        public const int BASIC_DRAW_AMOUNT = 5;

        [SerializeField]
        private HandView handView;

        [SerializeField]
        private Transform drawPileTransform;

        [SerializeField]
        private Transform discardPileTransform;

        private readonly List<Card> drawPile = new();
        private readonly List<Card> discardPile = new();
        private readonly List<Card> hand = new();

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<DrawCardGA>(DrawCardPerformer);
            ActionSystem.AttachPerformer<DiscardAllCardGA>(DiscardAllCardPerformer);
            ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);

            ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.Pre);
            ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.Post);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<DrawCardGA>();
            ActionSystem.DetachPerformer<DiscardAllCardGA>();
            ActionSystem.DetachPerformer<PlayCardGA>();

            ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPreReaction, ReactionTiming.Pre);
            ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.Post);
        }

        public void Setup(List<CardData> cardDatas)
        {
            drawPile.AddRange(cardDatas.Select(data => new Card(data)));
        }

        // Reaction
        private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGa)
        {
            DiscardAllCardGA discardAllCardGa = new();
            ActionSystem.Instance.AddReaction(discardAllCardGa);
        }

        private void EnemyTurnPostReaction(EnemyTurnGA gameAction)
        {
            ActionSystem.Instance.AddReaction(new DrawCardGA(BASIC_DRAW_AMOUNT));
        }

        // Performers
        private IEnumerator DrawCardPerformer(DrawCardGA drawCardGa)
        {
            int actualAmount = Mathf.Min(drawCardGa.Amount, drawPile.Count);
            int notDrawCount = drawCardGa.Amount - actualAmount;
            for (int i = 0; i < actualAmount; i++) yield return DrawCard();

            if (notDrawCount <= 0) yield break;

            RefillDeck();
            int canDrawCount = Math.Min(notDrawCount, drawPile.Count);
            for (int i = 0; i < canDrawCount; i++)
            {
                yield return DrawCard();
            }
        }

        private IEnumerator DiscardAllCardPerformer(DiscardAllCardGA discardAllCardGa)
        {
            foreach (CardView cardView in hand.Select(card => handView.RemoveCard(card)))
            {
                yield return DiscardCard(cardView);
            }

            hand.Clear();
        }

        private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
        {
            hand.Remove(playCardGA.Card);
            CardView cardView = handView.RemoveCard(playCardGA.Card);
            yield return DiscardCard(cardView);

            ActionSystem.Instance.AddReaction(new SpendManaGA(playCardGA.Card.Mana));

            if (playCardGA.Card.ManualTargetEffect is not null)
            {
                var effectReaction = new PerformEffectGA(playCardGA.Card.ManualTargetEffect,
                    new List<CombatantView> { playCardGA.Target });
                ActionSystem.Instance.AddReaction(effectReaction);
            }

            foreach (AutoTargetEffect effectWrapper in playCardGA.Card.OtherEffects)
            {
                List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
                PerformEffectGA reaction = new(effectWrapper.Effect, targets);
                ActionSystem.Instance.AddReaction(reaction);
            }
        }

        //#endregion
        /// <summary>
        ///     Make sure <see cref="drawPile" /> is not empty when before draw
        /// </summary>
        /// <param name="cardView"></param>
        /// <returns></returns>
        private IEnumerator DiscardCard(CardView cardView)
        {
            discardPile.Add(cardView.Card);
            cardView.transform.DOMove(Vector3.zero, .15f);
            TweenerCore<Vector3, Vector3, VectorOptions> tween =
                cardView.transform.DOMove(discardPileTransform.position, .15f);
            yield return tween.WaitForCompletion();
            Destroy(cardView.gameObject);
        }

        private IEnumerator DrawCard()
        {
            Card card = drawPile.Draw();
            hand.Add(card);
            CardView cardView =
                CradViewCreator.Instance.CreateCardView(card, drawPileTransform.position, drawPileTransform.rotation);
            yield return handView.AddCard(cardView);
        }

        private void RefillDeck()
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
        }
    }
}