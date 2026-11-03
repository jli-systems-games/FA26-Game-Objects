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

    private Camera cam;
    private Artifact currentTarget;
    private bool isReading = false;

    void Start()
    {
        cam = Camera.main;
        infoPanel.SetActive(false);
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (isReading)
        {
            // 阅读状态下，只检查关闭
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

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            Artifact artifact = hit.collider.GetComponent<Artifact>();
            if (artifact != null)
            {
                currentTarget = artifact;
                interactPrompt.SetActive(true);
                return;
            }
        }

        currentTarget = null;
        interactPrompt.SetActive(false);
    }

    void OpenPanel(Artifact artifact)
    {
        isReading = true;

        titleText.text = artifact.artifactName;
        bodyText.text = artifact.description;

        infoPanel.SetActive(true);
        interactPrompt.SetActive(false);
        crosshair.SetActive(false);

        // 暂停移动、解锁鼠标
        GetComponent<PlayerController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void ClosePanel()
    {
        isReading = false;

        infoPanel.SetActive(false);
        crosshair.SetActive(true);

        GetComponent<PlayerController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}