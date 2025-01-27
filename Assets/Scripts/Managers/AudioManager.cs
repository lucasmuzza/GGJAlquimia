using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioSetup
{
    public AudioClip audioClip;
    public string audioTypeName;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public List<AudioSetup> audioSetups;

    private AudioSource _audioSource;

    private AudioClip _currentAudioClip;

    private bool isClipPaused;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(instance);

            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }

    }


    private void Update()
    {
        if(GameManager.instance != null && GameManager.instance.isGamePaused && !isClipPaused)
        {
            isClipPaused = true;
            PauseAudioClip();
        }

        if(GameManager.instance != null && !GameManager.instance.isGamePaused && isClipPaused)
        {
            isClipPaused = false;
            UnPauseAudioClip();
        }
    }


    /// <summary>
    /// Play the sound based on the audioType inside the list of sounds
    /// </summary>
    /// <param name="audioType"></param>
    public void PlaySound(string audioType)
    {
        foreach(var audioSetup in audioSetups)
        {
            if(audioType == audioSetup.audioTypeName)
            {
                _audioSource.PlayOneShot(audioSetup.audioClip);
            }
        }
    }

    /// <summary>
    /// Gets the sound from the list of sounds based on the audioType
    /// </summary>
    /// <param name="audioType"></param>
    /// <returns></returns>
    public AudioClip GetSound(string audioType)
    {
        foreach(var audioSetup in audioSetups)
        {
            if(audioType == audioSetup.audioTypeName)
            {
                _currentAudioClip = audioSetup.audioClip;
                return _currentAudioClip;
            }
        }

        return null;
    }

    public void PauseAudioClip()
    {
        _audioSource.Pause();
    }

    public void UnPauseAudioClip()
    {
        _audioSource.UnPause();
    }
}
