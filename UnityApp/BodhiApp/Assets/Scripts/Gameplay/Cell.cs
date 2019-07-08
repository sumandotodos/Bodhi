﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public OpacityController foreOpacity;
    public OpacityController detailOpacity;
    public SpriteRenderer hasBichoRenderer;
    public string OKAnimName = "";
    public string WreckAnimName = "";
    public TextMesh hintText;
    bool isStarted = false;
    bool hasBicho = false;
    bool hasBeenTouched = false;
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
            foreOpacity.GetComponent<UIAnimatedImage>().go();
        }
        else
        {
            foreOpacity.SetOpacity(0.0f, delay);
            detailOpacity.SetOpacity(0.0f, delay);
        }
        hasBeenTouched = true;
        clearBicho();
    }
}
