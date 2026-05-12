using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;
using System.Collections;

public class YarnCutscene : MonoBehaviour
{
    public GameObject elsieCutscene;
    public GameObject dahliaCutscene;
    public GameObject veraCutscene;

    public VideoPlayer videoPlayer;
    public GameObject yarnCanvas;



    [YarnCommand("PlayElsieCutscene")]
    public IEnumerator PlayElsieCutscene()
    {
        
        elsieCutscene.SetActive(true);
        yarnCanvas.SetActive(false);

        videoPlayer.Play();

        
        yield return new WaitForSeconds(42f);

       
        EndElsieCutscene();
    }

    public void EndElsieCutscene()
    {
        videoPlayer.Stop();
        yarnCanvas.SetActive(true);
        elsieCutscene.SetActive(false);
    }

    [YarnCommand("PlayDahliaCutscene")]
    public IEnumerator PlayDahliaCutscene()
    {

        dahliaCutscene.SetActive(true);
        yarnCanvas.SetActive(false);

        videoPlayer.Play();


        yield return new WaitForSeconds(1f);


        EndDahliaCutscene();
    }

    public void EndDahliaCutscene()
    {
        videoPlayer.Stop();
        yarnCanvas.SetActive(true);
        dahliaCutscene.SetActive(false);
    }


    [YarnCommand("PlayVeraCutscene")]
    public IEnumerator PlayVeraCutscene()
    {

        veraCutscene.SetActive(true);
        yarnCanvas.SetActive(false);

        videoPlayer.Play();


        yield return new WaitForSeconds(43f);


        EndVeraCutscene();
    }

    public void EndVeraCutscene()
    {
        videoPlayer.Stop();
        yarnCanvas.SetActive(true);
        veraCutscene.SetActive(false);
    }

}
