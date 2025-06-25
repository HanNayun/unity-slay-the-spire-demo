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

    [SerializeField]
    private LayerMask dropLayer;

    public Card Card { get; private set; }
    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

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
        if (!InteractionSystem.Instance.CanPlayerHover()) return;

        var position = new Vector3(transform.position.x, -2, 0);
        CardHoverSystem.Instance.ShowCardView(Card, position);
        wrapper.SetActive(false);
    }

    private void OnMouseExit()
    {
        if (!InteractionSystem.Instance.CanPlayerHover()) return;

        wrapper.SetActive(true);
        CardHoverSystem.Instance.Hide();
    }

    private void OnMouseDown()
    {
        if (!InteractionSystem.Instance.CanPlayerInteract()) return;
        InteractionSystem.Instance.IsPlayerDragging = true;
        wrapper.gameObject.SetActive(true);
        CardHoverSystem.Instance.Hide();
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        transform.position = MouseUtil.GetMousePositionInWorld(-1);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnMouseDrag()
    {
        if (!InteractionSystem.Instance.CanPlayerInteract()) return;
        transform.position = MouseUtil.GetMousePositionInWorld(-1);
    }

    private void OnMouseUp()
    {
        if (!InteractionSystem.Instance.CanPlayerInteract()) return;

        if (ManaSystem.Instance.HasEnoughMana(Card.Mana) &&
            Physics.Raycast(transform.position, Vector3.forward, out var hit, 10f, dropLayer))
        {
            PlayCardGA playCardGA = new(Card);
            ActionSystem.Instance.Perform(playCardGA);
        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }

        InteractionSystem.Instance.IsPlayerDragging = false;
    }
}