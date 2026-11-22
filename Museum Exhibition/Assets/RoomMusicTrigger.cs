using UnityEngine;

public class RoomMusicTrigger : MonoBehaviour
{
    public MusicSwitcher musicSwitcher;
    public AudioClip roomMusic;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            musicSwitcher.PlayMusic(roomMusic);
        }
    }
}