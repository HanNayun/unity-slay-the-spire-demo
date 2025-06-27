using _Scripts.General;
using _Scripts.Models;
using DG.Tweening;
using UnityEngine;

public class CradViewCreator : Singleton<CradViewCreator>
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