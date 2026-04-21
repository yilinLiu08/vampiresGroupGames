using TMPro;
using UnityEngine;
using System.Collections;

public class Buy : MonoBehaviour
{
    [Header("Inventory Settings")]
    public Fruit itemToGive;


    [Header("UI Reference")]
    public Coins coinSystem;

    public GameObject youPoor;

    // public TextMeshProUGUI coinAmt;

    //public fruit coin amount

    public void AddItem()
    {

        /*FruitInventory inv = FindObjectOfType<FruitInventory>();

        if (inv != null && itemToGive != null)
        {
            inv.AddFruit(itemToGive);
            SubtractCoins();

        }
        else
        {
            Debug.LogError("where da inventory");
        }
        */

        FruitInventory inv = FindObjectOfType<FruitInventory>();

        if (inv != null && itemToGive != null)
        {
            int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

            // Check if player can afford it
            if (currentCoins >= itemToGive.price)
            {
                inv.AddFruit(itemToGive);
                SubtractCoins(itemToGive.price);
            }
            else
            {
                Debug.Log("u poor ass");
                
                StopAllCoroutines();
                StartCoroutine(Poor());

            }
        }
    }

    void SubtractCoins(int amount)
    {

        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        currentCoins -= amount;

        
        PlayerPrefs.SetInt("TotalCoins", currentCoins);
        PlayerPrefs.Save();

        
        if (coinSystem != null)
        {
            coinSystem.UpdateUI();
        }
    }

    IEnumerator Poor()
    {
        youPoor.SetActive(true);
        Debug.Log("turn on");
        yield return new WaitForSeconds(1f);
        youPoor.SetActive(false);
    }
}

