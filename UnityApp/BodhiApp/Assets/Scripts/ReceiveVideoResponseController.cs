using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class ReceiveVideoResponseController : MonoBehaviour
{
    public string TestVideoToDownloadRemotePath = "video/12/n4tn7s0h98mf7llfdupww4njax";
    public VideoPlayer videoPlayer;
    public RawImage cinema;
    public VolareVideoPlayer volareVideoPlayer;
    public Text originalQuestionText;

    public static ReceiveVideoResponseController instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetOriginalQuestion(string questionid, string question)
    {
        if(ContentsManager.IsLocalContent(questionid))
        {
            originalQuestionText.text = ContentsManager.GetSingleton().GetLocalContentFromId(questionid);
        }
        else
        {
            originalQuestionText.text = question;
        }

    }

    public static ReceiveVideoResponseController GetSingleton()
    {
        return instance;
    }

    private void Start()
    {
        LoginConfigurations.init();
        if (TestVideoToDownloadRemotePath != "")
        {
            DownloadAndPlayVideoResponse(TestVideoToDownloadRemotePath);
        }
    }

    public string idFromFullPath(string fullPath)
    {
        string[] fields = fullPath.Split('/');
        return fields[fields.Length - 1];
    }

    public void DownloadAndPlayVideoResponse(string remoteFilepath)
    {
        string id = idFromFullPath(remoteFilepath);
        FileDownloadController.GetSingleton().StartFileDownload(remoteFilepath,
        (err, res) =>
        {
            if (err == null)
            {
                File.WriteAllBytes(Application.temporaryCachePath + "/" + id, res);
                videoPlayer.url = Application.temporaryCachePath + "/" + id;
                volareVideoPlayer.ShowCinema();
                volareVideoPlayer.VideoRawBytes = res;
                volareVideoPlayer.StartPlaying();
                StartCoroutine(VideoFinishPoll());
            }
            else 
            {
                
            }
        });
    }

    public static void DeleteCache()
    {
        string path = Application.temporaryCachePath;

        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

        foreach (System.IO.FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true);
        }
    }


    IEnumerator VideoFinishPoll()
    {

        yield return new WaitForSeconds(0.25f);
        videoPlayer.playOnAwake = true;
        videoPlayer.Play();
        while (videoPlayer.time <= videoPlayer.length)
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("w: " + videoPlayer.width + ", h: " + videoPlayer.height);
            Debug.Log("sw: " + Screen.width + ", sh: " + Screen.height);

        }
        volareVideoPlayer.TouchOnStop();
    }

}
