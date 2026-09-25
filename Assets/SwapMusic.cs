using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class SwapMusic : MonoBehaviour
{
    public AudioSource bgMusicSource;
    public List<AudioClip> bgMusic;
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
        bgMusicSource.clip = bgMusic[0]; // Step 1: Replace current music
        bgMusicSource.Play(); // Step 2:

    }
}
