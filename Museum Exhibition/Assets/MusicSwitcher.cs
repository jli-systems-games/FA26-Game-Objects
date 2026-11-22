using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip lobbyMusic;
    public AudioClip overthinkingMusic;
    public AudioClip sleepingMusic;
    public AudioClip partyMusic;
    public AudioClip kitchenMusic;

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayLobby()
    {
        PlayMusic(lobbyMusic);
    }

    public void PlayOverthinking()
    {
        PlayMusic(overthinkingMusic);
    }

    public void PlaySleeping()
    {
        PlayMusic(sleepingMusic);
    }

    public void PlayParty()
    {
        PlayMusic(partyMusic);
    }

    public void PlayKitchen()
    {
        PlayMusic(kitchenMusic);
    }
}