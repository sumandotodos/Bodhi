using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{

    public OrbitalCamera orbitalCamera_A;

    System.Action updateDelegate;
    Vector3 touchCoordinates;
    public float PixelsToAngleFactor = 0.1f;
    float Yaw = 0.0f;
    float Pitch = 0.0f;

    private void Awake()
    {
        if(orbitalCamera_A==null)
        {
            orbitalCamera_A = FindObjectOfType<OrbitalCamera>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        updateDelegate = notTouchingUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        updateDelegate();
    }

    void isTouchingUpdate()
    {
        Vector3 touchPixelsDelta = (Input.mousePosition - touchCoordinates);
        float deltaAngleYaw = touchPixelsDelta.x * PixelsToAngleFactor;
        float deltaAnglePitch = -touchPixelsDelta.y * PixelsToAngleFactor;
        orbitalCamera_A.SetYAngleImmediate(Yaw+deltaAngleYaw);
        orbitalCamera_A.SetXAngleImmediate(Mathf.Clamp(Pitch+deltaAnglePitch, -170.0f, 170.0f));
        if (!Input.GetMouseButton(0))
        {
            Yaw += deltaAngleYaw;
            Pitch += deltaAnglePitch;
            Pitch = Mathf.Clamp(Pitch, -170.0f, 170.0f);
            updateDelegate = notTouchingUpdate;
        }
    }

    void notTouchingUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            touchCoordinates = Input.mousePosition;
            updateDelegate = isTouchingUpdate;
        }
    }
}
