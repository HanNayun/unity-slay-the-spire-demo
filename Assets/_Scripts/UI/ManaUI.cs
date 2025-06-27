using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
public class ManaUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text manaText;

    public void UpdateMana(int mana)
    {
        manaText.text = mana.ToString();
    }
}
}