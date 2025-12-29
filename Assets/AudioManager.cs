using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip introChime;
    public AudioClip diceRollSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    public void PlayChime()
    {
        sfxSource.PlayOneShot(introChime);
    }

    public void PlayDice()
    {
        sfxSource.PlayOneShot(diceRollSound);
    }
}
