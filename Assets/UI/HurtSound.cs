using UnityEngine;

public class HurtSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hurtClip;

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtClip != null)
        {
            audioSource.PlayOneShot(hurtClip);
        }
    }
}