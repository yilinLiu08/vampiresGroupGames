using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private string loseONE = "FightOneLoseCount";
    private string winONE = "FightOneWin";

   
    public void LoadNextScene(string sceneName)
    {
       
        if (this.gameObject.name == "FightOneLose")
        {
            int currentCount = PlayerPrefs.GetInt(loseONE, 0);
            currentCount++;
            PlayerPrefs.SetInt(loseONE, currentCount);
            PlayerPrefs.Save();
            Debug.Log("loss count: " + currentCount);
        }

        if (this.gameObject.name == "FightOneWin")
        {
            int currentCount = PlayerPrefs.GetInt(winONE, 0);
            currentCount++;
            PlayerPrefs.SetInt(winONE, currentCount);
            PlayerPrefs.Save();
            Debug.Log("win count: " + currentCount);
        }
        if (this.gameObject.name == "StartScreen")
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("StartScreen");
        }
        

            SceneManager.LoadScene(sceneName);
    }
}