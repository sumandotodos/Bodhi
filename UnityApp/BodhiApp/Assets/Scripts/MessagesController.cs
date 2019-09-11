using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesController : MonoBehaviour
{
    static MessagesController instance;

    public UIScaleFader messageScaler;
    public Text messageText;

    void Awake()
    {
        instance = this;
    }

    public static MessagesController GetSingleton()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        messageScaler.gameObject.SetActive(false);   
    }

    public void ShowMessage(string text, Color color)
    {
        messageText.text = text;
        messageText.color = color;
        messageScaler.gameObject.SetActive(true);
        messageScaler.maxScale = 10.0f;
        messageScaler.minScale = 1.0f;
        messageScaler.speed = 10.0f;
        messageScaler.SetSpeed(10.0f);
        messageScaler.setEaseType(EaseType.cubicOut);
        messageScaler.scaleInImmediately();
        messageScaler.scaleOut();
    }

    public void ShowMessage(string text)
    {
        ShowMessage(text, Color.red);
    }

    public void DismissMessage()
    {
        messageScaler.maxScale = 1.0f;
        messageScaler.minScale = 0.0f;
        messageScaler.speed = 1.0f;
        messageScaler.SetSpeed(1.0f);
        messageScaler.setEaseType(EaseType.cubicIn);
        messageScaler.scaleInImmediately();
        messageScaler.scaleOut();
    }
}
