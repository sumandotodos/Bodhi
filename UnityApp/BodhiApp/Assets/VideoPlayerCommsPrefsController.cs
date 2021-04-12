using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerCommsPrefsController : MonoBehaviour, OptionIndexReceiver
{
    public UIScaleFader CommsPane;
    public string OtherUserId;

    public void ReceiveIndex(int n)
    {
        API.GetSingleton().SetCommsPreference(PlayerPrefs.GetString("UserId"), OtherUserId, n, (err, result) =>
        {

        });
        CommsPane.scaleOut();
    }
}
