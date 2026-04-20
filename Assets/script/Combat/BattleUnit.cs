using System.Collections;
using System.Collections.Generic;
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

    [Header("Death Visual")]
    public SpriteRenderer[] tintSpriteRenderers;
    public Image[] tintImages;
    public Color deadTint = new Color(0.5f, 0.5f, 0.5f, 1f);

    [Header("Floating Text")]
    public TextMeshProUGUI damageHealText;
    public float damageHealTextFadeTime = 2f;

    [Header("Persistent Effect UI")]
    public Image persistentEffectIconImage;
    public BattleStatusTooltipUI persistentEffectTooltipUI;

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

    private Color[] originalSpriteColors;
    private Color[] originalImageColors;

    private Coroutine damageHealTextRoutine;

    private string damageBoostSourceName;
    private Sprite damageBoostSourceIcon;
    private int damageBoostDisplayOrder = -1;

    private string poisonSourceName;
    private Sprite poisonSourceIcon;
    private int poisonDisplayOrder = -1;

    private string clotSourceName;
    private Sprite clotSourceIcon;
    private int clotDisplayOrder = -1;

    private string nauseaSourceName;
    private Sprite nauseaSourceIcon;
    private int nauseaDisplayOrder = -1;

    private bool externalStatusActive = false;
    private string externalStatusName;
    private string externalStatusDescription;
    private Sprite externalStatusIcon;
    private int externalStatusDisplayOrder = -1;

    private int persistentEffectOrderCounter = 0;

    private void Awake()
    {
        CacheOriginalColors();
        SetupPersistentEffectIconHover();
    }

    private void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        UpdateHPUI();
        UpdateManaUI();
        SetHighlight(false);
        HideTargetButton();
        UpdateDeadVisual();
        HideDamageHealText();
        RefreshPersistentEffectIcon();
    }

    void CacheOriginalColors()
    {
        originalSpriteColors = new Color[tintSpriteRenderers.Length];
        originalImageColors = new Color[tintImages.Length];

        for (int i = 0; i < tintSpriteRenderers.Length; i++)
        {
            if (tintSpriteRenderers[i] == null)
            {
                continue;
            }

            originalSpriteColors[i] = tintSpriteRenderers[i].color;
        }

        for (int i = 0; i < tintImages.Length; i++)
        {
            if (tintImages[i] == null)
            {
                continue;
            }

            originalImageColors[i] = tintImages[i].color;
        }
    }

    void SetupPersistentEffectIconHover()
    {
        if (persistentEffectIconImage == null)
        {
            return;
        }

        BattleStatusIconHover hover = persistentEffectIconImage.GetComponent<BattleStatusIconHover>();

        if (hover == null)
        {
            hover = persistentEffectIconImage.gameObject.AddComponent<BattleStatusIconHover>();
        }

        hover.owner = this;
        hover.tooltipUI = persistentEffectTooltipUI;
    }

    void UpdateDeadVisual()
    {
        bool dead = IsDead();

        for (int i = 0; i < tintSpriteRenderers.Length; i++)
        {
            if (tintSpriteRenderers[i] == null)
            {
                continue;
            }

            if (dead)
            {
                tintSpriteRenderers[i].color = deadTint;
            }
            else
            {
                tintSpriteRenderers[i].color = originalSpriteColors[i];
            }
        }

        for (int i = 0; i < tintImages.Length; i++)
        {
            if (tintImages[i] == null)
            {
                continue;
            }

            if (dead)
            {
                tintImages[i].color = deadTint;
            }
            else
            {
                tintImages[i].color = originalImageColors[i];
            }
        }
    }

    void HideDamageHealText()
    {
        if (damageHealText == null)
        {
            return;
        }

        damageHealText.gameObject.SetActive(false);
    }

    void ShowDamageHealText(string textValue)
    {
        if (damageHealText == null)
        {
            return;
        }

        if (damageHealTextRoutine != null)
        {
            StopCoroutine(damageHealTextRoutine);
        }

        damageHealTextRoutine = StartCoroutine(DamageHealTextRoutine(textValue));
    }

    IEnumerator DamageHealTextRoutine(string textValue)
    {
        damageHealText.gameObject.SetActive(true);
        damageHealText.text = textValue;

        Color color = damageHealText.color;
        color.a = 1f;
        damageHealText.color = color;

        float timer = 0f;

        while (timer < damageHealTextFadeTime)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / damageHealTextFadeTime);
            damageHealText.color = color;

            yield return null;
        }

        damageHealText.gameObject.SetActive(false);
        damageHealTextRoutine = null;
    }

    bool HasEffect(Fruit fruit, FruitEffect effect)
    {
        if (fruit == null)
        {
            return false;
        }

        if (fruit.effects == null)
        {
            return false;
        }

        return fruit.effects.Contains(effect);
    }

    public bool CanReceiveFruit(Fruit fruit)
    {
        if (fruit == null)
        {
            return false;
        }

        bool hasRevive = HasEffect(fruit, FruitEffect.Revive);

        if (hasRevive)
        {
            return isPlayer && IsDead() && fruit.fruitCategory == FruitCategory.Buff;
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

        int beforeHP = currentHP;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        int actualDamage = beforeHP - currentHP;

        if (actualDamage > 0)
        {
            ShowDamageHealText("-" + actualDamage);
        }

        UpdateHPUI();
        UpdateDeadVisual();
    }

    public void TakeDirectDamage(int damage)
    {
        int beforeHP = currentHP;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        int actualDamage = beforeHP - currentHP;

        if (actualDamage > 0)
        {
            ShowDamageHealText("-" + actualDamage);
        }

        UpdateHPUI();
        UpdateDeadVisual();
    }

    public void Heal(int amount)
    {
        int beforeHP = currentHP;

        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        int actualHeal = currentHP - beforeHP;

        if (actualHeal > 0)
        {
            ShowDamageHealText("+" + actualHeal);
        }

        UpdateHPUI();
        UpdateDeadVisual();
    }

    public void Revive(int amount)
    {
        int beforeHP = currentHP;

        currentHP = Mathf.Clamp(amount, 1, maxHP);

        int actualHeal = currentHP - beforeHP;

        if (actualHeal > 0)
        {
            ShowDamageHealText("+" + actualHeal);
        }

        UpdateHPUI();
        UpdateDeadVisual();
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

    void RegisterPersistentDisplay(Fruit fruit, FruitEffect effect)
    {
        persistentEffectOrderCounter++;

        if (effect == FruitEffect.DamageBoost)
        {
            damageBoostSourceName = fruit.itemName;
            damageBoostSourceIcon = fruit.icon;
            damageBoostDisplayOrder = persistentEffectOrderCounter;
        }

        if (effect == FruitEffect.Poison)
        {
            poisonSourceName = fruit.itemName;
            poisonSourceIcon = fruit.icon;
            poisonDisplayOrder = persistentEffectOrderCounter;
        }

        if (effect == FruitEffect.Clot)
        {
            clotSourceName = fruit.itemName;
            clotSourceIcon = fruit.icon;
            clotDisplayOrder = persistentEffectOrderCounter;
        }

        if (effect == FruitEffect.Nausea)
        {
            nauseaSourceName = fruit.itemName;
            nauseaSourceIcon = fruit.icon;
            nauseaDisplayOrder = persistentEffectOrderCounter;
        }

        RefreshPersistentEffectIcon();
    }

    void ClearPersistentDisplay(FruitEffect effect)
    {
        if (effect == FruitEffect.DamageBoost)
        {
            damageBoostSourceName = "";
            damageBoostSourceIcon = null;
            damageBoostDisplayOrder = -1;
        }

        if (effect == FruitEffect.Poison)
        {
            poisonSourceName = "";
            poisonSourceIcon = null;
            poisonDisplayOrder = -1;
        }

        if (effect == FruitEffect.Clot)
        {
            clotSourceName = "";
            clotSourceIcon = null;
            clotDisplayOrder = -1;
        }

        if (effect == FruitEffect.Nausea)
        {
            nauseaSourceName = "";
            nauseaSourceIcon = null;
            nauseaDisplayOrder = -1;
        }

        RefreshPersistentEffectIcon();
    }

    public void SetExternalPersistentEffect(string sourceName, string description, Sprite icon)
    {
        persistentEffectOrderCounter++;

        externalStatusActive = true;
        externalStatusName = sourceName;
        externalStatusDescription = description;
        externalStatusIcon = icon;
        externalStatusDisplayOrder = persistentEffectOrderCounter;

        RefreshPersistentEffectIcon();
    }

    public void ClearExternalPersistentEffect()
    {
        externalStatusActive = false;
        externalStatusName = "";
        externalStatusDescription = "";
        externalStatusIcon = null;
        externalStatusDisplayOrder = -1;

        RefreshPersistentEffectIcon();
    }

    public bool HasPersistentEffectDisplay()
    {
        if (damageBoostTurnsRemaining > 0)
        {
            return true;
        }

        if (poisonTurnsRemaining > 0)
        {
            return true;
        }

        if (clotTurnsRemaining > 0)
        {
            return true;
        }

        if (nauseaTurnsRemaining > 0)
        {
            return true;
        }

        if (externalStatusActive)
        {
            return true;
        }

        return false;
    }

    public string GetPersistentEffectTooltipText()
    {
        List<string> lines = new List<string>();

        if (externalStatusActive)
        {
            lines.Add(externalStatusName + ": " + externalStatusDescription);
        }

        if (damageBoostTurnsRemaining > 0)
        {
            lines.Add(damageBoostSourceName + ": Attack x" + damageBoostMultiplier.ToString("0.0") + ", " + damageBoostTurnsRemaining + " turns left.");
        }

        if (poisonTurnsRemaining > 0)
        {
            lines.Add(poisonSourceName + ": " + poisonDamagePerTurn + " poison damage each turn, " + poisonTurnsRemaining + " turns left.");
        }

        if (clotTurnsRemaining > 0)
        {
            lines.Add(clotSourceName + ": Frozen, " + clotTurnsRemaining + " turns left.");
        }

        if (nauseaTurnsRemaining > 0)
        {
            lines.Add(nauseaSourceName + ": " + Mathf.RoundToInt(nauseaFailChance * 100f) + "% miss chance, " + nauseaTurnsRemaining + " turns left.");
        }

        return string.Join("\n", lines);
    }

    void RefreshPersistentEffectIcon()
    {
        if (persistentEffectIconImage == null)
        {
            return;
        }

        if (!HasPersistentEffectDisplay())
        {
            persistentEffectIconImage.gameObject.SetActive(false);
            persistentEffectIconImage.sprite = null;
            return;
        }

        Sprite newestSprite = null;
        int newestOrder = -1;

        if (externalStatusActive && externalStatusDisplayOrder > newestOrder)
        {
            newestOrder = externalStatusDisplayOrder;
            newestSprite = externalStatusIcon;
        }

        if (damageBoostTurnsRemaining > 0 && damageBoostDisplayOrder > newestOrder)
        {
            newestOrder = damageBoostDisplayOrder;
            newestSprite = damageBoostSourceIcon;
        }

        if (poisonTurnsRemaining > 0 && poisonDisplayOrder > newestOrder)
        {
            newestOrder = poisonDisplayOrder;
            newestSprite = poisonSourceIcon;
        }

        if (clotTurnsRemaining > 0 && clotDisplayOrder > newestOrder)
        {
            newestOrder = clotDisplayOrder;
            newestSprite = clotSourceIcon;
        }

        if (nauseaTurnsRemaining > 0 && nauseaDisplayOrder > newestOrder)
        {
            newestOrder = nauseaDisplayOrder;
            newestSprite = nauseaSourceIcon;
        }

        persistentEffectIconImage.gameObject.SetActive(true);
        persistentEffectIconImage.sprite = newestSprite;
        persistentEffectIconImage.preserveAspect = true;
    }

    public void ApplyDamageBoost(float multiplier, int turns, Fruit sourceFruit)
    {
        damageBoostMultiplier = Mathf.Max(1f, multiplier);
        damageBoostTurnsRemaining = Mathf.Max(1, turns);

        RegisterPersistentDisplay(sourceFruit, FruitEffect.DamageBoost);

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
            RefreshPersistentEffectIcon();
            return;
        }

        damageBoostMultiplier = 1f;
        damageBoostTurnsRemaining = 0;

        ClearPersistentDisplay(FruitEffect.DamageBoost);

        Debug.Log(unitName + " attack boost ended.");
    }

    public void ApplyPoison(int damagePerTurn, int turns, Fruit sourceFruit)
    {
        poisonDamagePerTurn = Mathf.Max(poisonDamagePerTurn, damagePerTurn);
        poisonTurnsRemaining = Mathf.Max(poisonTurnsRemaining, turns);

        RegisterPersistentDisplay(sourceFruit, FruitEffect.Poison);

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
            RefreshPersistentEffectIcon();
            return;
        }

        poisonDamagePerTurn = 0;
        poisonTurnsRemaining = 0;

        ClearPersistentDisplay(FruitEffect.Poison);

        Debug.Log(unitName + " poison ended.");
    }

    public void ApplyClot(int turns, Fruit sourceFruit)
    {
        clotTurnsRemaining = Mathf.Max(clotTurnsRemaining, turns);

        RegisterPersistentDisplay(sourceFruit, FruitEffect.Clot);
        RefreshPersistentEffectIcon();

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
            ClearPersistentDisplay(FruitEffect.Clot);
            Debug.Log(unitName + " clot ended.");
        }
        else
        {
            RefreshPersistentEffectIcon();
        }

        return true;
    }

    public void ApplyNausea(float failChance, int turns, Fruit sourceFruit)
    {
        nauseaFailChance = Mathf.Max(nauseaFailChance, failChance);
        nauseaTurnsRemaining = Mathf.Max(nauseaTurnsRemaining, turns);

        RegisterPersistentDisplay(sourceFruit, FruitEffect.Nausea);

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
            ClearPersistentDisplay(FruitEffect.Nausea);
            Debug.Log(unitName + " nausea ended.");
        }
        else
        {
            RefreshPersistentEffectIcon();
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
                ApplyDamageBoost(fruit.damageBoostMultiplier, fruit.damageBoostTurns, fruit);
            }

            if (effect == FruitEffect.ManaRestore)
            {
                RestoreMana(fruit.manaAmount);
            }

            if (effect == FruitEffect.Poison)
            {
                ApplyPoison(fruit.poisonDamagePerTurn, fruit.poisonTurns, fruit);
            }

            if (effect == FruitEffect.Clot)
            {
                ApplyClot(fruit.clotTurns, fruit);
            }

            if (effect == FruitEffect.Nausea)
            {
                ApplyNausea(fruit.nauseaFailChance, fruit.nauseaTurns, fruit);
            }

            if (effect == FruitEffect.Revive)
            {
                Revive(fruit.reviveHP);

                if (TurnBattleManager.Instance != null)
                {
                    TurnBattleManager.Instance.ShowFruitMessage(unitName + " was revived with " + fruit.reviveHP + " HP.");
                }
            }

            if (effect == FruitEffect.CoinFlip)
            {
                if (TurnBattleManager.Instance != null)
                {
                    TurnBattleManager.Instance.ResolveCoinFlipFruit(fruit);
                }
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