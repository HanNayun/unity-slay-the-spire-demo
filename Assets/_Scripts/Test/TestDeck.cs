using System.Collections.Generic;
using _Scripts.Data;
using _Scripts.Systems;
using UnityEngine;

public class TestDeck : MonoBehaviour
{
    [SerializeField]
    private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
    }
}