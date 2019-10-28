﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VolareVideoPlayer : MonoBehaviour
{
    public UIScaleFader Scaler;
    public UIScaleFader CommsMenuScaler;
    public RawImage playerRI;
    public VideoPlayer videoPlayer;
    public RawImage StopButtonRI;
    public RawImage PlayButtonRI;
    public RawImage PauseButtonRI;
    // Start is called before the first frame update
    void Start()
    {
        playerRI.enabled = true;
    }

    public void ShowCinema()
    {
        playerRI.enabled = true;
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        float aspect = width / height;
        float yScale = aspect / (9.0f / 16.0f);
        playerRI.transform.localScale = new Vector3(1, yScale, 1);
    }

    public void StartPlaying()
    {
        PlayButtonRI.enabled = false;
        PauseButtonRI.enabled = true;
        StopButtonRI.enabled = true;
        playerRI.enabled = true;
        videoPlayer.Play();
        Scaler.setEaseType(EaseType.cubicOut);
        Scaler.scaleIn();
    }

    public void TouchOnPlay()
    {
        videoPlayer.Play();
    }

    public void TouchOnPause()
    {
        if(videoPlayer.isPaused)
        {
            videoPlayer.Play();
            PlayButtonRI.enabled = false;
            PauseButtonRI.enabled = true;
        }
        else
        {
            videoPlayer.Pause();
            PlayButtonRI.enabled = true;
            PauseButtonRI.enabled = false;
        }

    }

    public void TouchOnStop()
    {
        videoPlayer.Stop();
        Scaler.setEaseType(EaseType.cubicIn);
        Scaler.scaleOut( () =>
        {
            playerRI.enabled = true;
            // if no communications agreement, show comms menu
            CommsMenuScaler.scaleIn();
        });
    }
}
