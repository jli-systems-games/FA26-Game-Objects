using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string words;

    public GameObject interactionText;

    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public Animator cameraAnimator;
    public string cameraAnimation;

    public PlayerController playerController;

    public float dialogueDelay = 1f;

    public KeyCode interactKey = KeyCode.E;
    public KeyCode exitKey = KeyCode.Q;

    private bool playerTrigger;
    private bool isTalking;

    void Start()
    {
        dialogueBox.SetActive(false);
        interactionText.SetActive(false);
        switchCamera.SetActive(false);
    }

    void Update()
    {
        if (isTalking)
        {
            if (Input.GetKeyDown(exitKey) ||
                (dialogueBox.activeSelf &&
                Input.GetKeyDown(interactKey)))
            {
                ConEnd();
            }
        }
        else if (playerTrigger &&
                 mainCameraRef.activeInHierarchy &&
                 playerController.enabled &&
                 Input.GetKeyDown(interactKey))
        {
            StartCoroutine(Talk());
        }
    }

    IEnumerator Talk()
    {
        isTalking = true;
        interactionText.SetActive(false);

        playerController.enabled = false;

        mainCameraRef.SetActive(false);
        switchCamera.SetActive(true);

        cameraAnimator.Play(cameraAnimation, 0, 0f);

        yield return new WaitForSeconds(dialogueDelay);

        dialogueText.text = words;
        dialogueBox.SetActive(true);
    }

    void ConEnd()
    {
        StopAllCoroutines();

        dialogueBox.SetActive(false);

        switchCamera.SetActive(false);
        mainCameraRef.SetActive(true);

        playerController.enabled = true;
        isTalking = false;

        interactionText.SetActive(playerTrigger);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = true;

            if (mainCameraRef.activeInHierarchy &&
                playerController.enabled)
            {
                interactionText.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTrigger = false;
            interactionText.SetActive(false);

            if (isTalking)
            {
                ConEnd();
            }
        }
    }
}