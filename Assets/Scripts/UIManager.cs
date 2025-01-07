using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private Transform levelFinishedMenu, levelFailedMenu, pauseMenu, adMenu, background;
    [SerializeField] private TextMeshProUGUI countDownText, levelText;
    [SerializeField] private float transitionTime = 1f;
    [SerializeField] private GameObject helpMenu, handBrakeImage, lowGearImage;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite;
    [SerializeField] private Image soundButton;
    [SerializeField] private Sprite musicOnSprite, musicOffSprite;
    [SerializeField] private Image musicButton;
    [SerializeField] private GameObject MobileController, helpButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        levelText.text = GameManager.Instance.GetCurrentLevelNumber().ToString();
        if (YandexGame.EnvironmentData.isTablet || YandexGame.EnvironmentData.isMobile)
        {
            MobileController.SetActive(true);
        }
        if (YandexGame.EnvironmentData.isTablet || YandexGame.EnvironmentData.isMobile)
        {
            helpButton.SetActive(false);
        }
        else
        {
            if (!YandexGame.savesData.isFirstHelpShowen)
            {
                ShowHelp();
                YandexGame.savesData.isFirstHelpShowen = true;
                YandexGame.SaveProgress();
            }
        }
    }

    public void OnHandBrakeToggle(bool isOn)
    {
        if (YandexGame.EnvironmentData.isDesktop || YandexGame.EnvironmentData.isTV || !isOn)
            handBrakeImage.SetActive(isOn);
    }

    public void OnLowGearToggle(bool isLow)
    {
        if (YandexGame.EnvironmentData.isDesktop || YandexGame.EnvironmentData.isTV || !isLow)
            lowGearImage.SetActive(isLow);
    }

    public void OnToggleSoundFX()
    {
        YandexGame.savesData.isSoundEnabled = !YandexGame.savesData.isSoundEnabled;
        YandexGame.SaveProgress();
        UpdateSoundButtonSprite();
    }

    private void UpdateSoundButtonSprite()
    {
        if (YandexGame.savesData.isSoundEnabled)
        {
            soundButton.sprite = soundOnSprite;
        }
        else
        {
            soundButton.sprite = soundOffSprite;
        }
    }

    public void OnToggleMusic()
    {
        BackgroundMusicManager.Instance.ToggleBackgroundMusic();
        UpdateMusicButtonSprite();
    }

    private void UpdateMusicButtonSprite()
    {
        if (YandexGame.savesData.isMusicEnabled)
        {
            musicButton.sprite = musicOnSprite;
        }
        else
        {
            musicButton.sprite = musicOffSprite;
        }
    }

    public void SetCountdown(int countDown)
    {
        countDownText.gameObject.SetActive(true);
        countDownText.transform.localScale = new Vector3(.5f, .5f, .5f);
        countDownText.transform.LeanScale(Vector3.one, transitionTime).setEaseOutBack();
        countDownText.text = countDown.ToString();
    }

    public void StopCountdown()
    {
        countDownText.gameObject.SetActive(false);
    }

    public void LevelFinished()
    {
        Time.timeScale = 0;
        background.gameObject.SetActive(true);
        levelFinishedMenu.LeanScale(Vector3.one, transitionTime).setIgnoreTimeScale(true).setEaseOutBack();
    }
    public void LevelFailed()
    {
        Time.timeScale = 0;
        background.gameObject.SetActive(true);
        levelFailedMenu.LeanScale(Vector3.one, transitionTime).setIgnoreTimeScale(true).setEaseOutBack();
    }

    public void OnPauseButton()
    {
        Time.timeScale = 0;
        background.gameObject.SetActive(true);
        pauseMenu.LeanScale(Vector3.one, transitionTime).setIgnoreTimeScale(true).setEaseOutBack();
        CarController.Instance.StopEngineSound();
        UpdateSoundButtonSprite();
        UpdateMusicButtonSprite();
    }
    public void OnContinueButton()
    {
        Time.timeScale = 1;
        background.gameObject.SetActive(false);
        pauseMenu.LeanScale(Vector3.zero, transitionTime).setIgnoreTimeScale(true).setEaseInBack();
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnWatchAd()
    {
        Time.timeScale = 0;
        background.gameObject.SetActive(true);
        adMenu.LeanScale(Vector3.one, transitionTime).setIgnoreTimeScale(true).setEaseOutBack();
        CarController.Instance.StopEngineSound();
    }

    public void CloseAdTab()
    {
        Time.timeScale = 1;
        background.gameObject.SetActive(false);
        adMenu.LeanScale(Vector3.zero, transitionTime).setIgnoreTimeScale(true).setEaseInBack();
    }

    public void HideHelp()
    {
        helpMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void ShowHelp()
    {
        helpMenu.SetActive(true);
        background.gameObject.SetActive(false);
        pauseMenu.LeanScale(Vector3.zero, transitionTime).setIgnoreTimeScale(true).setEaseInBack();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex <= 20)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            ToMainMenu();
        }
    }

    public void ShowRewardedAdd()
    {
        CloseAdTab();
        YandexGame.RewVideoShow(1);
    }

    private void Rewarded(int id)
    {
        GameManager.Instance.LevelCompleted();
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }
}
