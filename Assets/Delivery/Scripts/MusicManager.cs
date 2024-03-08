using UnityEngine;
using YG;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    // Add your audio clips (songs) here
    public AudioClip[] playlist;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("music", 1) == 0)
            return;

        // Play the first track when the scene starts
        PlayFirstTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && !YandexGame.nowAdsShow)
        {
            audioSource.clip = playlist[Random.Range(0, playlist.Length - 1)];
            audioSource.Play();
        }
    }

    private void PlayFirstTrack()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    public void PauseMusic()
    {
        audioSource.volume = 0f;
        PlayerPrefs.SetInt("music", 0);
    }
    public void PlayMusic()
    {
        audioSource.volume = .2f; 
        PlayerPrefs.SetInt("music", 1);
    }
}
