using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCameraOrbit : MonoBehaviour
{
    float Phase;
    public float Speed;
    public OrbitalCamera orbitalCamera;
    // Start is called before the first frame update
    void Start()
    {
        Phase = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Phase += Speed * Time.deltaTime;
        orbitalCamera.YPivot.transform.Rotate(new Vector3(0, Speed*Time.deltaTime, 0));
        //orbitalCamera.PitchOffset = Phase / 2.0f;
    }
}
