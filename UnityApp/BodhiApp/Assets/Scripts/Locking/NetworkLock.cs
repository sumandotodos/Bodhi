using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLock : MonoBehaviour
{
    static bool Locked = false;

    public static bool Attempt()
    {
        bool result = !Locked;
        Locked = true;
        return result;
    }

    public static void Release()
    {
        Locked = false;
    }
}
