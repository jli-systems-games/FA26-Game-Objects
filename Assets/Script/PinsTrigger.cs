using UnityEngine;
using System.Collections;

public class PinsTrigger : MonoBehaviour
{
    // The pin will disappear after this delay when hit by the player.
    public float disappearDelay = 3f;

// Detects collision with the player
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;

        if (otherRb != null &&
            otherRb.CompareTag("Player"))
        {
            if (GameSetting.Instance != null)
            {
                GameSetting.Instance.HitPin();
            }

            StartCoroutine(HideAfterDelay());

            enabled = false;
        }
    }

// Hides the pin after delay
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(disappearDelay);

        gameObject.SetActive(false);
    }
}