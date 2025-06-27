using _Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
public class PerkUI : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    public Perk Perk { get; private set; }

    public void Setup(Perk perk)
    {
        Perk = perk;
        iconImage.sprite = perk.Image;
    }
}
}