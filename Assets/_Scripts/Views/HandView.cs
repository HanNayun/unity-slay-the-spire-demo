using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Models;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

namespace _Scripts.Views
{
public class HandView : MonoBehaviour
{
    [SerializeField]
    private SplineContainer splineContainer;

    private readonly List<CardView> cards = new();

    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        yield return UpdateCardPositions(.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null)
        {
            return null;
        }

        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions(.15f));
        return cardView;
    }

    private CardView GetCardView(Card card)
    {
        return cards.FirstOrDefault(cardView => cardView.Card == card);
    }

    private IEnumerator UpdateCardPositions(float durationSecond)
    {
        if (cards.Count is 0)
        {
            yield break;
        }

        float cardSpacing = 1f / 10f;
        float firstCardPosition = .5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 normal = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-Vector3.back, normal);
            CardView card = cards[i];
            card.transform.DOMove(transform.position + splinePosition + .01f * i * Vector3.back, durationSecond);
            card.transform.DORotateQuaternion(rotation, durationSecond);
        }

        yield return new WaitForSeconds(durationSecond);
    }
}
}