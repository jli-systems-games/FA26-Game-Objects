using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Jumpad : MonoBehaviour
{
    public float jumpadSpeed;
    GameObject player;
    public AudioSource audioSource; // The AudioSource component

    private void Start()
    {
        // Ensure the AudioSource component is on the same GameObject and fetch it
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            Jump();
            PlaySound();  // Play sound when the player touches the jump pad
        }
    }

    private void Jump()
    {
        Rigidbody rig = player.GetComponent<Rigidbody>();
        rig.AddForce(player.transform.up * jumpadSpeed, ForceMode.Impulse);
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();  // Play the sound using the AudioSource directly
        }
    }

}
