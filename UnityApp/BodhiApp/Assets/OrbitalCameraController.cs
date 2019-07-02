using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalCameraController : MonoBehaviour
{

    public OrbitalCamera orbitalCamera;
    public float yAngle;
    public float xAngle;
    public float yPhase;
    public float xPhase;
    public float ySpeed;
    public float xSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        orbitalCamera.SetYAngle(Mathf.Sin(yPhase)*yAngle);
        orbitalCamera.SetXAngle(Mathf.Sin(xPhase)*xAngle);
        yPhase += ySpeed * Time.deltaTime;
        xPhase += xSpeed * Time.deltaTime;
    }
}
