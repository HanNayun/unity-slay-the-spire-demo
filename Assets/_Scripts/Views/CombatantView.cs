using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Scripts.Views
{
public class CombatantView : MonoBehaviour
{
    private const int SIZE = 180;

    [SerializeField]
    private TMP_Text healthText;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public void SetupBase(int health, Sprite sprite)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = sprite;
        spriteRenderer.size = new Vector2(1, 1);
        UpdateHealthText();
        UpdateSpriteSize();
    }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }

        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth + "/" + MaxHealth;
    }

    private void UpdateSpriteSize()
    {
        Vector2 spriteSize = spriteRenderer.sprite.rect.size;
        float scale = SIZE / Math.Max(spriteSize.x, spriteSize.y);
        spriteRenderer.transform.localScale = new Vector3(scale, scale, 1);
    }
}
}