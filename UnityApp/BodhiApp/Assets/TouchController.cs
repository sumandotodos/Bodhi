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
        float touchPixelsDelta = (Input.mousePosition - touchCoordinates).x;
        float deltaAngle = touchPixelsDelta * PixelsToAngleFactor;
        orbitalCamera_A.SetYAngle(Yaw+deltaAngle);
        if(!Input.GetMouseButton(0))
        {
            Yaw += deltaAngle;
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
