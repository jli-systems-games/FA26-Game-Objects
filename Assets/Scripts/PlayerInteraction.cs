using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform playerCamera;
    public float interactDistance = 5f;
    public DialogueManager dialogueManager;
    public GameObject interactPrompt;

    SphereDialogue targetedSphere;

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

        targetedSphere = null;

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            targetedSphere = hit.collider.GetComponent<SphereDialogue>();
        }

        interactPrompt.SetActive(targetedSphere != null);

        if (targetedSphere != null && Input.GetKeyDown(KeyCode.E))
        {
            interactPrompt.SetActive(false);
            dialogueManager.StartDialogue(targetedSphere.lines);
        }
    }
}