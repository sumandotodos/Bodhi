using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float Amplitude;
    public float Frequency;

    float Phase;
    TweenableSoftFloat Magnitude;

    Vector3 InitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        Magnitude = new TweenableSoftFloat(0.0f);
        Magnitude.setSpeed(6.0f);
        Phase = Random.Range(0.0f, 600.0f);
        InitialPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Phase += Frequency * Time.deltaTime;
        Magnitude.update();
        Vector3 Noise = new Vector3(Mathf.Sin(Phase), Mathf.Cos(Phase * 1.42f), Mathf.Sin(Phase * 1.27f));
        this.transform.localPosition = InitialPosition + Amplitude * Magnitude.getValue() * Noise;
    }

    public void Shake()
    {
        Debug.Log("Shake!!");
        StartCoroutine(ShakeCoroutine(0.75f));
    }

    IEnumerator ShakeCoroutine(float duration)
    {
        Magnitude.setValue(1.0f);
        yield return new WaitForSeconds(duration);
        Magnitude.setValue(0.0f);
    }

    public void SetMagnitude(float mag)
    {
        Magnitude.setValue(mag);
    }
}
