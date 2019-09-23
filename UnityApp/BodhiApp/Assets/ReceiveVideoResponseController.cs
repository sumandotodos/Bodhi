using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveVideoResponseController : MonoBehaviour
{
    public string TestVideoToDownloadRemotePath = "video/12/i9q3jqzeyohzr43cegxr2wbnyp";

    private void Start()
    {
        LoginConfigurations.init();
        FileDownloadController.GetSingleton().StartFileDownload(TestVideoToDownloadRemotePath,
        (err, res) =>
        {
            Debug.Log("Download done, it seems");
        });
    }

}
