using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform playerCamera;
    public float interactDistance = 5f;
    public DialogueManager dialogueManager;
    public GameObject interactPrompt;

    bool sphereIsTargeted;

    void Start()
    {
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (dialogueManager.IsDialogueOpen)
        {
            interactPrompt.SetActive(false);
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        sphereIsTargeted = false;

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                sphereIsTargeted = true;
            }
        }

        interactPrompt.SetActive(sphereIsTargeted);

        if (sphereIsTargeted && Input.GetKeyDown(KeyCode.E))
        {
            interactPrompt.SetActive(false);
            dialogueManager.StartDialogue();
        }
    }
}