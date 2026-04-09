using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class WhatYarn : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public string fightoneWin = "Win";
    public string fightoneLose = "Lose";

    void Start()
    {
       
        int winCount = PlayerPrefs.GetInt("FightOneWin", 0);
        int loseCount = PlayerPrefs.GetInt("FightOneLoseCount", 0);

        
        if (winCount >= 1)
        {
            
            dialogueRunner.StartDialogue(fightoneWin); 
           // PlayerPrefs.SetInt("FightOneWin", 0); //this resets it back to zero lel)
        }
       
        else if (loseCount >= 1)
        {
            dialogueRunner.StartDialogue(fightoneLose);
           // PlayerPrefs.SetInt("FightOneLoseCount", 0);
        }

        PlayerPrefs.Save();
    }

    [YarnCommand("HQ")]
    public void HQ()
    {
        Debug.Log("go home after battle");
        SceneManager.LoadScene("HQ_realone");
    }
}