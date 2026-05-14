using UnityEngine;
using Yarn.Unity;


public class IfWon : MonoBehaviour
{
    public GameObject skipButton1;
    public GameObject skipButton2;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("ShowSkipButton1")]
    public void ShowSkipButton1()
    {
        skipButton1.SetActive(true);


    }

    [YarnCommand("ShowSkipButton2")]
    public void ShowSkipButton2()
    {
        skipButton2.SetActive(true);


    }


}
