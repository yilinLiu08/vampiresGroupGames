/*using UnityEngine;
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private string loseONE = "FightOneLoseCount";
    private string winONE = "FightOneWin";

    private string fight1Complete = "fight1Complete";
    private string fight1Lose = "fight1Lose";

    public void LoadNextScene(string sceneName)
    {
       
        if (this.gameObject.name == "FightOneLose")
        {
            int currentLosses = PlayerPrefs.GetInt(loseONE, 0);
            currentLosses++;
            PlayerPrefs.SetInt(loseONE, currentLosses);
            Debug.Log("Lost fight one and collect coins " + currentLosses);

            int totalCompletions = PlayerPrefs.GetInt(fight1Lose, 0);
            totalCompletions++;
            PlayerPrefs.SetInt(fight1Lose, totalCompletions);
            Debug.Log("fight 1 lost (for dialogue)" + totalCompletions);

            
            PlayerPrefs.Save();

        }

        if (this.gameObject.name == "FightOneWin")
        {
            int currentCount = PlayerPrefs.GetInt(winONE, 0);
            currentCount++;
            PlayerPrefs.SetInt(winONE, currentCount);
            PlayerPrefs.Save();
            Debug.Log("win count: " + currentCount);

            int totalCompletions = PlayerPrefs.GetInt(fight1Complete, 0);
            totalCompletions++;
            PlayerPrefs.SetInt(fight1Complete, totalCompletions);
            Debug.Log("fight 1 completed (for dialogue)" + totalCompletions);
        }
        if (this.gameObject.name == "StartScreen")
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            SceneManager.LoadScene("StartScreen");
        }
        

            SceneManager.LoadScene(sceneName);
    }
}
*/

