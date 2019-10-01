using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VolareVideoPlayer : MonoBehaviour
{
    public UIScaleFader Scaler;
    public RawImage playerRI;
    public VideoPlayer videoPlayer;
    public RawImage StopButtonRI;
    public RawImage PlayButtonRI;
    public RawImage PauseButtonRI;
    // Start is called before the first frame update
    void Start()
    {
        //playerRI.enabled = false;
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
            //playerRI.enabled = false;
        });
    }
}
