/*using UnityEngine;
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
*/
using UnityEngine;
using Yarn.Unity;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class YarnVoice : MonoBehaviour
{
    
    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Audio Settings")]
    public List<SoundEffect> soundEffects;

    private Dictionary<string, AudioClip> _soundLibrary;
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        //set the dictionary first thing on game load
        _soundLibrary = new Dictionary<string, AudioClip>();

        foreach (var effect in soundEffects)
        {
            if (!string.IsNullOrEmpty(effect.name) && effect.clip != null)
            {
                // this is a dictionary we are adding to
                _soundLibrary[effect.name] = effect.clip;
            }
        }
    }

    // new way of doing yarn commands i jus found out, call it in yarn using <<PlaySound audio ElsieSqueal (name of thing u assigned in inspector) I HOPE THIS WORKS>>
    [YarnCommand("PlaySound")]
    public void PlaySound(string soundName)
    {
        if (_soundLibrary.TryGetValue(soundName, out AudioClip clip))
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' where is it");
        }
    }
}