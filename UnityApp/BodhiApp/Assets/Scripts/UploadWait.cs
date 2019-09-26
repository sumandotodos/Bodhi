using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadWait : MonoBehaviour
{
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public ProgressBar GetProgressBar()
    {
        return GetComponentInChildren<ProgressBar>();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void AutoHide(float time)
    {
        System.Timers.Timer aTimer = new System.Timers.Timer(Mathf.RoundToInt(time * 1000.0f));
        aTimer.Elapsed += (source, e) =>
        {
            this.gameObject.SetActive(false);
        };
        aTimer.Enabled = true;
        aTimer.AutoReset = false;

    }

}
