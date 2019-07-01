using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCamera : MonoBehaviour
{
    public GameObject YPivot;
    public GameObject XPivot;
    public GameObject CameraHolder;

    public void SetYAngle(float angle)
    {
        YPivot.transform.rotation = Quaternion.EulerAngles(0, angle, 0);
    }

    public void SetXAngle(float angle)
    {
        XPivot.transform.rotation = Quaternion.EulerAngles(angle, 0, 0);
    }

    public void SetZDistance(float distance)
    {
        CameraHolder.transform.localPosition = new Vector3(0, 0, -distance);
    }
}
