using System.Collections.Generic;
using _Scripts.Data;
using UnityEngine;

namespace _Scripts.Models
{
    public class Card
    {
        private readonly CardData data;

        public Card(CardData curData)
        {
            data = curData;
            Mana = data.Mana;
        }

        public string Title => data.Title;
        public string Description => data.Description;
        public Sprite Image => data.Image;
        public int Mana { get; private set; }
        public Effect ManualTargetEffect => data.ManualTargetEffect;
        public List<AutoTargetEffect> OtherEffects => data.OtherEffects;
    }
}