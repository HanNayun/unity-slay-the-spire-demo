using _Scripts.General;
using _Scripts.Models;
using _Scripts.Views;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Creators
{
public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField]
    private CardView cardViewPrefab;

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        cardView.Setup(card);
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(Vector3.one, .15f);
        return cardView;
    }
}
}