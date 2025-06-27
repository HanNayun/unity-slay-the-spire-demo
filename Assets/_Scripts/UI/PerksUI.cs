using System.Collections.Generic;
using System.Linq;
using _Scripts.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Scripts.UI
{
public class PerksUI : MonoBehaviour
{
    [SerializeField]
    private PerkUI perkPrefab;

    private readonly List<PerkUI> perkUIs = new();

    public void AddPerkUI(Perk perk)
    {
        var perkUI = Instantiate(perkPrefab);
        perkUI.Setup(perk);
        perkUIs.Add(perkUI);
        perkUI.transform.SetParent(transform);
    }

    public void RemovePerkUI(Perk perk)
    {
        var perkUI = perkUIs.FirstOrDefault(perkUI => perkUI.Perk == perk);
        if (perkUI is null) return;

        perkUIs.Remove(perkUI);
        Destroy(perkUI.gameObject);
    }
}
}