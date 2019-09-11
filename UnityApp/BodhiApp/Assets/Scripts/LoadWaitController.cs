using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWaitController : MonoBehaviour
{
    public GameObject SpinningThing;

    private void Awake()
    {
        SpinningThing.SetActive(false);
    }

    public void StartNetworkTransfer()
    {
        SpinningThing.SetActive(true);
    }

    public void CompleteNetworkTransfer()
    {
        SpinningThing.SetActive(false);
    }
}
