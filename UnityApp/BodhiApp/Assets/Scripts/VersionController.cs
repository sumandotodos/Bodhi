using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionController : MonoBehaviour
{
    public static string version = "1.0";
    public static string getVersion()
    {
        return "V " + version;
    }
}
