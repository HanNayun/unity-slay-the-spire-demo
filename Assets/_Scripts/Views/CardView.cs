using _Scripts.GameActions;
using _Scripts.General.ActionSystem;
using _Scripts.General.Utils;
using _Scripts.Models;
using _Scripts.Systems;
using TMPro;
using UnityEngine;

namespace _Scripts.Views
{
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

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;

    public Card Card { get; private set; }

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

        if (Card.ManualTargetEffect is not null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
    }

    private void OnMouseDrag()
    {
        if (!InteractionSystem.Instance.CanPlayerInteract()) return;

        if (Card.ManualTargetEffect is null)
        {
            transform.position = MouseUtil.GetMousePositionInWorld(-1);
        }
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

    private void OnMouseUp()
    {
        if (!InteractionSystem.Instance.CanPlayerInteract()) return;

        if (Card.ManualTargetEffect is null)
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) &&
                Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                ActionSystem.Instance.Perform(new PlayCardGA(Card));
            }
            else
            {
                transform.position = dragStartPosition;
                transform.rotation = dragStartRotation;
            }
        }
        else if (ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorld(-1), out EnemyView target)
                 && ManaSystem.Instance.HasEnoughMana(Card.Mana))
        {
            ActionSystem.Instance.Perform(new PlayCardGA(Card, target));
        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }

        InteractionSystem.Instance.IsPlayerDragging = false;
    }

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        mana.text = card.Mana.ToString();
        description.text = card.Description;
        image.sprite = card.Image;
        wrapper.SetActive(true);
    }
}
}