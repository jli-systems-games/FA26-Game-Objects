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

    

    // private void Jump()
    // {
    //     Rigidbody rig = player.GetComponent<Rigidbody>();
        

    // public void PlaySound()
    // {
    //     if (audioSource != null)
    //     {
    //         audioSource.Play();  // Play the sound using the AudioSource directly
    //     }
    // }

    public void OnTriggerEnter(Collider other)
    { 
       Debug.Log("Ok");

         if (other.gameObject.tag == "Player")
        {
            Debug.Log("Time to jump");
             Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();
             Vector3 jumpForce = other.gameObject.transform.up;
             jumpForce = jumpForce * jumpadSpeed;
             playerRB.AddForce(jumpForce, ForceMode.Impulse);
        }

            
            //player = other.gameObject;
            //Jump();
            //PlaySound();  // Play sound when the player touches the jump pad
    }
    
}
