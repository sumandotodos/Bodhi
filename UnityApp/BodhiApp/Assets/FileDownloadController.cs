using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDownloadController : MonoBehaviour
{
    public UploadWait uploadWait;

    public static FileDownloadController instance;

    private void Awake()
    {
        instance = this;
    }

    public static FileDownloadController GetSingleton()
    {
        return instance;
    }

    public void GetVideoURL(string path, System.Action<string, string> callback = null)
    {
        StartCoroutine(GetVideoURLCoroutine(path, callback));
    }

    IEnumerator GetVideoURLCoroutine(string filepath, System.Action<string, string> callback = null)
    {
        string downloadUrl = "";
        yield return API.GetSingleton().GetDownloadUrl(PlayerPrefs.GetString("UserId"), filepath, (err, url) =>
        {
            Debug.Log("Download url: " + url);
            downloadUrl = url;
        });
        if(callback!=null)
        {
            callback(null, downloadUrl);
        }
    }

    public void StartFileDownload(string path, System.Action<string, byte[]> callback = null)
    {
        StartCoroutine(DownloadVideoCoroutine(path, callback));
    }

    IEnumerator DownloadVideoCoroutine(string filepath, System.Action<string, byte[]> callback = null)
    {
        uploadWait.Show();
        uploadWait.GetProgressBar().SetUpTransfer(0);
        byte[] downloadedBytes = null;
        string downloadUrl = "";
        yield return API.GetSingleton().GetDownloadUrl(PlayerPrefs.GetString("UserId"), filepath, (err, url) =>
        {
            Debug.Log("Download url: " + url);
            downloadUrl = url;
        });

        yield return REST.GetSingleton().GET_Binary(downloadUrl, uploadWait.GetProgressBar().UpdateProgress,
            (err, allTheBytes) => {
                downloadedBytes = allTheBytes;
            Debug.Log("Bytes downloaded: " + allTheBytes.Length);
        });

       
        yield return new WaitForSeconds(1.0f);
        uploadWait.Hide();

        callback(null, downloadedBytes);
      
        /*if (callback != null)
        {
            callback("success", storedFileId);
        }*/
    }

}
