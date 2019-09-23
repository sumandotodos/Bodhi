using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SendVideoResponseController : MonoBehaviour
{
    public UIScaleFader SelectSourceMenuScaler;
    public UIScaleFader ConfirmMenuScaler;
    public VideoPlayer videoPlayer;

    public string TestVideoRecordPath = "/Resources/Video/DefaultVideo.MP4";
    public string TestGalleryRecordPath = "/Resources/Video/DefaultGalleryVideo.MP4";

    public OtherUsersPlanetsController otherUsersPlanetController;

    string videoPath = "";

    string MessageRecipient = "";
    string AnsweredQuestion = "";

    public void TouchOnSendVideoResponse()
    {
        string userId = otherUsersPlanetController.GetUserId();
        MessageRecipient = userId;
        string question = otherUsersPlanetController.GetQuestion();
        if (question != "")
        {
            AnsweredQuestion = question;
            SelectSourceMenuScaler.scaleIn();

        }
    }

    public void TouchOnRecordVideo() {

        SelectSourceMenuScaler.scaleOut();

#if UNITY_EDITOR
        //FileUploadController.GetSingleton().StartFileUpload(Application.dataPath + TestVideoRecordPath);
        videoPath = Application.dataPath + TestVideoRecordPath;
        ShowVideoPreview(videoPath);
        ConfirmMenuScaler.scaleIn();
#else
            NativeCamera.RecordVideo((path) =>
            {
                //FileUploadController.GetSingleton().StartFileUpload(path);
                videoPath = path;
                ShowVideoPreview(videoPath);
                ConfirmMenuScaler.scaleIn();
            }, NativeCamera.Quality.Low);
#endif

    }

    public void TouchOnSelectFromGallery()
    {
        SelectSourceMenuScaler.scaleOut();
    }

    public void TouchOnCancel()
    {
        SelectSourceMenuScaler.scaleOut();
    }

    public void ShowVideoPreview(string path)
    {
        videoPlayer.url = path;
        videoPlayer.Play();
    }

    public void TouchOnYes()
    {
        videoPlayer.Stop();
        FileUploadController.GetSingleton().StartFileUpload(videoPath, 
        (result, fileid) =>
        {
            if(result=="success")
            {
                API.GetSingleton().CreateMessage(
                        PlayerPrefs.GetString("UserId"),
                        MessageRecipient,
                        "Answered Question",
                        AnsweredQuestion,
                        fileid);

            }
        });
        ConfirmMenuScaler.scaleOut();
    }

    public void TouchOnNo()
    {
        videoPlayer.Stop();
        ConfirmMenuScaler.scaleOut();
    }

}
