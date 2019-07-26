using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetor : MonoBehaviour
{
    public Vector2 Destination;
    public Vector2 CurrentPosition;
    public Vector2 CurrentSpeed;
    public float MaxAbsSpeedPerDimension = 40.0f;
    public float k = 0.5f;
    public float b = 3.5f;
    public bool Going = false;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if (!Going) return;
        Vector2 Diff = (Destination - CurrentPosition) - (b * CurrentSpeed);
        Diff = new Vector2(Mathf.Clamp(Diff.x, -MaxAbsSpeedPerDimension, MaxAbsSpeedPerDimension), Mathf.Clamp(Diff.y, -MaxAbsSpeedPerDimension, MaxAbsSpeedPerDimension));
        CurrentSpeed += Diff * Time.deltaTime * k;
        CurrentPosition += CurrentSpeed * Time.deltaTime;
        this.transform.localPosition = CurrentPosition;
    }
}
