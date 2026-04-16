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
            SceneManager.LoadScene("StartScreen");
        }
        

            SceneManager.LoadScene(sceneName);
    }
}