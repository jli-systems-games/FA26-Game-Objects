using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("UI References")]
    public GameObject crosshair;
    public GameObject interactPrompt;
    public GameObject infoPanel;
    public TMP_Text titleText;
    public TMP_Text bodyText;

    [Header("Audio")]
    public AudioClip openSound;
    public AudioClip closeSound;

    private Camera cam;
    private AudioSource audioSource;
    private Artifact currentTarget;
    private bool isReading = false;

    void Start()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();

        infoPanel.SetActive(false);
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (isReading)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
                ClosePanel();
            return;
        }

        DetectArtifact();

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
            OpenPanel(currentTarget);
    }

    void DetectArtifact()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Artifact found = null;

        // 检测所有层，让墙能挡住射线
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            found = hit.collider.GetComponent<Artifact>();
        }

        if (found != currentTarget)
        {
            if (currentTarget != null)
                currentTarget.SetHighlight(false);

            if (found != null)
                found.SetHighlight(true);

            currentTarget = found;
        }

        interactPrompt.SetActive(currentTarget != null);
    }

    void OpenPanel(Artifact artifact)
    {
        isReading = true;

        titleText.text = artifact.artifactName;
        bodyText.text = artifact.description;

        infoPanel.SetActive(true);
        interactPrompt.SetActive(false);
        crosshair.SetActive(false);

        PlaySound(openSound);

        GetComponent<PlayerController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ClosePanel()
    {
        isReading = false;

        infoPanel.SetActive(false);
        crosshair.SetActive(true);

        PlaySound(closeSound);

        GetComponent<PlayerController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}