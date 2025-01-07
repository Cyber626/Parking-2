using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParkingInteraction : MonoBehaviour
{
    public DirectionArrow directionArrow;
    [SerializeField] private float countDownTimer = 3f;
    private int borderStayingCount = 0, prevCountdown;
    private float timer = 0;
    private Rigidbody2D rb;
    private bool isParking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isParking && borderStayingCount == 0 && rb.velocity.magnitude < 0.25f)
        {
            if (timer < 0)
            {
                UIManager.Instance.StopCountdown();
                GameManager.Instance.LevelCompleted();
                GetComponent<CarController>().StopEngineSound();
            }
            timer -= Time.deltaTime;
            int currentCountdown = (int)timer + 1;
            if (currentCountdown != prevCountdown)
            {
                UIManager.Instance.SetCountdown(currentCountdown);
                prevCountdown = currentCountdown;   
            }
        }
        else
        {
            timer = countDownTimer;
            if (prevCountdown != 0)
            {
                prevCountdown = 0;
                UIManager.Instance.StopCountdown();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<CarController>().StopEngineSound();
        GameManager.Instance.LevelFailed();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ParkingBorder"))
        {
            borderStayingCount++;
            collision.GetComponent<ParkingBorder>().PlayerEntered();
        }
        else if (collision.CompareTag("ParkingSpot"))
        {
            directionArrow.gameObject.SetActive(false);
            isParking = true;
        }
        else if (collision.CompareTag("Direction"))
        {
            Destroy(collision.gameObject);
            directionArrow.Next();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ParkingBorder"))
        {
            borderStayingCount--;
            collision.GetComponent<ParkingBorder>().PlayerExited();
        }
        else if (collision.CompareTag("ParkingSpot"))
        {
            isParking = false;
        }
    }
}
