using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWaitController : MonoBehaviour
{
    public float Timeout = 3.0f;
    float remain = -1.0f;
    float remain2 = -1.0f;
    public GameObject SpinningThing;
    public UIOpacityWiggle TextThing;

    private void Awake()
    {
        remain = -1;
        remain2 = -1;
        SpinningThing.SetActive(false);
        SetTextEnabled(false);
    }

    private void SetTextEnabled(bool en)
    {
        if (TextThing != null)
        {
            TextThing.reset();
            TextThing.isActive = en;
            TextThing.gameObject.SetActive(en);
        }
    }

    public void StartNetworkTransfer()
    {
        remain = Timeout;
        remain2 = Timeout * 1.5f;
    }

    public void CompleteNetworkTransfer()
    {
        remain = -1.0f;
        remain2 = -1.0f;
        SpinningThing.SetActive(false);
        SetTextEnabled(false);
    }

    public void Force()
    {
        remain = 0.05f;
    }

    private void Update()
    {
        if(remain > 0.0f)
        {
            remain -= Time.deltaTime;
            if(remain <= 0.0)
            {
                SpinningThing.SetActive(true);
            }
        }

        if(remain2 > 0.0F)
        {
            remain2 -= Time.deltaTime;
            if (remain2 <= 0.0)
            {
                SetTextEnabled(true);
            }
        }
    }
}
