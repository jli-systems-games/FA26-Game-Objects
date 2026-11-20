using UnityEngine;

public class soundEffectsScript : MonoBehaviour
{
   public AudioSource source;
    characterScript player;

    void Start()
    {
        player = GetComponent<characterScript>();
    }

    void Update()
    {
        bool pressing = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        bool shouldPlay = player.enabled && pressing && player.IsGrounded();

        if (shouldPlay && !source.isPlaying) source.Play();
        if (!shouldPlay && source.isPlaying) source.Stop();
    }
}