using _Scripts.General;
using _Scripts.Models;
using UnityEngine;

public class CardHoverSystem : Singleton<CardHoverSystem>
{
    [SerializeField]
    private CardView hoverCardView;

    public void ShowCardView(Card card, Vector3 position)
    {
        hoverCardView.Setup(card);
        hoverCardView.transform.position = position;
        hoverCardView.gameObject.SetActive(true);
    }

    public void Hide()
    {
        hoverCardView.gameObject.SetActive(false);
    }
}