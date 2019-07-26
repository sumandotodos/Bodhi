using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera Camera_A;

    public float Height = 0.0f;

    Vector3 InitialPosition;

    private void Awake()
    {
        if(Camera_A==null)
        {
            Camera_A = FindObjectOfType<Camera>();
        }
        InitialPosition = this.transform.position;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera_A.transform.rotation * Vector3.forward,
            Camera_A.transform.rotation * Vector3.up);

        if (Height > 0.0f)
        {
            this.transform.position = InitialPosition + (Camera_A.transform.position - InitialPosition).normalized * Height;
        }

    }
}
