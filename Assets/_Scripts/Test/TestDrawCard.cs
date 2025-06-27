using _Scripts.Creators;
using _Scripts.Data;
using _Scripts.Models;
using _Scripts.Views;
using UnityEngine;

namespace _Scripts.Test
{
public class TestDrawCard : MonoBehaviour
{
    [SerializeField]
    private HandView handView;

    [SerializeField]
    private CardData cardData;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var card = new Card(cardData);
            CardView cardView =
                CardViewCreator.Instance.CreateCardView(card, transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));
        }
    }
}
}