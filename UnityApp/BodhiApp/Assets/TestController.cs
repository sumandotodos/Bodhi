using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class TestController : MonoBehaviour
{

    public Text debugText;

    public void TouchOnGallery()
    {
        NativeGallery.GetVideoFromGallery((path) =>
        {
            FileInfo finfo = new FileInfo(path);
            debugText.text = "Gallery file: " + path + ", length: " + finfo.Length;
        });
    }

    public void TouchOnCamera()
    {
        debugText.text = "going...";
        var cam = NativeCamera.DeviceHasCamera();
        var busy = NativeCamera.IsCameraBusy();
        var success = NativeCamera.RecordVideo((path) =>
        {
            FileInfo finfo = new FileInfo(path);
            debugText.text = "Camera file: " + path + ", length: " + finfo.Length;
        }, NativeCamera.Quality.Default, 60, 200000000);
        debugText.text = "op: " + success + " " + cam + " " + busy;

    }
}
