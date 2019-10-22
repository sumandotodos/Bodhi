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

    static SendVideoResponseController instance;

    private void Awake()
    {
        instance = this;
    }


    public static void TouchOnSendVideoResponse()
    {
        instance.touchOnSendVideoResponse();
    }
    public void touchOnSendVideoResponse()
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


    public static void TouchOnSendVideoResponse(string _OtherUserId, string question, string _QuestionId)
    {
        instance.touchOnSendVideoResponse(_OtherUserId, question, _QuestionId);
    }
    public void touchOnSendVideoResponse(string _OtherUserId, string question, string _QuestionId)
    {
        MessageRecipient = _OtherUserId;
        OtherUserId = _OtherUserId;
        QuestionId = _QuestionId;
        AnsweredQuestion = question;
        SelectSourceMenuScaler.scaleIn();
    }


    public static void TouchOnRecordVideo()
    {
        instance.touchOnRecordVideo();
    }
    public void touchOnRecordVideo() {

        SelectSourceMenuScaler.scaleOut();

#if UNITY_EDITOR
       
        videoPath = Application.dataPath + TestVideoRecordPath;
        ConfirmMenuScaler.scaleIn();
#else
            NativeCamera.RecordVideo((path) =>
            {
                //FileUploadController.GetSingleton().StartFileUpload(path);
                videoPath = path;
                ConfirmMenuScaler.scaleIn();
            }, NativeCamera.Quality.Low, 60, 200000000);
#endif

    }


    public static void TouchOnSelectFromGallery()
    {
        instance.touchOnSelectFromGallery();
    }
    public void touchOnSelectFromGallery()
    {
        SelectSourceMenuScaler.scaleOut();

#if UNITY_EDITOR
       
        videoPath = Application.dataPath + TestVideoRecordPath;
        ConfirmMenuScaler.scaleIn();
      
#else
            NativeGallery.GetVideoFromGallery((path) =>
            {
                //FileUploadController.GetSingleton().StartFileUpload(path);
                videoPath = path;
                ConfirmMenuScaler.scaleIn();
            });
#endif
    }



    public static void TouchOnCancel()
    {
        instance.touchOnCancel();
    }
    public void touchOnCancel()
    {
        SelectSourceMenuScaler.scaleOut();
    }

    /*public void ShowVideoPreview(string path)
    {
        videoPlayer.url = path;
        videoPlayer.Play();
    }*/

    public static void TouchOnYes()
    {
        instance.touchOnYes();
    }
    public void touchOnYes()
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


    public static void TouchOnNo()
    {
        instance.touchOnNo();
    }
    public void touchOnNo()
    {
        videoPlayer.Stop();
        ConfirmMenuScaler.scaleOut();
    }

}
