using UnityEngine;
using System.Collections.Generic;

public class SwapMusic : MonoBehaviour
{
    private static SwapMusic instance;
    public AudioSource bgMusicSource; // This references the AudioSource
    public List<AudioClip> bgMusic; // Holds a list of all the music
    public int currentMusic = 0; // The index of the current music

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This makes sure the music continues playing across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bgMusicSource.clip = bgMusic[currentMusic]; // Set the initial music
        bgMusicSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to switch to the next music
        {
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        currentMusic++; // Move to the next music in the list

        if (currentMusic >= bgMusic.Count)
        {
            currentMusic = 0;
        }

        bgMusicSource.clip = bgMusic[currentMusic];
        bgMusicSource.Play();
    }
    public void PauseMusic() // Pause the music in PetCat scene
    {
        bgMusicSource.Pause();
    }

    public void ResumeMusic()
    {
        bgMusicSource.UnPause();
    }
}
