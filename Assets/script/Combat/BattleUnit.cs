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
    public Button targetButton;

    private float damageBoostMultiplier = 1f;
    private int damageBoostTurnsRemaining = 0;

    private int poisonDamagePerTurn = 0;
    private int poisonTurnsRemaining = 0;

    private int clotTurnsRemaining = 0;

    private float nauseaFailChance = 0f;
    private int nauseaTurnsRemaining = 0;

    private void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        UpdateHPUI();
        UpdateManaUI();
        SetHighlight(false);
        HideTargetButton();
    }

    public bool CanReceiveFruit(Fruit fruit)
    {
        if (fruit == null)
        {
            return false;
        }

        if (IsDead())
        {
            return false;
        }

        if (fruit.fruitCategory == FruitCategory.Buff)
        {
            return isPlayer;
        }

        if (fruit.fruitCategory == FruitCategory.Debuff)
        {
            return !isPlayer;
        }

        return false;
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

    public void TakeDirectDamage(int damage)
    {
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

    public int GetAttackDamage()
    {
        return Mathf.RoundToInt(attackDamage * damageBoostMultiplier);
    }

    public void ApplyDamageBoost(float multiplier, int turns)
    {
        damageBoostMultiplier = Mathf.Max(1f, multiplier);
        damageBoostTurnsRemaining = Mathf.Max(1, turns);

        Debug.Log(unitName + " attack boost: x" + damageBoostMultiplier + " for " + damageBoostTurnsRemaining + " turns.");
    }

    public void AdvanceBuffTurn()
    {
        if (damageBoostTurnsRemaining <= 0)
        {
            return;
        }

        damageBoostTurnsRemaining--;

        if (damageBoostTurnsRemaining > 0)
        {
            return;
        }

        damageBoostMultiplier = 1f;
        damageBoostTurnsRemaining = 0;

        Debug.Log(unitName + " attack boost ended.");
    }

    public void ApplyPoison(int damagePerTurn, int turns)
    {
        poisonDamagePerTurn = Mathf.Max(poisonDamagePerTurn, damagePerTurn);
        poisonTurnsRemaining = Mathf.Max(poisonTurnsRemaining, turns);

        Debug.Log(unitName + " is poisoned.");
    }

    public bool HasPoison()
    {
        return poisonTurnsRemaining > 0 && poisonDamagePerTurn > 0;
    }

    public int GetPoisonDamage()
    {
        return poisonDamagePerTurn;
    }

    public void TickPoison()
    {
        if (!HasPoison())
        {
            return;
        }

        TakeDirectDamage(poisonDamagePerTurn);

        poisonTurnsRemaining--;

        if (poisonTurnsRemaining > 0)
        {
            return;
        }

        poisonDamagePerTurn = 0;
        poisonTurnsRemaining = 0;

        Debug.Log(unitName + " poison ended.");
    }

    public void ApplyClot(int turns)
    {
        clotTurnsRemaining = Mathf.Max(clotTurnsRemaining, turns);

        Debug.Log(unitName + " is frozen.");
    }

    public bool ConsumeClotTurn()
    {
        if (clotTurnsRemaining <= 0)
        {
            return false;
        }

        clotTurnsRemaining--;

        if (clotTurnsRemaining <= 0)
        {
            clotTurnsRemaining = 0;
            Debug.Log(unitName + " clot ended.");
        }

        return true;
    }

    public void ApplyNausea(float failChance, int turns)
    {
        nauseaFailChance = Mathf.Max(nauseaFailChance, failChance);
        nauseaTurnsRemaining = Mathf.Max(nauseaTurnsRemaining, turns);

        Debug.Log(unitName + " is nauseous.");
    }

    public bool TryFailFromNausea()
    {
        if (nauseaTurnsRemaining <= 0)
        {
            return false;
        }

        nauseaTurnsRemaining--;

        bool failed = Random.value < nauseaFailChance;

        if (nauseaTurnsRemaining <= 0)
        {
            nauseaTurnsRemaining = 0;
            nauseaFailChance = 0f;
            Debug.Log(unitName + " nausea ended.");
        }

        return failed;
    }

    public void GiveShield()
    {
        shieldActive = true;
    }

    public bool UseFruit(Fruit fruit)
    {
        if (!CanReceiveFruit(fruit))
        {
            return false;
        }

        foreach (FruitEffect effect in fruit.effects)
        {
            if (effect == FruitEffect.Heal)
            {
                Heal(fruit.healAmount);
            }

            if (effect == FruitEffect.DamageBoost)
            {
                ApplyDamageBoost(fruit.damageBoostMultiplier, fruit.damageBoostTurns);
            }

            if (effect == FruitEffect.ManaRestore)
            {
                RestoreMana(fruit.manaAmount);
            }

            if (effect == FruitEffect.Poison)
            {
                ApplyPoison(fruit.poisonDamagePerTurn, fruit.poisonTurns);
            }

            if (effect == FruitEffect.Clot)
            {
                ApplyClot(fruit.clotTurns);
            }

            if (effect == FruitEffect.Nausea)
            {
                ApplyNausea(fruit.nauseaFailChance, fruit.nauseaTurns);
            }
        }

        Debug.Log(unitName + " used fruit: " + fruit.itemName);
        return true;
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

    public void ShowTargetButton()
    {
        switch (targetButton)
        {
            case Button button:
                button.gameObject.SetActive(true);
                break;
        }
    }

    public void HideTargetButton()
    {
        switch (targetButton)
        {
            case Button button:
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
                break;
        }
    }

    public void BindTargetButton(UnityEngine.Events.UnityAction action)
    {
        switch (targetButton)
        {
            case Button button:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(action);
                button.gameObject.SetActive(true);
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