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

    int BytesToTransfer;
    int TransferredBytes;

    /*IEnumerator Start()
    {
        SetUpTransfer(1168000);
        yield return new WaitForSeconds(80.0f);
        SetTransferredBytes(6000);
        yield return new WaitForSeconds(3.0f);
        SetTransferredBytes(16000);
        yield return new WaitForSeconds(3.0f);
        SetTransferredBytes(80000);
        yield return new WaitForSeconds(3.0f);
        SetTransferredBytes(150000);
        yield return new WaitForSeconds(3.0f);
        SetTransferredBytes(450000);
        yield return new WaitForSeconds(3.0f);
        SetTransferredBytes(850000);
        SetTransferredBytes(1168000);
        yield return new WaitForSeconds(3.0f);
    }*/

    private static int BytesToKBytes(int bytes)
    {
        return bytes / 1024;
    }

    private float GetTransferFraction()
    {
        if(BytesToTransfer == TransferredBytes)
        {
            return 1.0f;
        }
        float BYTES = (float)BytesToTransfer;
        float bytes = (float)TransferredBytes;
        return bytes / BYTES;
    }

    public void SetUpTransfer(int NewBytesToTransfer)
    {
        SetBarFraction(0.0f);

        BytesToTransfer = NewBytesToTransfer;
        TransferredBytes = 0;
        UpdateProgress();
    }

    private void SetBarFraction(float fraction)
    {
        Vector2 size = ProgressBarTransform.sizeDelta;
        size.x = MinProgressWidth + (MaxProgressWifth-MinProgressWidth) * fraction;
        ProgressBarTransform.sizeDelta = size;
    }

    public void SetTransferredBytes(int bytes)
    {
        TransferredBytes = bytes;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        float fraction = GetTransferFraction();

        ProgressText.text = BytesToKBytes(TransferredBytes) + "/" + BytesToKBytes(BytesToTransfer) + " kBytes / (" +
            Mathf.RoundToInt(fraction * 100.0f) + "%)";

        SetBarFraction(fraction);
    }
}
