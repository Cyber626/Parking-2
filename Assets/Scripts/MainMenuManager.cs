using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private float transitionTime = 0.25f;
    [SerializeField] private Transform selectLevelMenu, selectLevelPage1, selectLevelPage2;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] levelLockers;
    [SerializeField] private Sprite soundOnSprite, soundOffSprite;
    [SerializeField] private Image soundButton;
    [SerializeField] private Sprite musicOnSprite, musicOffSprite;
    [SerializeField] private Image musicButton;
    private bool isFirstPage = true;

    public void OnPlay()
    {
        LoadManager.Instance.selectedLevel = YandexGame.savesData.clearedLevels + 1;
        SceneManager.LoadScene(21);
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


    public void LoadSelectedLevel(int level)
    {
        LoadManager.Instance.selectedLevel = level;
        SceneManager.LoadScene(21);
    }

    private void Start()
    {
        Time.timeScale = 1;
        int i;
        for (i = 0; i < YandexGame.savesData.clearedLevels; i++)
        {
            levelLockers[i].SetActive(false);
        }

        for (int j = i + 1; j < buttons.Length; j++)
        {
            buttons[j].enabled = false;
        }

        UpdateMusicButtonSprite();
        UpdateSoundButtonSprite();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { OnPrevPage(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { OnNextPage(); }
    }

    public void OnSelectLevel()
    {
        selectLevelMenu.LeanScale(Vector3.one, transitionTime).setEaseOutBack();
    }

    public void OnReturnFromSelLev()
    {
        selectLevelMenu.LeanScale(Vector3.zero, transitionTime).setEaseInBack();
    }

    public void OnNextPage()
    {
        if (isFirstPage)
        {
            isFirstPage = false;
            selectLevelPage2.LeanScale(Vector3.one, transitionTime);
            selectLevelPage2.LeanMoveLocalX(0, transitionTime);
            selectLevelPage1.LeanScale(Vector3.zero, transitionTime);
            selectLevelPage1.LeanMoveLocalX(-2000, transitionTime);
        }
    }

    public void OnPrevPage()
    {
        if (!isFirstPage)
        {
            isFirstPage = true;
            selectLevelPage2.LeanScale(Vector3.zero, transitionTime);
            selectLevelPage2.LeanMoveLocalX(2000, transitionTime);
            selectLevelPage1.LeanScale(Vector3.one, transitionTime);
            selectLevelPage1.LeanMoveLocalX(0, transitionTime);
        }
    }
}
