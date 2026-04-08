using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUnit : MonoBehaviour
{
    public enum SkillType
    {
        Damage,
        TeamHeal,
        TeamShield,
        AoE
    }

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
    public int skillManaCost = 20;

    [Header("Skill")]
    public SkillType skillType = SkillType.Damage;
    public int healAmount = 30;

    [Header("Status")]
    public bool shieldActive = false;

    [Header("UI")]
    public Image hpBarImage;
    public Image manaBarImage;
    public Image highlightImage;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;

    private void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        UpdateHPUI();
        UpdateManaUI();
        SetHighlight(false);
    }

    public void TakeDamage(int damage)
    {
        if (shieldActive)
        {
            shieldActive = false;
            Debug.Log(unitName + " blocked the damage.");
            return;
        }

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPUI();
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPUI();
    }

    public void RestoreMana(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        UpdateManaUI();
    }

    public bool TryUseMana()
    {
        if (currentMana < skillManaCost)
        {
            return false;
        }

        currentMana -= skillManaCost;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        UpdateManaUI();
        return true;
    }

    public void AddDamageBoost(int amount)
    {
        attackDamage += amount;
    }

    public void GiveShield()
    {
        shieldActive = true;
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

    public void UpdateHPUI()
    {
        switch (hpBarImage)
        {
            case Image image:
                image.fillAmount = (float)currentHP / maxHP;
                break;
        }

        switch (hpText)
        {
            case TextMeshProUGUI text:
                text.text = currentHP + "/" + maxHP;
                break;
        }
    }

    public void UpdateManaUI()
    {
        switch (manaBarImage)
        {
            case Image image:
                image.fillAmount = (float)currentMana / maxMana;
                break;
        }

        switch (manaText)
        {
            case TextMeshProUGUI text:
                text.text = currentMana + "/" + maxMana;
                break;
        }
    }

    public void SetHighlight(bool value)
    {
        switch (highlightImage)
        {
            case Image image:
                image.gameObject.SetActive(value);
                break;
        }
    }

    public string GetSkillDescription()
    {
        if (skillType == SkillType.Damage)
        {
            return "Deal " + skillDamage + " damage to one enemy. Cost: " + skillManaCost + " MP.";
        }

        if (skillType == SkillType.TeamHeal)
        {
            return "Heal all allies for " + healAmount + " HP. Cost: " + skillManaCost + " MP.";
        }

        if (skillType == SkillType.TeamShield)
        {
            return "All allies ignore the next hit. Cost: " + skillManaCost + " MP.";
        }

        if (skillType == SkillType.AoE)
        {
            return "Deal " + skillDamage + " damage to all enemies. Cost: " + skillManaCost + " MP.";
        }

        return "";
    }
}