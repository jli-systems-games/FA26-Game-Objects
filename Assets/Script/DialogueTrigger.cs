using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string words;

    public GameObject interactionText;
    public Animator cameraAnimator;
    public Transform playerCamera;
    public Transform npc;

    public PlayerController playerController;
    public CameraController cameraController;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Q;

    private bool playerTrigger;
    private bool isTalking;
    private Quaternion CamRotation;

    void Start()
    {
        dialogueBox.SetActive(false);
        interactionText.SetActive(false);
    }

    void Update()
    {
        if (playerTrigger && !isTalking &&
            Input.GetKeyDown(interactKey))
        {
            StartCoroutine(Talk());
        }
    }

    void LateUpdate()
    {
        if (isTalking)
        {
            playerCamera.LookAt(npc);
        }
    }

    IEnumerator Talk()
    {
        CamRotation = playerCamera.localRotation;

        isTalking = true;
        interactionText.SetActive(false);

        playerController.enabled = false;
        cameraController.enabled = false;

        playerCamera.LookAt(npc);
        cameraAnimator.Play("Base Layer.DiaCamera", 0, 0f);

        yield return new WaitForSeconds(1f);

        if (playerTrigger)
        {
            dialogueText.text = words;
            dialogueBox.SetActive(true);

            yield return new WaitUntil(() =>
                Input.GetKeyDown(interactKey) ||
                Input.GetKeyDown(exitKey) ||
                !playerTrigger);
        }

        dialogueBox.SetActive(false);
        cameraAnimator.Play("Base Layer.Idle", 0, 0f);

        isTalking = false;
        playerCamera.localRotation = CamRotation;

        playerController.enabled = true;
        cameraController.enabled = true;

        interactionText.SetActive(playerTrigger);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTrigger = true;
            interactionText.SetActive(!isTalking);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTrigger = false;
            interactionText.SetActive(false);
            dialogueBox.SetActive(false);
        }
    }
}
