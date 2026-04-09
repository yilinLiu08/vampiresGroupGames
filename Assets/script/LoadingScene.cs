using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Image LoadingBarfill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));    
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarfill.fillAmount = progressValue;
            yield return null;
        }
    }
}

