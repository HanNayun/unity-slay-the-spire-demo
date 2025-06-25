using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField]
    private TMP_Text attckPowerText;

    public int AttackPower { get; set; }

    public void Setup()
    {
        AttackPower = 10;
        UpdateAttackPowerText();
        Setup(100, null);
    }

    private void UpdateAttackPowerText()
    {
        attckPowerText.text = "ATK: " + AttackPower;
    }
}