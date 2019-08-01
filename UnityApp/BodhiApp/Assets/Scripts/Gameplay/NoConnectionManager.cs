using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoConnectionManager : MonoBehaviour
{
    public static NoConnectionManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static NoConnectionManager GetSingleton()
    {
        return instance;
    }

    public void SetNoConnection(bool NewNoConnectionState)
    {

    }

}
