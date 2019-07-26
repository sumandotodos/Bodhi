using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalAdder : MonoBehaviour
{
    public OrbitalInlet[] inlets;
    public OrbitalCamera orbitalCamera;

    void Update()
    {
        float Pitch = 0.0f;
        float Yaw = 0.0f;
        foreach(OrbitalInlet i in inlets)
        {
            Yaw += i.GetYAngle();
            Pitch += i.GetXAngle();
        }
        orbitalCamera.SetYAngleImmediate(Yaw);
        orbitalCamera.SetXAngleImmediate(Pitch);
    }
}
