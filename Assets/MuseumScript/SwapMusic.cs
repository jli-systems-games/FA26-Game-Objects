using UnityEngine;
using UnityEngine.Video;
using System.Collections.Generic;

public class SwapMusic : MonoBehaviour
{
    public AudioSource bgMusicSource;
    public List<AudioClip> bgMusic;
    public int currentMusic = 0;

    public AudioSource[] otherAudioSources;
    public VideoPlayer[] videoPlayers;

    private bool videoPaused;
    private bool musicPaused;

    void Start()
    {
        if (bgMusicSource == null || bgMusic.Count == 0)
            return;

        currentMusic = Mathf.Clamp(currentMusic, 0, bgMusic.Count - 1);

        bgMusicSource.clip = bgMusic[currentMusic];
        bgMusicSource.Play();
    }

    void Update()
    {
        if (bgMusicSource == null || bgMusic.Count == 0)
            return;

        UpdateMusicState();

        if (Input.GetKeyDown(KeyCode.M) && !musicPaused)
            PlayMusic();
    }

    void UpdateMusicState()
    {
        bool otherAudioPlaying = videoPaused;

        foreach (AudioSource source in otherAudioSources)
        {
            if (source != null &&
                source != bgMusicSource &&
                source.isPlaying)
            {
                otherAudioPlaying = true;
                break;
            }
        }

        foreach (VideoPlayer video in videoPlayers)
        {
            if (video != null && video.isPlaying)
            {
                otherAudioPlaying = true;
                break;
            }
        }

        if (otherAudioPlaying && !musicPaused)
        {
            bgMusicSource.Pause();
            musicPaused = true;
        }
        else if (!otherAudioPlaying && musicPaused)
        {
            bgMusicSource.UnPause();
            musicPaused = false;
        }
    }

    public void PlayMusic()
    {
        if (bgMusicSource == null || bgMusic.Count == 0 || musicPaused)
            return;

        currentMusic = (currentMusic + 1) % bgMusic.Count;

        bgMusicSource.clip = bgMusic[currentMusic];
        bgMusicSource.Play();
    }

    public void PauseMusic()
    {
        videoPaused = true;
        UpdateMusicState();
    }

    public void ResumeMusic()
    {
        videoPaused = false;
        UpdateMusicState();
    }
}