using System.Collections.Generic;
using UnityEngine;

public enum FruitEffect
{
    Heal,
    DamageBoost,
    ManaRestore,
    Poison,
    Clot,
    Nausea,
    Revive,
    CoinFlip
}

public enum FruitCategory
{
    Buff,
    Debuff
}

[CreateAssetMenu(fileName = "fruit", menuName = "Scriptable Objects/fruit")]
public class Fruit : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;

    public int price; // doing coin stuff here

    [Header("Category")]
    public FruitCategory fruitCategory = FruitCategory.Buff;

    [Header("Values")]
    public int healAmount;
    public float damageBoostMultiplier = 1.5f;
    public int damageBoostTurns = 1;
    public int manaAmount;

    [Header("Poison")]
    public int poisonDamagePerTurn = 5;
    public int poisonTurns = 3;

    [Header("Clot")]
    public int clotTurns = 1;

    [Header("Nausea")]
    [Range(0f, 1f)]
    public float nauseaFailChance = 0.5f;
    public int nauseaTurns = 1;

    [Header("Revive")]
    public int reviveHP = 50;

    [Header("Coin Flip")]
    public float coinFlipSkillMultiplier = 1.2f;
    public int coinFlipTeamDamage = 20;

    [Header("Effects")]
    public List<FruitEffect> effects = new List<FruitEffect>();
}