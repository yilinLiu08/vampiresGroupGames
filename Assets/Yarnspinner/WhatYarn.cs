using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class WhatYarn : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    public string fightoneWin = "Win";
    public string fightoneLose = "Lose";
    public string pregame = "Pregame";

    // public string fighttwoWin = "NPCFight2Win";
    //public string fighttwoLose = "Lose2";

    // public string fightthreeWin = "NPCFight3Win";
    // public string fightthreeLose = "NPCFight3Lose";

    public string fighttwoWin = "Win2";
    public string fighttwoLose = "Lose2";

    public string fightthreeWin = "Win3";
    public string fightthreeLose = "Lose3";

    void Start()
    {
        int lastFightNumber = PlayerPrefs.GetInt("LastFightNumber", 0);
        int lastFightWon = PlayerPrefs.GetInt("LastFightWon", -1);

        if (lastFightNumber == 1 && lastFightWon == 1)
        {
            dialogueRunner.StartDialogue(fightoneWin);
            // PlayerPrefs.SetInt("FightOneWin", 0); //this resets it back to zero lel)
        }
        else if (lastFightNumber == 1 && lastFightWon == 0)
        {
            dialogueRunner.StartDialogue(fightoneLose);
            // PlayerPrefs.SetInt("FightOneLoseCount", 0);
        }
        else if (lastFightNumber == 2 && lastFightWon == 1)
        {
            dialogueRunner.StartDialogue(fighttwoWin);
        }
        else if (lastFightNumber == 2 && lastFightWon == 0)
        {
            dialogueRunner.StartDialogue(fighttwoLose);
        }
        else if (lastFightNumber == 3 && lastFightWon == 1)
        {
            dialogueRunner.StartDialogue(fightthreeWin);
        }
        else if (lastFightNumber == 3 && lastFightWon == 0)
        {
            dialogueRunner.StartDialogue(fightthreeLose);
        }
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