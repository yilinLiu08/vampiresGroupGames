using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class WhatYarn : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public string fightoneWin = "Win";
    public string fightoneLose = "Lose";
    public string pregame = "Pregame";


    public string fighttwoWin = "Win2";
    public string fighttwoLose = "Lose2";

    public string fightthreeWin = "Win3";
    public string fightthreeLose = "Lose3";


    void Start()
    {
       
        int winCount = PlayerPrefs.GetInt("FightOneWin", 0);
        int loseCount = PlayerPrefs.GetInt("FightOneLoseCount", 0);

        /*
        int win2Count = PlayerPrefs.GetInt("FightOneWin", 0);
        int lose2Count = PlayerPrefs.GetInt("FightOneLoseCount", 0);

        int win3Count = PlayerPrefs.GetInt("FightOneWin", 0);
        int lose3Count = PlayerPrefs.GetInt("FightOneLoseCount", 0);
        */

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

        /*
           else if (win2Count >= 1)
        {
            
            dialogueRunner.StartDialogue(fightoneWin); 
           // PlayerPrefs.SetInt("FightOneWin", 0); //this resets it back to zero lel)
        }
       
        else if (lose2Count >= 1)
        {
            dialogueRunner.StartDialogue(fightoneLose);
           // PlayerPrefs.SetInt("FightOneLoseCount", 0);
        }

         else if (win3Count >= 1)
        {
            
            dialogueRunner.StartDialogue(fightoneWin); 
           // PlayerPrefs.SetInt("FightOneWin", 0); //this resets it back to zero lel)
        }
       
        else if (lose3Count >= 1)
        {
            dialogueRunner.StartDialogue(fightoneLose);
           // PlayerPrefs.SetInt("FightOneLoseCount", 0);
        }
          */

        else
        {
            dialogueRunner.StartDialogue(pregame);
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