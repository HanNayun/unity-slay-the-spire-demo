using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text healthText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public void Setup(int health, Sprite sprite)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = sprite;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth + "/" + MaxHealth;
    }
}