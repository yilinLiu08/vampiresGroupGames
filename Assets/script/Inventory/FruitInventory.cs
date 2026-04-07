using System.Collections.Generic;
using UnityEngine;

public class FruitInventory : MonoBehaviour
{
    public List<Fruit> fruits = new List<Fruit>();

    public void AddFruit(Fruit newFruit)
    {
        fruits.Add(newFruit);
        Debug.Log("Added: " + newFruit.itemName);
    }

    public void RemoveFruit(Fruit removeFruit)
    {
        fruits.Remove(removeFruit);
        Debug.Log("Removed: " + removeFruit.itemName);
    }

    public bool HasFruit(Fruit checkFruit)
    {
        return fruits.Contains(checkFruit);
    }
}