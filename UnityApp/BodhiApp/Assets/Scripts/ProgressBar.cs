using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public RectTransform ProgressBarTransform;
    public float MinProgressWidth;
    public float MaxProgressWifth;
    public Text ProgressText;

    uint BytesToTransfer;
    uint TransferredBytes;

    private static uint BytesToKBytes(uint bytes)
    {
        return bytes / 1024;
    }

    private float GetTransferFraction()
    {
        if(BytesToTransfer == TransferredBytes)
        {
            return 1.0f;
        }
        if (BytesToTransfer == 0)
        {
            return 0.0f;
        }
        else
        {
            float BYTES = (float)BytesToTransfer;
            float bytes = (float)TransferredBytes;
            return bytes / BYTES;
        }
    }

    public void SetUpTransfer(uint NewBytesToTransfer)
    {
        SetBarFraction(0.0f);

        BytesToTransfer = NewBytesToTransfer;
        TransferredBytes = 0;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        float fraction = GetTransferFraction();

        uint kbytes = BytesToKBytes(BytesToTransfer);
        ProgressText.text = BytesToKBytes(TransferredBytes) + "/" + kbytes + " kBytes / (" +
            Mathf.RoundToInt(fraction * 100.0f) + "%)";
        ProgressText.enabled = (kbytes > 0);

        SetBarFraction(fraction);
    }

    public void UpdateProgress(uint length, float fraction)
    {
        Debug.Log("Progress updated: " + length + ", " + fraction);
        if(length > 0)
        {
            BytesToTransfer = length;
            uint transferred = (uint)(((float)length) * fraction);
            SetTransferredBytes(transferred);
        }
        else
        {
            uint transferred = (uint)(((float)BytesToTransfer) * fraction);
            SetTransferredBytes(transferred);
        }
        SetBarFraction(fraction);
    }

    private void SetBarFraction(float fraction)
    {
        Vector2 size = ProgressBarTransform.sizeDelta;
        size.x = MinProgressWidth + (MaxProgressWifth-MinProgressWidth) * fraction;
        ProgressBarTransform.sizeDelta = size;
    }

    public void SetTransferredBytes(uint bytes)
    {
        TransferredBytes = bytes;
        UpdateProgress();
    }


}
