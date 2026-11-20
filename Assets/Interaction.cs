using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MuseumInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerMovement playerMovement;

    public GameObject descriptionPanel;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public float interactionDistance = 6f;

    private bool descriptionOpen;
    private bool movementWasEnabled;
    private int lastClosedFrame = -1;

    void Start()
    {
        descriptionPanel.SetActive(false);
    }

    void LateUpdate()
    {
        if (descriptionOpen)
        {
            if (Keyboard.current != null &&
                Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CloseDescription();
            }

            return;
        }

        // Prevent the Close click from opening another exhibit.
        if (Time.frameCount == lastClosedFrame)
            return;

        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        // Check the object at the center of the player's view.
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
        {
            ExhibitInfo exhibit =
                hit.collider.GetComponentInParent<ExhibitInfo>();

            if (exhibit != null)
            {
                OpenDescription(exhibit);
            }
        }
    }

    void OpenDescription(ExhibitInfo exhibit)
    {
        titleText.text = exhibit.exhibitTitle;
        descriptionText.text = exhibit.description;

        descriptionOpen = true;

        movementWasEnabled = playerMovement.enabled;
        playerMovement.enabled = false;

        descriptionPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseDescription()
    {
        if (!descriptionOpen)
            return;

        descriptionPanel.SetActive(false);
        descriptionOpen = false;
        lastClosedFrame = Time.frameCount;

        playerMovement.enabled = movementWasEnabled;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}