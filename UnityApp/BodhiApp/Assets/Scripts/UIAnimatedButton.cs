using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimatedButton : MonoBehaviour
{
    public string OnPressAnimName;
    public string OnReleaseAnimName;
    public UIAnimatedImage animatedImage;
    public UIMoverTwoPoints labelMover;
    public float ReactivationDelay = 5.0f;
    float Remaining = 0.0f;

    public MonoBehaviour TargetScript;
    public string MethodToInvoke;

    private void Update()
    {
        if(Remaining > 0.0f)
        {
            Remaining -= Time.deltaTime;
        }
    }

    public void OnPress()
    {
       
       animatedImage.PlaySegment(OnPressAnimName);
       labelMover.Go();
       
    }

    public void OnRelease()
    {
        animatedImage.PlaySegment(OnReleaseAnimName);
        labelMover.Return();
        if (Remaining <= 0.0f)
        {
            Remaining = ReactivationDelay;
            Debug.Log("Trying to invoke button method...");
            if (TargetScript != null && MethodToInvoke != "")
            {
                TargetScript.Invoke(MethodToInvoke, 0.0f);
            }
        }
    }
}
