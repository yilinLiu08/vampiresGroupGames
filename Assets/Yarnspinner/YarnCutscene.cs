using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class YarnCutscene : MonoBehaviour
{
    public GameObject elsieCutscene;
    public VideoPlayer videoPlayer;
    public GameObject yarnCanvas;

    [YarnCommand("PlayElsieCutscene")]
    public void PlayElsieCutscene()
    {
        elsieCutscene.SetActive(true);
        videoPlayer.Play();
        yarnCanvas.SetActive(false);

        //vid.loopPointReached += CheckOver;
        //if video has ended (EndElsieCutscene()
        /*if (videoPlayer.loopPointReached += OnLoopPointReached) ;

        {
            EndElsieCutscene();
        }
        */
    }

    public void EndElsieCutscene()
    {
        yarnCanvas.SetActive(true);
        
        elsieCutscene.SetActive(false);
    }

}

