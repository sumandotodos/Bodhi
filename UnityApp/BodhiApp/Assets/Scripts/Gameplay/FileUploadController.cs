using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileUploadController : MonoBehaviour
{

    public UploadWait uploadWait;

    public static FileUploadController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        uploadWait.Hide();
    }

    public static FileUploadController GetSingleton()
    {
        return instance;
    }

    public void StartFileUpload(string path, System.Action<string, string> callback = null)
    {
        StartCoroutine(UploadVideoCoroutine(path, callback));
    }

    IEnumerator UploadVideoCoroutine(string filepath, System.Action<string, string> callback = null)
    {
        byte[] allBytes = System.IO.File.ReadAllBytes(filepath);
        uploadWait.Show();
        uploadWait.GetProgressBar().SetUpTransfer((uint)allBytes.Length);
        string uploadUrl = "";
        string storedFileId = "";
        yield return API.GetSingleton().GetUploadUrl(PlayerPrefs.GetString("UserId"), (err, urlObj) =>
        {
            Debug.Log("<color=orange>Upload URL = " + urlObj.url + "</color>");
            Debug.Log("<color=purple>Upload id = " + urlObj.id + "</color>");
            uploadUrl = urlObj.url;
            storedFileId = urlObj.id;
        });
        yield return REST.GetSingleton().PUT(uploadUrl, allBytes, uploadWait.GetProgressBar().UpdateProgress,
            (err, response) =>
            {
                Debug.Log("This was the err: " + err + ", and this was the response: " + response);
                if (callback != null)
                {
                    callback("error", "");
                }
            });
        yield return new WaitForSeconds(1.0f);
        uploadWait.Hide();
        MessagesController.GetSingleton().ShowMessage("Tu vídeo-respuesta se ha subido correctamente", Color.white);
        if(callback!=null)
        {
            callback("success", storedFileId);
        }
    }


}
