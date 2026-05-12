using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoPlay : MonoBehaviour
{
    
    public float delayInSeconds = 49f;
    public string nextScene;

    void Start()
    {
        
        StartCoroutine(WaitAndChangeScene());
    }

    IEnumerator WaitAndChangeScene()
    {
        
        yield return new WaitForSeconds(delayInSeconds);

        
        SceneManager.LoadScene(nextScene);
    }
}