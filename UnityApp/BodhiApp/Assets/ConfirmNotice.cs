using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmNotice : MonoBehaviour
{
    public Text textComponent;
    public UIScaleFader scaler;

    public void SetText(string txt)
    {
        textComponent.text = txt;
    }

    public void Dismiss()
    {
        scaler.setEaseType(EaseType.cubicIn);
        scaler.scaleOut();
    }

    public void Show()
    {
        scaler.setEaseType(EaseType.cubicOut);
        scaler.scaleIn();
    }
}
