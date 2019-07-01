using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera Camera_A;

    private void Awake()
    {
        if(Camera_A==null)
        {
            Camera_A = FindObjectOfType<Camera>();
        }
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera_A.transform.rotation * Vector3.forward,
            Camera_A.transform.rotation * Vector3.up);
    }
}
