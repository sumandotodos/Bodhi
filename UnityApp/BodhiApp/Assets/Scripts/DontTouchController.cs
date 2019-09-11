using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontTouchController : MonoBehaviour
{
    public static DontTouchController instance;
    public UIFader DontTouchFader;

    private void Awake()
    {
        instance = this;
    }

    public static DontTouchController GetSingleton()
    {
        return instance;
    }

    public bool TouchPrevented;

    public void PreventTouch()
    {
        TouchPrevented = true;
        DontTouchFader.fadeToOpaque();

    }

    public void AllowTouch()
    {
        TouchPrevented = false;
        DontTouchFader.fadeToTransparent();
    }

}
