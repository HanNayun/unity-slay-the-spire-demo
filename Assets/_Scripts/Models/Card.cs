using UnityEngine;

public class Card
{
    public string Title => data.Title;
    public string Description => data.Description;
    public Sprite Image => data.Image;
    public int Mana { get; private set; }

    private readonly CardData data;

    public Card(CardData curData)
    {
        data = curData;
        Mana = data.Mana;
    }
}