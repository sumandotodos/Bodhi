using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class ReceiveVideoResponseController : MonoBehaviour
{
    public string TestVideoToDownloadRemotePath = "video/12/n4tn7s0h98mf7llfdupww4njax";
    public Text DebugTecst;
    public VideoPlayer videoPlayer;
    public RawImage cinema;
    public VolareVideoPlayer volareVideoPlayer;

    private void Start()
    {
        LoginConfigurations.init();
        FileDownloadController.GetSingleton().StartFileDownload(TestVideoToDownloadRemotePath,
        (err, res) =>
        {
          Debug.Log("Download done, it seems");
          DebugTecst.text = "Download done, it seems";
          File.WriteAllBytes(Application.temporaryCachePath + "/" + "sss.MP4", res);
            DebugTecst.text = "" + res.Length;
            videoPlayer.url = Application.temporaryCachePath + "/" + "sss.MP4";
            volareVideoPlayer.StartPlaying();
            StartCoroutine(VideoFinishPoll());
          //NativeGallery.Permission permission = NativeGallery.SaveVideoToGallery(res, "Volare", "asdasd.mp4");
          //NativeGallery.GetVideoFromGallery((path) =>
          //{
          //    DebugTecst.text += "\n"+path;
          //    Handheld.PlayFullScreenMovie(path);
          //});
          //Handheld.PlayFullScreenMovie("asdasd.mp4");
        });
        /*FileDownloadController.GetSingleton().GetVideoURL(TestVideoToDownloadRemotePath,
        (err, url) =>
        {
            Debug.Log("<color=blue>Url=" + url + "</video>");
            videoPlayer.url = url;
            videoPlayer.Play();
        });*/
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
