using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public TextMeshProUGUI coinAmt;

    void Start()
    {
        // 1. Get current balance
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // 2. Check LOSS (Key: FightOneLoseCount)
        if (PlayerPrefs.GetInt("FightOneLoseCount", 0) >= 1)
        {
            currentCoins += 10;
            PlayerPrefs.SetInt("FightOneLoseCount", 0); // Reset it!
        }

        // 3. Check WIN (Matching your WhatYarn script key: "FightOneWin")
        if (PlayerPrefs.GetInt("FightOneWin", 0) >= 1)
        {
            currentCoins += 30;
            PlayerPrefs.SetInt("FightOneWin", 0); // Reset it!
        }

        // 4. Save the new total and update the screen
        PlayerPrefs.SetInt("TotalCoins", currentCoins);
        PlayerPrefs.Save();

        coinAmt.text = "Coins: " + currentCoins;
    }
}