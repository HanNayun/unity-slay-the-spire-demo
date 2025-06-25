using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Models;
using UnityEngine;

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
                CradViewCreator.Instance.CreateCardView(card, transform.position, Quaternion.identity);
            StartCoroutine(handView.AddCard(cardView));
        }
    }
}