using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;

public class ReceiveVideoResponseController : MonoBehaviour
{
    public string TestVideoToDownloadRemotePath = "video/12/i9q3jqzeyohzr43cegxr2wbnyp";
    public Text DebugTecst;
    public VideoPlayer videoPlayer;
    public RawImage cinema;

    private void Start()
    {
        cinema.enabled = false;
        LoginConfigurations.init();
        FileDownloadController.GetSingleton().StartFileDownload(TestVideoToDownloadRemotePath,
        (err, res) =>
        {
          Debug.Log("Download done, it seems");
          DebugTecst.text = "Download done, it seems";
          File.WriteAllBytes(Application.temporaryCachePath + "/" + "sss.MP4", res);
            DebugTecst.text = "" + res.Length;
            videoPlayer.url = Application.temporaryCachePath + "/" + "sss.MP4";
            cinema.enabled = true;
            videoPlayer.Play();
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

}
