using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class SelectCarManager : MonoBehaviour
{
    [Serializable]
    public class CarSet
    {
        public int requiredLevel;
        public Transform[] cars;
    }

    [SerializeField] private Transform cameraTransform, adMenu;
    [SerializeField] private float cameraXMovement, cameraYMovement, transitionTime = 0.25f;
    [SerializeField] private CarSet[] allCars;
    [SerializeField] private GameObject lockedByLevel, lockedByAd, playButton;
    [SerializeField] private TextMeshProUGUI requiredLevelText;
    private int carIndex, colorIndex;
    private Vector3 cameraInitialPos;
    private Transform selectedCar;

    private void Start()
    {
        carIndex = LoadManager.Instance.carIndex;
        colorIndex = LoadManager.Instance.colorIndex;
        cameraInitialPos = cameraTransform.position;
        GetNewSelectedCar();
        MoveCameraToSelectedCar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { PrevCar(); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { NextCar(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { NextColor(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { PrevColor(); }
    }

    public void SelectCar()
    {
        LoadManager.Instance.carIndex = carIndex;
        LoadManager.Instance.colorIndex = colorIndex;
        SceneManager.LoadScene(LoadManager.Instance.selectedLevel);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void WatchAdToUnlock()
    {
        adMenu.LeanScale(Vector3.one, transitionTime).setIgnoreTimeScale(true).setEaseOutBack();
    }

    public void CloseAdTab()
    {
        adMenu.LeanScale(Vector3.zero, transitionTime).setIgnoreTimeScale(true).setEaseInBack();
    }

    private void Rewarded(int id)
    {
        CloseAdTab();
        YandexGame.savesData.unlockedByAdCars[id - 100] = true;
        YandexGame.SaveProgress();
        UpdateSelectedCar();
    }

    public void ShowRewardedAd()
    {
        YandexGame.RewVideoShow(allCars[carIndex].requiredLevel);
    }


    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }


    public void NextCar()
    {
        if (++carIndex < allCars.Length)
        {
            colorIndex = 0;
            UpdateSelectedCar();
        }
        else
        {
            carIndex--;
        }
    }

    public void PrevCar()
    {
        if (carIndex > 0)
        {
            colorIndex = 0;
            carIndex--;
            UpdateSelectedCar();
        }
    }

    public void NextColor()
    {
        if (allCars[carIndex].cars.Length > ++colorIndex)
        {
            UpdateSelectedCar();
        }
        else
        {
            colorIndex--;
        }
    }

    public void PrevColor()
    {
        if (colorIndex > 0)
        {
            colorIndex--;
            UpdateSelectedCar();
        }
    }

    private void MoveCameraToSelectedCar()
    {
        cameraTransform.LeanMove(new Vector3(cameraInitialPos.x + cameraXMovement * colorIndex,
                                    cameraInitialPos.y + cameraYMovement * carIndex,
                                    cameraInitialPos.z
            ), transitionTime);
    }

    private void UpdateSelectedCar()
    {
        selectedCar.LeanScale(Vector3.one, transitionTime);
        selectedCar.GetComponent<RealisticEngineSound>().enabled = false;
        GetNewSelectedCar();
        MoveCameraToSelectedCar();
    }

    private void GetNewSelectedCar()
    {
        selectedCar = allCars[carIndex].cars[colorIndex];
        selectedCar.LeanScale(new Vector3(1.5f, 1.5f, 1.5f), transitionTime);
        if (YandexGame.savesData.isSoundEnabled)
        {
            RealisticEngineSound soundFX = selectedCar.GetComponent<RealisticEngineSound>();
            soundFX.enabled = true;
            soundFX.engineCurrentRPM = 800;
        }

        if (allCars[carIndex].requiredLevel >= 100 && !YandexGame.savesData.unlockedByAdCars[allCars[carIndex].requiredLevel - 100])
        {
            lockedByLevel.SetActive(false);
            lockedByAd.SetActive(true);
            playButton.SetActive(false);
        }
        else if (allCars[carIndex].requiredLevel > YandexGame.savesData.clearedLevels &&
                    allCars[carIndex].requiredLevel < 100)
        {
            lockedByLevel.SetActive(true);
            lockedByAd.SetActive(false);
            playButton.SetActive(false);
            requiredLevelText.text = allCars[carIndex].requiredLevel.ToString();
        }
        else
        {
            lockedByLevel.SetActive(false);
            lockedByAd.SetActive(false);
            playButton.SetActive(true);
        }
    }
}
