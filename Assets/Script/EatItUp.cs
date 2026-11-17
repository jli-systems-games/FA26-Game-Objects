using UnityEngine;

public class EatItUp : MonoBehaviour
{
    public GameObject npc;
    public GameObject interactionText;
    public AudioSource eatSound;

    private bool playerTrigger;

    void Start()
    {
        interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerTrigger && Input.GetKeyDown(KeyCode.E))
        {
            eatSound.Play();
            interactionText.SetActive(false);
            Destroy(npc);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;
            interactionText.SetActive(true);
        }
    } // Detects if the player enters the trigger zone.

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
            interactionText.SetActive(false);
        }
    } 
}
    
    // Detects if the player exits the trigger zone.
