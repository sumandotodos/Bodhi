using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{

    public static SoundController instance;
    public AudioSource aSource;

    public static void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            instance.aSource.PlayOneShot(clip);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        aSource = this.GetComponent<AudioSource>();
    }


}
