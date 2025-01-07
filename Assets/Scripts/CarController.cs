using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CarController : MonoBehaviour
{
    public static CarController Instance { get; private set; }
    public float accelerationHigh, acceleration;
    public bool isHandBrakeOn;
    [SerializeField] private float accelerationLow, steeringMax, braking, rpmMax, rpmIdle, rpmAccel;
    private RealisticEngineSound carAudio;
    private Rigidbody2D rb;
    private float accelerationFactor, initialDrag, rpm;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        carAudio = GetComponent<RealisticEngineSound>();
        rb = GetComponent<Rigidbody2D>();
        accelerationFactor = 0.25f * accelerationHigh;
        initialDrag = rb.drag;
        acceleration = accelerationHigh;
        rpm = rpmIdle;
    }

    public void StopEngineSound()
    {
        carAudio.engineCurrentRPM = 0;
    }

    void FixedUpdate()
    {
        float h, v;

        if (MobileController.horizontalAxis != 0)
        { h = MobileController.horizontalAxis; }
        else { h = -Input.GetAxis("Horizontal"); }

        if (MobileController.verticalAxis != 0)
        { v = MobileController.verticalAxis; }
        else { v = Input.GetAxis("Vertical"); }

        if (YandexGame.savesData.isSoundEnabled)
        {
            HandleAudio(v);
        }
        else
        {
            carAudio.engineCurrentRPM = 0;
        }

        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        float steering;
        if (rb.velocity.magnitude > 0)
        {
            steering = (accelerationFactor / rb.velocity.magnitude) + 0.25f;
            steering = Mathf.Clamp(steering, 0.5f, 1);
            steering *= steeringMax;
        }
        else
        {
            steering = 0.25f;
        }

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 5.0f);
            //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        AddRelativeForce();
    }

    private void HandleAudio(float vertical)
    {
        if (vertical > -0.01 && vertical < 0.01)
        {
            if (rpm > 0)
            {
                rpm -= rpmAccel * 2 * Time.fixedDeltaTime;
            }
            else
            {
                rpm += rpmAccel * 2 * Time.fixedDeltaTime;
            }
        }
        else
        {
            rpm += vertical * rpmAccel * Time.fixedDeltaTime;
        }

        if (rpm < rpmIdle && rpm > 0)
        {
            rpm = -rpmIdle;
        }
        else if (rpm > -rpmIdle && rpm < 0)
        {
            rpm = rpmIdle;
        }
        else if (rpm > rpmMax)
        {
            rpm = rpmMax;
        }
        else if (rpm < -rpmMax)
        {
            rpm = -rpmMax;
        }
        carAudio.engineCurrentRPM = Mathf.Abs(rpm);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleHandBrake();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeGear();
        }
    }

    public void ToggleHandBrake()
    {
        if (isHandBrakeOn)
        {
            isHandBrakeOn = false;
            rb.drag = initialDrag;
        }
        else
        {
            isHandBrakeOn = true;
            rb.drag = braking;
        }

        if (YandexGame.EnvironmentData.isDesktop || !isHandBrakeOn)
        {
            UIManager.Instance.OnHandBrakeToggle(isHandBrakeOn);
        }
    }

    public void ChangeGear()
    {
        if (acceleration >= accelerationHigh)
        {
            acceleration = accelerationLow;
            if (YandexGame.EnvironmentData.isDesktop) { UIManager.Instance.OnLowGearToggle(true); }
        }
        else
        {
            acceleration = accelerationHigh;
            UIManager.Instance.OnLowGearToggle(false);
        }
    }

    private void AddRelativeForce()
    {
        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;
        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

        //Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }
}
