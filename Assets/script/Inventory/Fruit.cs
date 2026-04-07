using System.Collections.Generic;
using UnityEngine;

public enum FruitEffect
{
    Heal,
    DamageBoost,
    ManaRestore
}

[CreateAssetMenu(fileName = "fruit", menuName = "Scriptable Objects/fruit")]
public class Fruit : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;

    [Header("Values")]
    public int healAmount;
    public int damageBoostAmount;
    public int manaAmount;

    [Header("Effects")]
    public List<FruitEffect> effects = new List<FruitEffect>();
}