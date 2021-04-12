using System.Collections;
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
    public Texture2D renderTexture;
    public byte[] VideoRawBytes;
    public string file;
    public string OtherUserId;
    public ConfirmNotice confirmNotice;
    // Start is called before the first frame update
    void Start()
    {
        playerRI.enabled = true;

    }

    public void SetOtherUserId(string _otherUserId)
    {
        OtherUserId = _otherUserId;
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
            API.GetSingleton().GetCommsPreference(PlayerPrefs.GetString("UserId"), OtherUserId, (err, status) =>
            {
                if(status == -1)
                {
                    CommsMenuScaler.scaleIn();
                }
            });

        });
    }

    public void TouchOnSaveToGallery()
    {
#if UNITY_EDITOR
        Debug.Log("Image saved to gallery");
        confirmNotice.SetText("Vídeo guardado de coña");
        confirmNotice.Show();
#else
        NativeGallery.Permission perm = NativeGallery.SaveVideoToGallery(file, 
            "Volare", 
            "VolareVideo-"+new System.DateTime().ToString(), 
            (err) => {
                if(err==null) {
                    confirmNotice.SetText("Vídeo guardado");
                    confirmNotice.Show();
                }
                else {
                    confirmNotice.SetText("Error guardando vídeo: " + err);
                    confirmNotice.Show();
                }
        });
        if(perm!=NativeGallery.Permission.Granted) {
            confirmNotice.SetText("Esta app no tiene permiso para guardar vídeo en la galería");
            confirmNotice.Show();
        }
#endif
    }
}
