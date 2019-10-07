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

    string QuestionId;
    string OtherUserId;

    public OtherUsersPlanetsController otherUsersPlanetController;

    string videoPath = "";

    string MessageRecipient = "";
    string AnsweredQuestion = "";

    public void TouchOnSendVideoResponse()
    {
        string userId = otherUsersPlanetController.GetUserId();
        MessageRecipient = userId;
        string question = otherUsersPlanetController.GetQuestion();
        string questionId = otherUsersPlanetController.GetQuestionId();
        if (question != "")
        {
            AnsweredQuestion = question;
            SelectSourceMenuScaler.scaleIn();

        }
        Debug.Log("Answering question: " + questionId + ", of user: " + userId);
        OtherUserId = userId;
        QuestionId = questionId;
    }

    public void TouchOnRecordVideo() {

        SelectSourceMenuScaler.scaleOut();

#if UNITY_EDITOR
        //FileUploadController.GetSingleton().StartFileUpload(
        //    Application.dataPath + TestVideoRecordPath,
        //    OtherUserId,
        //    QuestionId);
        videoPath = Application.dataPath + TestVideoRecordPath;
        ConfirmMenuScaler.scaleIn();
#else
            NativeCamera.RecordVideo((path) =>
            {
                FileUploadController.GetSingleton().StartFileUpload(path);
                videoPath = path;
                ConfirmMenuScaler.scaleIn();
            }, NativeCamera.Quality.Low);
#endif

    }

    public void TouchOnSelectFromGallery()
    {
        SelectSourceMenuScaler.scaleOut();

#if UNITY_EDITOR
        //FileUploadController.GetSingleton().StartFileUpload(
        //    Application.dataPath + TestGalleryRecordPath,
        //    OtherUserId,
        //    QuestionId);
        videoPath = Application.dataPath + TestVideoRecordPath;
        ConfirmMenuScaler.scaleIn();
#else
            NativeGallery.GetVideoFromGallery((path) =>
            {
                FileUploadController.GetSingleton().StartFileUpload(path);
                videoPath = path;
                ConfirmMenuScaler.scaleIn();
            }, NativeCamera.Quality.Low);
#endif
    }

    public void TouchOnCancel()
    {
        SelectSourceMenuScaler.scaleOut();
    }

    /*public void ShowVideoPreview(string path)
    {
        videoPlayer.url = path;
        videoPlayer.Play();
    }*/

    public void TouchOnYes()
    {
        //videoPlayer.Stop();
        FileUploadController.GetSingleton().StartFileUpload(
            videoPath,
            OtherUserId,
            QuestionId, 
             (result, fileid) =>
             {
                if(result=="success")
                    {
                        /*API.GetSingleton().CreateMessage(
                            PlayerPrefs.GetString("UserId"),
                            MessageRecipient,
                            "Answered Question",
                            AnsweredQuestion,
                            fileid);*/

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
