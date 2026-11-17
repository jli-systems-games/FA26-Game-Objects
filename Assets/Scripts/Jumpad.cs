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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            Jump();
            PlaySound();  // Play sound when the player touches the jump pad
        }
    }

    private void Jump()
    {
        var playerController = player.GetComponentInParent<PlayerMovement>();
        playerController.moveDirection.y = jumpadSpeed;
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();  // Play the sound using the AudioSource directly
        }
    }

}
