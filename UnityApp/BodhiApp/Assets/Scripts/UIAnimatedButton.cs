using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimatedButton : MonoBehaviour
{
    public string OnPressAnimName;
    public string OnReleaseAnimName;
    public UIAnimatedImage animatedImage;
    public UIMoverTwoPoints labelMover;

    public MonoBehaviour TargetScript;
    public string MethodToInvoke;

    public void OnPress()
    {
        animatedImage.PlaySegment(OnPressAnimName);
        labelMover.Go();
    }

    public void OnRelease()
    {
        animatedImage.PlaySegment(OnReleaseAnimName);
        labelMover.Return();
        if(TargetScript != null && MethodToInvoke != "")
        {
            TargetScript.Invoke(MethodToInvoke, 0.0f);
        }
    }
}
