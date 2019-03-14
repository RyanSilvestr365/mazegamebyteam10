using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//can be played multiple times
public class triggeredAudio : MonoBehaviour
{
     AudioSource aud;
    public AudioClip myClip;
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    void OnTriggerEnter()
    {
        aud.PlayOneShot(myClip);
    }
}
