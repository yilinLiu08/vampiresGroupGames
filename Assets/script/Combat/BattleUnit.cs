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

    [Header("UI")]
    public Image hpBarImage;
    public Image highlightImage;

    private void Start()
    {
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
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