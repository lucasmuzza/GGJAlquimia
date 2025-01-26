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

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
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
                return audioSetup.audioClip;
            }
        }

        return null;
    }
}
