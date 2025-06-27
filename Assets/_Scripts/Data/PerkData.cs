using _Scripts.Models;
using SerializeReferenceEditor;
using UnityEngine;

namespace _Scripts.Data
{
[CreateAssetMenu(menuName = "Data/Perk")]
public class PerkData : ScriptableObject
{
    [field: SerializeField]
    public Sprite Image { get; private set; }

    [field: SerializeReference, SR]
    public PerkCondition PerkCondition { get; private set; }

    [field: SerializeReference, SR]
    public AutoTargetEffect AutoTargetEffect { get; private set; }

    [field: SerializeField]
    public bool IsUseAutoTarget { get; private set; } = true;

    [field: SerializeField]
    public bool IsUseCasterAsTarget { get; private set; }
}
}