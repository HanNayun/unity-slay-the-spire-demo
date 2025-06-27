using _Scripts.Data;
using TMPro;
using UnityEngine;

namespace _Scripts.Views
{
public class EnemyView : CombatantView
{
    [SerializeField]
    private TMP_Text attckPowerText;

    public int AttackPower { get; set; }

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        UpdateAttackPowerText();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    private void UpdateAttackPowerText()
    {
        attckPowerText.text = "ATK: " + AttackPower;
    }
}
}