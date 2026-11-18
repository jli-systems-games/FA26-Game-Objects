using UnityEngine;
public class EatItUp : MonoBehaviour
{
    public enum InteractionType
    {
        Eat,
        Animate
    }

    public InteractionType interactionType;

    public GameObject npc;
    public GameObject interactionText;
    public Animator npcAnimator;
    public string animationName;
    public AudioSource soundSource;
    public PlayerController playerController;

    private bool playerTrigger;

    void Start()
    {
        if (interactionText != null)
            interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerTrigger &&
            playerController.enabled &&
            Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (soundSource != null)
            soundSource.Play();

        if (interactionType == InteractionType.Eat)
        {
            playerTrigger = false;

            if (interactionText != null)
                interactionText.SetActive(false);

            Destroy(npc);
        }
        else
        {
            npcAnimator.Play(animationName, 0, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;

            if (interactionText != null)
                interactionText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;

            if (interactionText != null)
                interactionText.SetActive(false);
        }
    }
}