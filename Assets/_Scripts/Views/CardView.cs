using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;

    [SerializeField]
    private TMP_Text mana;

    [SerializeField]
    private TMP_Text description;

    [SerializeField]
    private SpriteRenderer image;

    [SerializeField]
    private GameObject wrapper;

    public Card Card { get; private set; }

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        mana.text = card.Mana.ToString();
        description.text = card.Description;
        image.sprite = card.Image;
        wrapper.SetActive(true);
    }

    private void OnMouseEnter()
    {
        var position = new Vector3(transform.position.x, -2, 0);
        CardHoverSystem.Instance.ShowCardView(Card, position);
        wrapper.SetActive(false);
    }

    private void OnMouseExit()
    {
        wrapper.SetActive(true);
        CardHoverSystem.Instance.Hide();
    }
}