using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    public static Music instance;
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;

        audioSource.Play();
    }
}
