using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField]
    private HandView handView;

    [SerializeField]
    private Transform drawPileTransform;

    [SerializeField]
    private Transform discardPileTransform;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand = new();

    public void Setup(List<CardData> cardDatas)
    {
        drawPile.AddRange(cardDatas.Select(data => new Card(data)));
    }

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

    // Reaction
    private void EnemyTurnPreReaction(EnemyTurnGA enemyTurnGa)
    {
        DiscardAllCardGA discardAllCardGa = new();
        ActionSystem.Instance.AddReaction(discardAllCardGa);
    }

    private void EnemyTurnPostReaction(EnemyTurnGA gameAction)
    {
        DrawCardGA drawCardGa = new(5);
        ActionSystem.Instance.AddReaction(drawCardGa);
    }

    // Performers
    private IEnumerator DrawCardPerformer(DrawCardGA drawCardGa)
    {
        int actualAmount = Mathf.Min(drawCardGa.Amount, drawPile.Count);
        int notDrawCount = drawCardGa.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++) yield return DrawCard();

        if (notDrawCount <= 0) yield break;

        RefillDeck();
        var canDrawCount = Math.Min(notDrawCount, drawPile.Count);
        for (int i = 0; i < canDrawCount; i++)
            yield return DrawCard();
    }

    private IEnumerator DiscardAllCardPerformer(DiscardAllCardGA discardAllCardGa)
    {
        foreach (Card card in hand)
        {
            discardPile.Add(card);
            var cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }

        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        var cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);
    }

    //#endregion
    private IEnumerator DiscardCard(CardView cardView)
    {
        cardView.transform.DOMove(Vector3.zero, .15f);
        var tween = cardView.transform.DOMove(discardPileTransform.position, .15f);
        yield return tween.WaitForCompletion();
        discardPile.Add(cardView.Card);
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