using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    public AudioClip[] clips;

    AudioSource aSource;
    // Start is called before the first frame update
    void Start()
    {
        aSource = this.GetComponent<AudioSource>();
    }

    public void PlaySoundFromList()
    {
        if(clips.Length == 0)
        {
            return;
        }

        int r = Random.Range(0, clips.Length);
        aSource.PlayOneShot(clips[r]);
    }
}
