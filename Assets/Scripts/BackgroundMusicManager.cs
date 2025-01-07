using UnityEngine;
using YG;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private void Awake()
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (YandexGame.savesData.isMusicEnabled)
        {
            audioSource.Play();
        }
    }

    public void ToggleBackgroundMusic()
    {
        if (YandexGame.savesData.isMusicEnabled)
        {
            YandexGame.savesData.isMusicEnabled = false;
            audioSource.Pause();
        }
        else
        {
            YandexGame.savesData.isMusicEnabled = true;
            audioSource.Play();
        }
        YandexGame.SaveProgress();
    }

}
