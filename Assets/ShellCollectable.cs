using UnityEngine;

public class ShellCollectible : MonoBehaviour
{
    [SerializeField] private AudioSource pickupAudioSource;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField, Range(0f, 1f)]
    private float pickupVolume = 0.7f;

    private bool collected;

    public bool Collect()
    {
        if (collected)
            return false;

        collected = true;

        if (pickupAudioSource != null && pickupSound != null)
        {
            pickupAudioSource.PlayOneShot(
                pickupSound, pickupVolume);
        }

        gameObject.SetActive(false);
        return true;
    }
}