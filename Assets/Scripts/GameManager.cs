using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int levelNo;
    [SerializeField] private PlayerCarsSO playerCarsSO;
    [SerializeField] private Transform playerCarCreationPoint;
    [SerializeField] private CameraFollowScript cameraFollowScript;
    [SerializeField] private GameObject directionArrowPrefab;
    [SerializeField] private Vector2 arrowOffset;
    [SerializeField] private Transform[] followObjects;
    [SerializeField] private AudioClip levelFailedFX, levelPassedFX;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        GameObject selectedCar = playerCarsSO.playerCars[LoadManager.Instance.carIndex].cars[LoadManager.Instance.colorIndex];
        GameObject selectedCarObject = Instantiate(selectedCar, playerCarCreationPoint.position, playerCarCreationPoint.rotation);
        cameraFollowScript.followObject = selectedCarObject;
        Destroy(playerCarCreationPoint.gameObject);

        GameObject directionArrow = Instantiate(directionArrowPrefab, selectedCarObject.transform);
        directionArrow.transform.localPosition = arrowOffset;
        DirectionArrow directionArrowScript = directionArrow.GetComponent<DirectionArrow>();

        for (int i = 0; i < followObjects.Length; i++)
        {
            directionArrowScript.EnqueueTransform(followObjects[i]);
        }

        directionArrowScript.Next();
        selectedCarObject.GetComponent<PlayerParkingInteraction>().directionArrow = directionArrowScript;
    }

    public int GetCurrentLevelNumber()
    {
        return levelNo;
    }

    public void LevelCompleted()
    {
        YandexGame.savesData.clearedLevels = Mathf.Max(YandexGame.savesData.clearedLevels, levelNo);
        YandexGame.SaveProgress();
        UIManager.Instance.LevelFinished();
        if (YandexGame.savesData.isSoundEnabled)
        {
            audioSource.clip = levelPassedFX;
            audioSource.Play();
        }
    }

    public void LevelFailed()
    {
        UIManager.Instance.LevelFailed();
        if (YandexGame.savesData.isSoundEnabled)
        {
            audioSource.clip = levelFailedFX;
            audioSource.Play();
        }
    }
}
