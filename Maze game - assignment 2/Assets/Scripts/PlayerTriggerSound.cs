using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerSound : MonoBehaviour
{

    public AudioClip SoundToPlay ;
    //volume
    //public float Volume;
    public  AudioSource audio;
    public bool alreadyPlayed = false;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter()
    {
        if (!alreadyPlayed)
        {
            //audio.PlayOneShot(SoundToPlay, Volume);
            audio.PlayOneShot(SoundToPlay);
            alreadyPlayed = true;
        }
    }
}
