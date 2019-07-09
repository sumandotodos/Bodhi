using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public OpacityController foreOpacity;
    public OpacityController detailOpacity;
    public SpriteRenderer hasBichoRenderer;
    public string OKAnimName = "";
    public string WreckAnimName = "";
    public float delayWindow = 0.05f;
    public TextMesh hintText;
    bool isStarted = false;
    bool hasBicho = false;
    bool hasBeenTouched = false;
    public bool FadeDetailOnOK = false;
    public bool FadeDetailOnWreck = true;
    int neighbors = 0;
    public Color[] colors;
    // Start is called before the first frame update
    public void Start()
    {
        if (isStarted) return;
        isStarted = true;
        foreOpacity.Start();
        foreOpacity.SetOpacityImmediate(1.0f);
        detailOpacity.Start();
        detailOpacity.SetOpacityImmediate(1.0f);
        hasBichoRenderer.enabled = false;
        hasBicho = false;
        hasBeenTouched = false;
        hintText.text = "";
    }

    public void setBicho()
    {
        hasBichoRenderer.enabled = true;
        hasBicho = true;
    }

    public void clearBicho()
    { 
        hasBicho = false;
    }

    public bool getBicho()
    {
        return hasBicho;
    }

    public bool isTerminal()
    {
        return hasBicho ||
            hasBeenTouched;
    }

    public bool isFrontier()
    {
        return neighbors > 0;
    }

    public void incNeighbors()
    {
        if (hasBicho) return;
        neighbors++;
        hintText.text = "" + neighbors;
        hintText.color = colors[neighbors%8];
    }

    public void touch(int delay)
    {
        if(hasBicho)
        {
            //foreOpacity.GetComponent<UIAnimatedImage>().PlayheadStart = 0;
            //foreOpacity.GetComponent<UIAnimatedImage>().PlayheadEnd = 15;
            foreOpacity.GetComponent<UIAnimatedImage>().PlaySegment(OKAnimName);
            foreOpacity.GetComponent<UIAnimatedImage>().go((float)(delay-1)*delayWindow);
            if(FadeDetailOnOK)
            {
                detailOpacity.SetOpacity(0.0f);
            }
        }
        else
        {
            //foreOpacity.GetComponent<UIAnimatedImage>().PlayheadStart = 16;
            //foreOpacity.GetComponent<UIAnimatedImage>().PlayheadEnd = 23;
            foreOpacity.GetComponent<UIAnimatedImage>().PlaySegment(WreckAnimName);
            foreOpacity.GetComponent<UIAnimatedImage>().go((float)(delay - 1) * delayWindow);
            detailOpacity.SetOpacityImmediate(0.0f);
            foreOpacity.SetOpacity(0.0f, delay+9);
            detailOpacity.SetOpacity(0.0f, delay+9);
        }
        hasBeenTouched = true;
        clearBicho();
    }
}
