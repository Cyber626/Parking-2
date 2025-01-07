using UnityEngine;
using UnityEngine.UI;

public class MobileController : MonoBehaviour
{
    [SerializeField] private Image gearButton;
    [SerializeField] private Sprite gearLowSprite, gearHighSprite;
    [SerializeField] private Image handBrakeButton;
    [SerializeField] private Sprite handBrakeOnSprite, handBrakeOffSprite;

    public static int horizontalAxis, verticalAxis;

    public static void OnVerticalDown(int val) { verticalAxis = val; }
    public static void OnHorizontalDown(int val) { horizontalAxis = val; }
    public static void OnHorizontalUp() { horizontalAxis = 0; }
    public static void OnVerticalUp() { verticalAxis = 0; }

    public void OnHandBrake()
    {
        CarController.Instance.ToggleHandBrake();
        if (CarController.Instance.isHandBrakeOn)
        {
            handBrakeButton.sprite = handBrakeOnSprite;
        }
        else
        {
            handBrakeButton.sprite = handBrakeOffSprite;
        }
    }
    public void OnGear()
    {
        CarController.Instance.ChangeGear();
        if (CarController.Instance.acceleration >= CarController.Instance.accelerationHigh)
        {
            gearButton.sprite = gearHighSprite;
        }
        else
        {
            gearButton.sprite = gearLowSprite;
        }
    }
}
