using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneLoader : MonoBehaviour
{
  public VideoPlayer beginCutscene;

    void Awake()
    {
        beginCutscene = GetComponent<VideoPlayer>();
        beginCutscene.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer source)
    {
        
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);

        
    }
}