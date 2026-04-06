using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class FruitTracker : MonoBehaviour
{
    public static FruitTracker Instance;
 
    public TextMeshProUGUI touchedFruit;

    //insert fruit name AND number
    //i finally searched up how to use a dictionary 
    public Dictionary<string, int> inventory = new Dictionary<string, int>();

    public string currentFruitPickedUp;

    void Awake()
    {
        if (Instance == null) Instance = this;
        //inventoryManagerScript = GameObject.Find("Inventory_Holder").GetComponent<InventoryManager>();
    }

    public void AddFruit(string fruitName)
    {
        //set fruit int to zero if new
        /*(!inventory.ContainsKey(fruitName))
        {
            inventory[fruitName] = PlayerPrefs.GetInt(fruitName, 0);
        }
        */
        //set fruit int to zero if new
        if (!inventory.ContainsKey(fruitName))
        {
            inventory[fruitName] = PlayerPrefs.GetInt(fruitName, 0);
        }

        //add  fruit
        inventory[fruitName]++;

        //reference the actual fruits name
        PlayerPrefs.SetInt(fruitName, inventory[fruitName]);
        PlayerPrefs.Save();

        // tracker in the console instead of the tmp bc it fucked me in the ass
        Debug.Log($"ayo you GAINED 1 to {fruitName}. yo total {fruitName}s: {inventory[fruitName]}");



        //add  fruit
       // currentFruitPickedUp = fruitName;
        //inventory[fruitName]++;

        //reference the actual fruits name
       // PlayerPrefs.SetInt(fruitName, inventory[fruitName]);
       // PlayerPrefs.Save();

       // LogInventoryContents(fruitName);

       // Debug.Log("Trying to update inventory");
    }

   /* void LogInventoryContents(string collectedFruitName)
    {
        foreach (KeyValuePair<string, int> pair in inventory)
        {
            Debug.Log("Collected item: " + pair.Key + " | Amount: " + pair.Value, this);
        }
    }
   */
}