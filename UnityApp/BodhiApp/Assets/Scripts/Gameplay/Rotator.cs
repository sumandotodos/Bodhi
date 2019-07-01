using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    public Vector3 EulerAngularSpeeds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(
            EulerAngularSpeeds.x * Time.deltaTime,
            EulerAngularSpeeds.y * Time.deltaTime,
            EulerAngularSpeeds.z * Time.deltaTime,
        Space.Self);
    }
}
