using System.Collections.Generic;
using _Scripts.Models;
using SerializeReferenceEditor;
using UnityEngine;

namespace _Scripts.Data
{
[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField]
    public int Mana { get; private set; }

    [field: SerializeField]
    public string Title { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public Sprite Image { get; private set; }

    [field: SerializeField]
    public List<AutoTargetEffect> OtherEffects { get; private set; }

    [field: SerializeReference, SR]
    public Effect ManualTargetEffect { get; private set; }
}
}