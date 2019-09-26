using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string ColonSeparatedToSlashSeparated(string path)
    {
        return path.Replace(":", "/");
    }

    public static string SlashSeparatedToColonSeparated(string path)
    {
        return path.Replace("/", ":");
    }
}
