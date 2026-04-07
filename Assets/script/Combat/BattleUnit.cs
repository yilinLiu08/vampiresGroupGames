using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [Header("Info")]
    public string unitName;
    public bool isPlayer;

    [Header("Stats")]
    public int maxHP = 100;
    public int currentHP = 100;
    public int attackDamage = 20;
    public int skillDamage = 35;

    [Header("Mana")]
    public int maxMana = 100;
    public int currentMana = 100;

    [Header("UI")]
    public Image hpBarImage;
    public Image highlightImage;

    private void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        UpdateHPBar();
        SetHighlight(false);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHPBar();
    }

    public void RestoreMana(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    public void AddDamageBoost(int amount)
    {
        attackDamage += amount;
    }

    public void UseFruit(Fruit fruit)
    {
        if (fruit == null)
        {
            return;
        }

        foreach (FruitEffect effect in fruit.effects)
        {
            if (effect == FruitEffect.Heal)
            {
                Heal(fruit.healAmount);
            }

            if (effect == FruitEffect.DamageBoost)
            {
                AddDamageBoost(fruit.damageBoostAmount);
            }

            if (effect == FruitEffect.ManaRestore)
            {
                RestoreMana(fruit.manaAmount);
            }
        }

        Debug.Log(unitName + " used fruit: " + fruit.itemName);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public void UpdateHPBar()
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = (float)currentHP / maxHP;
        }
    }

    public void SetHighlight(bool value)
    {
        if (highlightImage != null)
        {
            highlightImage.gameObject.SetActive(value);
        }
    }
}