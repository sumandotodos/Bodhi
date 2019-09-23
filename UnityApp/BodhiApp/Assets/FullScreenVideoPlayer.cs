using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenVideoPlayer : MonoBehaviour
{
    public static void PlayFullScreenMovie(string deviceFilepath)
    {
        Handheld.PlayFullScreenMovie("file://" + deviceFilepath);
    }
}
