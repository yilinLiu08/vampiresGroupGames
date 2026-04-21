using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public TextMeshProUGUI coinAmt;

    void Start()
    {
        // get current balance
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);

        // check LOSS (Key: FightOneLoseCount)
        if (PlayerPrefs.GetInt("FightOneLoseCount", 0) >= 1)
        {
            currentCoins += 10;
            PlayerPrefs.SetInt("FightOneLoseCount", 0); // Reset it!
        }

        // check WIN  "FightOneWin"
        if (PlayerPrefs.GetInt("FightOneWin", 0) >= 1)
        {
            currentCoins += 30;
            PlayerPrefs.SetInt("FightOneWin", 0); // reset it!
        }

        // save the new total and update the screen
        PlayerPrefs.SetInt("TotalCoins", currentCoins);
        PlayerPrefs.Save();

        UpdateUI();

        //coinAmt.text = "Coins: " + currentCoins;
    }

    public void UpdateUI()
    {
        int currentCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coinAmt != null)
        {
            coinAmt.text = "Coins: " + currentCoins;
        }
    }
}