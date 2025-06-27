using System.Collections.Generic;
using _Scripts.Models;
using _Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Systems
{
public class PerkSystem : General.Singleton<PerkSystem>
{
    [SerializeField]
    private PerksUI perksUI;

    private readonly List<Perk> perks = new();

    public void AddPerk(List<Perk> perks)
    {
        perks.ForEach(AddPerk);
    }

    public void AddPerk(Perk perk)
    {
        perks.Add(perk);
        perk.OnAdd();
        perksUI.AddPerkUI(perk);
    }

    public void RemovePerk(Perk perk)
    {
        perks.Remove(perk);
        perksUI.RemovePerkUI(perk);
        perk.OnRemove();
    }
}
}