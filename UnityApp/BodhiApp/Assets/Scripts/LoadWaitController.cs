using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWaitController : MonoBehaviour
{
    public float Timeout = 3.0f;
    float remain = 3.0f;
    public GameObject SpinningThing;

    private void Awake()
    {
        SpinningThing.SetActive(false);
    }

    public void StartNetworkTransfer()
    {
        remain = Timeout;
    }

    public void CompleteNetworkTransfer()
    {
        SpinningThing.SetActive(false);
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
    }
}
