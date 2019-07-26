using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalInlet : OrbitalObject
{
    float Pitch, Yaw;

    override public void SetYAngleImmediate(float angle)
    {
        Yaw = angle;
    }

    override public void SetXAngleImmediate(float angle)
    {
        Pitch = angle;
    }

    public float GetYAngle()
    {
        return Yaw;
    }

    public float GetXAngle()
    {
        return Pitch;
    }

}
