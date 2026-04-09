/*using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoNextScene : MonoBehaviour
{
    void Awake()
    {
        GetComponent<VideoPlayer>().loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer source)
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }
}
*/