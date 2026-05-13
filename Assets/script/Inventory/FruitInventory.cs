using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitInventory : MonoBehaviour
{
    public List<Fruit> fruits = new List<Fruit>();

    [Header("Inventory Limit")]
    public int maxFruitCount = 9;

    public Action onInventoryChanged;

    public bool IsFull()
    {
        return fruits.Count >= maxFruitCount;
    }

    public void AddFruit(Fruit newFruit)
    {


        if (IsFull())
        {
            Debug.Log("Inventory is full. count: " + fruits.Count);
            return;
        }

        fruits.Add(newFruit);
        Debug.Log("added: " + newFruit.itemName + " count: " + fruits.Count);

        onInventoryChanged?.Invoke();
    }

    public void RemoveFruit(Fruit removeFruit)
    {

        fruits.Remove(removeFruit);
        Debug.Log("removed: " + removeFruit.itemName + " count: " + fruits.Count);

        onInventoryChanged?.Invoke();
    }

    public bool HasFruit(Fruit checkFruit)
    {
        return fruits.Contains(checkFruit);
    }


}