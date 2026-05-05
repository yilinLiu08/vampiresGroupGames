using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private string loseONE = "FightOneLoseCount";
    private string winONE = "FightOneWin";

    private string fight1Complete = "fight1Complete";
    private string fight1Lose = "fight1Lose";

    private string loseTWO = "FightTwoLoseCount";
    private string winTWO = "FightTwoWin";

    private string fight2Complete = "fight2Complete";
    private string fight2Lose = "fight2Lose";

    private string loseTHREE = "FightThreeLoseCount";
    private string winTHREE = "FightThreeWin";

    private string fight3Complete = "fight3Complete";
    private string fight3Lose = "fight3Lose";

    private string lastFightNumber = "LastFightNumber";
    private string lastFightWon = "LastFightWon";

    public void LoadNextScene(string sceneName)
    {
        if (this.gameObject.name == "FightOneLose")
        {
            SaveBattleResult(1, false);
        }

        if (this.gameObject.name == "FightOneWin")
        {
            SaveBattleResult(1, true);
        }

        if (this.gameObject.name == "FightTwoLose")
        {
            SaveBattleResult(2, false);
        }

        if (this.gameObject.name == "FightTwoWin")
        {
            SaveBattleResult(2, true);
        }

        if (this.gameObject.name == "FightThreeLose")
        {
            SaveBattleResult(3, false);
        }

        if (this.gameObject.name == "FightThreeWin")
        {
            SaveBattleResult(3, true);
        }

        if (this.gameObject.name == "StartScreen")
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            SceneManager.LoadScene("StartScreen");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void LoadBattleResultScene(int fightNumber, bool won, string sceneName)
    {
        SaveBattleResult(fightNumber, won);
        SceneManager.LoadScene(sceneName);
    }

    public void SaveBattleResult(int fightNumber, bool won)
    {
        PlayerPrefs.SetInt(lastFightNumber, fightNumber);
        PlayerPrefs.SetInt(lastFightWon, won ? 1 : 0);

        if (fightNumber == 1)
        {
            SaveFightOneResult(won);
        }

        if (fightNumber == 2)
        {
            SaveFightTwoResult(won);
        }

        if (fightNumber == 3)
        {
            SaveFightThreeResult(won);
        }

        PlayerPrefs.Save();
    }

    void SaveFightOneResult(bool won)
    {
        if (!won)
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

            return;
        }

        int currentCount = PlayerPrefs.GetInt(winONE, 0);
        currentCount++;
        PlayerPrefs.SetInt(winONE, currentCount);
        PlayerPrefs.Save();
        Debug.Log("win count: " + currentCount);

        int totalCompletionsWin = PlayerPrefs.GetInt(fight1Complete, 0);
        totalCompletionsWin++;
        PlayerPrefs.SetInt(fight1Complete, totalCompletionsWin);
        Debug.Log("fight 1 completed (for dialogue)" + totalCompletionsWin);
    }

    void SaveFightTwoResult(bool won)
    {
        if (!won)
        {
            int currentLosses = PlayerPrefs.GetInt(loseTWO, 0);
            currentLosses++;
            PlayerPrefs.SetInt(loseTWO, currentLosses);
            Debug.Log("fight 2 lost " + currentLosses);

            int totalCompletions = PlayerPrefs.GetInt(fight2Lose, 0);
            totalCompletions++;
            PlayerPrefs.SetInt(fight2Lose, totalCompletions);
            Debug.Log("fight 2 lost (for dialogue)" + totalCompletions);

            return;
        }

        int currentCount = PlayerPrefs.GetInt(winTWO, 0);
        currentCount++;
        PlayerPrefs.SetInt(winTWO, currentCount);
        Debug.Log("fight 2 win count: " + currentCount);

        int totalCompletionsWin = PlayerPrefs.GetInt(fight2Complete, 0);
        totalCompletionsWin++;
        PlayerPrefs.SetInt(fight2Complete, totalCompletionsWin);
        Debug.Log("fight 2 completed (for dialogue)" + totalCompletionsWin);
    }

    void SaveFightThreeResult(bool won)
    {
        if (!won)
        {
            int currentLosses = PlayerPrefs.GetInt(loseTHREE, 0);
            currentLosses++;
            PlayerPrefs.SetInt(loseTHREE, currentLosses);
            Debug.Log("fight 3 lost " + currentLosses);

            int totalCompletions = PlayerPrefs.GetInt(fight3Lose, 0);
            totalCompletions++;
            PlayerPrefs.SetInt(fight3Lose, totalCompletions);
            Debug.Log("fight 3 lost (for dialogue)" + totalCompletions);

            return;
        }

        int currentCount = PlayerPrefs.GetInt(winTHREE, 0);
        currentCount++;
        PlayerPrefs.SetInt(winTHREE, currentCount);
        Debug.Log("fight 3 win count: " + currentCount);

        int totalCompletionsWin = PlayerPrefs.GetInt(fight3Complete, 0);
        totalCompletionsWin++;
        PlayerPrefs.SetInt(fight3Complete, totalCompletionsWin);
        Debug.Log("fight 3 completed (for dialogue)" + totalCompletionsWin);
    }
}