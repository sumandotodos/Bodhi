using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

    public OrbitalObject orbitalCamera;

    System.Action updateDelegate;
    Vector3 touchCoordinates;
    Vector3 lastFrameCoordinates;
    public float PixelsToAngleFactor = 0.1f;
    float Yaw = 0.0f;
    float Pitch = 0.0f;

    public float YawSpeed = 0;
    float PitchSpeed = 0;

    public float Accel = 100.0f;
    float PitchAccel = 0.0f;
    float YawAccel = 0.0f;
    public float DeltaPixelsToSpeed = 1.0f;

    public float RefreshTime = 0.32f;
    float Remaining;

    // Start is called before the first frame update
    void Start()
    {
        updateDelegate = notTouchingUpdate;
        Remaining = RefreshTime;
    }

    // Update is called once per frame
    void Update()
    {
        updateDelegate();
        updatePitchYaw();
    }

    private void LateUpdate()
    {
        Remaining -= Time.deltaTime;
        if (Remaining <= 0.0f)
        {
            Remaining = RefreshTime;
            lastFrameCoordinates = Input.mousePosition;
        }
    }

    private void updatePitchYaw()
    {
        Yaw += YawSpeed * Time.deltaTime;
        Pitch = Mathf.Clamp(Pitch + PitchSpeed * Time.deltaTime, -85.0f, 85.0f);
        YawSpeed += YawAccel * Time.deltaTime;
        if(YawSpeed * YawAccel > 0.0f)
        {
            YawSpeed = 0.0f;
            YawAccel = 0.0f;
        }
        PitchSpeed += PitchAccel * Time.deltaTime;
        if(PitchSpeed * PitchAccel > 0.0f)
        {
            PitchSpeed = 0.0f;
            PitchAccel = 0.0f;
        }

    }

    void isTouchingUpdate()
    {
        Vector3 touchPixelsDelta = (Input.mousePosition - touchCoordinates);
        float deltaAngleYaw = touchPixelsDelta.x * PixelsToAngleFactor;
        float deltaAnglePitch = -touchPixelsDelta.y * PixelsToAngleFactor;
        orbitalCamera.SetYAngleImmediate(Yaw+deltaAngleYaw);
        orbitalCamera.SetXAngleImmediate(Mathf.Clamp(Pitch+deltaAnglePitch, -85.0f, 85.0f));
        if (!Input.GetMouseButton(0))
        {
            Yaw += deltaAngleYaw;
            Pitch += deltaAnglePitch;
            Pitch = Mathf.Clamp(Pitch, -85.0f, 85.0f);
            updateDelegate = notTouchingUpdate;
            PitchSpeed = DeltaPixelsToSpeed * -(Input.mousePosition.y - lastFrameCoordinates.y);
            YawSpeed = DeltaPixelsToSpeed * (Input.mousePosition.x - lastFrameCoordinates.x);

            Debug.Log("<color=blue>" + YawSpeed + "</color>");
            if(PitchSpeed > 0.0f)
            {
                PitchAccel = -Accel;
            }
            else
            {
                PitchAccel = Accel;
            }
            if(YawSpeed > 0.0f)
            {
                YawAccel = -Accel;
            }
            else
            {
                YawAccel = Accel;
            }

        }

    }

    void notTouchingUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            touchCoordinates = Input.mousePosition;
            updateDelegate = isTouchingUpdate;
            YawSpeed = 0.0f;
            PitchSpeed = 0.0f;
        }
        orbitalCamera.SetYAngleImmediate(Yaw);
        orbitalCamera.SetXAngleImmediate(Mathf.Clamp(Pitch, -85.0f, 85.0f));
    }
}
