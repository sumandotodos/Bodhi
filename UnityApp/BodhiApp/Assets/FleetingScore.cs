using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class FleetingScore : MonoBehaviour
{
    TextMesh textMesh;
    Color TextMeshColor;
    TweenableSoftFloat Opacity;
    public float Delay1;
    public float Delay2;
    public float buoyancy;
    public Vector3 buoyancyDirection;
    float remaining1;
    float remaining2;
    bool isStarted = false;
    // Start is called before the first frame update
    public void Start()
    {
        if (isStarted) return;
        isStarted = true;
        textMesh = GetComponent<TextMesh>();
        TextMeshColor = textMesh.color;
        Opacity = new TweenableSoftFloat();
        Opacity.setSpeed(1.0f);
        Opacity.setValueImmediate(0.0f);
        Opacity.setValue(1.0f);
        remaining1 = Delay1;
        remaining2 = 0.0f;
    }

    public void Init(int score)
    {
        Start();
        textMesh.text = "+ " + score;
    }

    // Update is called once per frame
    void Update()
    {
        Opacity.update();
        Color newColor = TextMeshColor;
        newColor.a = Opacity.getValue();
        textMesh.color = newColor;

        if (remaining1 > 0.0f)
        {
            remaining1 -= Time.deltaTime;
            if (remaining1 <= 0.0f)
            {
                remaining2 = Delay2;
                Opacity.setValue(0.0f);
            }
        }

        if (remaining2 > 0.0f)
        {
            remaining2 -= Time.deltaTime;
            if (remaining2 <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }

        this.transform.localPosition += buoyancyDirection * buoyancy * Time.deltaTime;
    }
}
