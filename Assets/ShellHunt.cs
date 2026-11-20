using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ShellHunt : MonoBehaviour
{
    [Header("Drag these objects into the Inspector")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text shellCounter;
    [SerializeField] private GameObject mosasaur;
    [SerializeField] private GameObject descriptionPanel;

    [Header("Collection distance")]
    [SerializeField] private float collectDistance = 4f;

    private const int TotalShells = 8;
    private int shellsCollected;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (mosasaur != null)
            mosasaur.SetActive(false);

        UpdateCounter();
    }

    private void Update()
    {
        if (shellsCollected >= TotalShells)
            return;

        // Prevent collecting while reading a description or paused.
        if (descriptionPanel != null &&
            descriptionPanel.activeInHierarchy)
            return;

        if (Time.timeScale <= 0f)
            return;

        if (playerCamera == null || Mouse.current == null)
            return;

        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        // Aim through the center of the player's view.
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            collectDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore))
            return;

        ShellCollectible shell =
            hit.collider.GetComponentInParent<ShellCollectible>();

        if (shell == null || !shell.Collect())
            return;

        shellsCollected++;

        if (shellsCollected == TotalShells && mosasaur != null)
        {
            mosasaur.SetActive(true);

            AudioSource groan = mosasaur.GetComponent<AudioSource>();

            if (groan != null)
            {
                groan.Play();
            }
            else
            {
                Debug.LogWarning(
                    "No Audio Source found on the mosasaur object."
                );
            }
        }

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        if (shellCounter == null)
            return;

        shellCounter.text = $"Shells: {shellsCollected}/{TotalShells}";

        if (shellsCollected == TotalShells)
            shellCounter.text += "\nMosasaur unlocked!";
    }
}