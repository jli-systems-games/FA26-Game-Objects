using UnityEngine;
using System.Collections.Generic;

public class SwapMusic : MonoBehaviour
{
    public AudioSource bgMusicSource;//This references the audiosource we want to use.

    public List<AudioClip> bgMusic;//Holds a list of all the music we want to play!
    
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
        bgMusicSource.clip = bgMusic[0]; //Step 1: Replace current music clip with new one.   
        bgMusicSource.Play();//Step 2: Tell the audiosource to play the clip.
    }
}
