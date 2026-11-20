using UnityEngine;

public class EatItUp : MonoBehaviour
{
    public enum InteractionType
    {
        Eat,
        Fun
    }

    public InteractionType interactionType;

    public GameObject npc;
    public Animator npcAnimator;
    public string animationName;
    public AudioSource soundSource;
    public PlayerController playerController;
    public TodoList todoList;
    public HintUI hintUI;

    private bool playerTrigger;

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

            if (hintUI != null)
                hintUI.LeaveRange(this);

            if (todoList != null)
                todoList.MarkEaten(this);

            Destroy(npc);
        }
        else if (npcAnimator != null && !string.IsNullOrEmpty(animationName))
        {
            npcAnimator.Play(animationName, 0, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;

            if (hintUI != null)
                hintUI.EnterRange(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;

            if (hintUI != null)
                hintUI.LeaveRange(this);
        }
    }
}