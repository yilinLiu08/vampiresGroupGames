using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;
using System.Collections;


public class YarnCutscene : MonoBehaviour
{
    public GameObject elsieCutscene;
    public GameObject dahliaCutscene;
    public GameObject veraCutscene;

    public GameObject campFire;
    public GameObject endScene;

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


        yield return new WaitForSeconds(39f);


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

    [YarnCommand("ShowCampfire")]
    public void ShowCampfire()
    {
        campFire.SetActive(true);


    }

    [YarnCommand("HideCampfire")]
    public void HideCampfire()
    {
        campFire.SetActive(false);


    }

    [YarnCommand("ShowEndScene")]
    public void ShowEndScene()
    {
        endScene.SetActive(true);


    }

    [YarnCommand("HideEndScene")]
    public void HideEndScene()
    {
        endScene.SetActive(false);


    }

}
