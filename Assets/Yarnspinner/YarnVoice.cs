using UnityEngine;
using Yarn.Unity;
public class YarnVoice : MonoBehaviour
{

    public AudioClip elsieSqueal;

    public AudioClip lovedayHmph;

    public AudioClip dahliaYell;

    public AudioClip veraLaugh;

    AudioSource audio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [YarnCommand("ElsieSqueal")]
    public void ElsieSqueal()
    {
        audio.clip = elsieSqueal;
        audio.Play();
    }


    [YarnCommand("LovedayHmph")]
    public void LovedayHmph()
    {
        audio.clip = lovedayHmph;
        audio.Play();
    }

    [YarnCommand("DahliaYell")]
    public void DahliaYell()
    {
        audio.clip = dahliaYell;
        audio.Play();
    }

    [YarnCommand("VeraLaugh")]
    public void VeraLaugh()
    {
        audio.clip = veraLaugh;
        audio.Play();
    }



}
