using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeText : MonoBehaviour
{
    public Text AText;
    public Text BText;
    public Color color;
    TweenableSoftFloat AOpacity;
    TweenableSoftFloat BOpacity;
    Text[] texts;
    TweenableSoftFloat[] opacities;
    int currentText = 0;

    bool isStarted = false;
    // Start is called before the first frame update
    public void Start()
    {
        if (isStarted) return;
        isStarted = true;
        AOpacity = new TweenableSoftFloat(0.0f);
        BOpacity = new TweenableSoftFloat(0.0f);
        AOpacity.setSpeed(1.0f);
        BOpacity.setSpeed(1.0f);
        AOpacity.setEaseType(EaseType.linear);
        BOpacity.setEaseType(EaseType.linear);
        updateAColor();
        updateBColor();
        texts = new Text[2];
        texts[0] = AText;
        texts[1] = BText;
        opacities = new TweenableSoftFloat[2];
        opacities[0] = AOpacity;
        opacities[1] = BOpacity;
        currentText = 1;
    }

    private void updateAColor()
    {
        AText.color = new Color(color.r, color.g, color.b, AOpacity.getValue());
    }

    private void updateBColor()
    {
        BText.color = new Color(color.r, color.g, color.b, BOpacity.getValue());
    }

    // Update is called once per frame
    void Update()
    {
        if(AOpacity.update())
        {
            updateAColor();
        }

        if(BOpacity.update())
        {
            updateBColor();
        }
    }

    public void SetText(string t)
    {
        opacities[currentText].setValue(0.0f);
        currentText = (currentText + 1) % 2;
        texts[currentText].text = t;
        opacities[currentText].setValue(1.0f);
    }
}
