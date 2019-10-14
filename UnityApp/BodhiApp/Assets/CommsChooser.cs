using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface OptionIndexReceiver
{
    void ReceiveIndex(int index); 
}

public class CommsChooser : MonoBehaviour
{
    public Color ChatColor;
    public Color AudioColor;
    public Color VideoColor;

    public Image ChatImage;
    public Image AudioImage;
    public Image VideoImage;

    int LastTouchIndex = -1;

    public MonoBehaviour indexReceiver;

    private void ActivateToIndex(int n)
    {
        ((OptionIndexReceiver)indexReceiver).ReceiveIndex(n);
        SetIndex(n);

    }

    public void SetIndex(int n)
    {
        ChatImage.color = AudioImage.color = VideoImage.color = Color.white;
        if (n >= 0)
        {
            ChatImage.color = ChatColor;
        }
        if (n >= 1)
        {
            AudioImage.color = AudioColor;
        }
        if (n >= 2)
        {
            VideoImage.color = VideoColor;
        }
    }

    public void TouchOnChat()
    {
        if (LastTouchIndex == 0)
        {
            ActivateToIndex(-1);
            LastTouchIndex = -1;
        }
        else
        {
            ActivateToIndex(0);
            LastTouchIndex = 0;
        }
    }

    public void TouchOnAudio()
    {
        if (LastTouchIndex == 1)
        {
            ActivateToIndex(-1);
            LastTouchIndex = -1;
        }
        else
        {
            ActivateToIndex(1);
            LastTouchIndex = 1;
        }
    }

    public void TouchOnVideo()
    {
        if (LastTouchIndex == 2)
        {
            ActivateToIndex(-1);
            LastTouchIndex = -1;
        }
        else
        {
            ActivateToIndex(2);
            LastTouchIndex = 2;
        }
    }
}
