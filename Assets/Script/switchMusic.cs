using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class switchMusic : MonoBehaviour
{
    public AudioSource bgMusicSource; //this references the audio source we want to use
    public List<AudioClip> bgMusic; //holds a list of audio we want to play

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgMusicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        bgMusicSource.clip = bgMusic[0]; //step1: replace current music clip with new one
        bgMusicSource.Play(); //step2: tlwl the audio source to play music 
    }
}
