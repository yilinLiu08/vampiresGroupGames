using TMPro;
using UnityEngine;

public class Buy : MonoBehaviour
{
    [Header("Inventory Settings")]
    public Fruit itemToGive;

   // public TextMeshProUGUI coinAmt;

    //public fruit coin amount

    public void AddItem()
    {

        FruitInventory inv = FindObjectOfType<FruitInventory>();

        if (inv != null && itemToGive != null)
        {
            inv.AddFruit(itemToGive);
            SubtractCoins();

        }
        else
        {
            Debug.LogError("where da inventory");
        }
    }

    void SubtractCoins()
    {
        
        /* int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
         currentCoins -= 30;
         Debug.Log("minused coins");
        */
        // int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // 2. Check LOSS (Key: FightOneLoseCount)

        // currentCoins += 10;
        // PlayerPrefs.SetInt("FightOneLoseCount", 0); // Reset it!


        //take specific coin amount and 
    }
}

