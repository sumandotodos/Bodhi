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

    public void SetOriginalQuestion(string question)
    {
        originalQuestionText.text = question;
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
                File.WriteAllBytes(Application.temporaryCachePath + "/" + id + ".MP4", res);
                videoPlayer.url = Application.temporaryCachePath + "/" + id + ".MP4";
                volareVideoPlayer.ShowCinema();
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
        yield return new WaitForSeconds(1.0f);
        while(videoPlayer.time < videoPlayer.length)
        {
            yield return new WaitForSeconds(1.0f);
        }
        volareVideoPlayer.TouchOnStop();
    }

}
