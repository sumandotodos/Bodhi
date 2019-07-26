using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCameraOrbit : MonoBehaviour
{
    float Phase;
    public float Speed;
    public float MaxPitch = 20.0f;
    public OrbitalObject orbitalCamera;
    // Start is called before the first frame update
    void Start()
    {
        Phase = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Phase += Speed * Time.deltaTime;
        orbitalCamera.SetYAngleImmediate(Phase);
        orbitalCamera.SetXAngleImmediate(MaxPitch * Mathf.Sin(Phase/60.0f));
    }
}
