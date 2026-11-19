using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NoteCard : MonoBehaviour
{
    [Header("Optional UI panels")]
    public GameObject noteContentUI;
    public GameObject promptUI;

    [Header("Interaction")]
    public Transform player;
    public float interactionDistance = 2.2f;
    public string prompt = "Press E to read";
    public string title = "Exhibition Note";
    [TextArea(3, 10)] public string description = "Add the exhibit introduction here.";

    private bool playerInRange;
    private bool isShowing;
    private Text[] contentLabels;
    private Text promptLabel;

    private void Awake()
    {
        ResolvePlayer();
        if (noteContentUI != null)
        {
            contentLabels = noteContentUI.GetComponentsInChildren<Text>(true);
            noteContentUI.SetActive(false);
        }
        if (promptUI != null)
        {
            var labels = promptUI.GetComponentsInChildren<Text>(true);
            if (labels.Length > 0) promptLabel = labels[0];
            promptUI.SetActive(false);
        }
        UpdateAssignedText();
    }

    private void Update()
    {
        if (player == null) ResolvePlayer();
        playerInRange = player != null && Vector3.Distance(player.position, transform.position) <= interactionDistance;

        if (!playerInRange && isShowing)
            SetShowing(false);

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame && playerInRange)
            SetShowing(!isShowing);

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && isShowing)
            SetShowing(false);

        if (promptUI != null) promptUI.SetActive(playerInRange && !isShowing);
        if (noteContentUI != null) noteContentUI.SetActive(isShowing);
    }

    private void ResolvePlayer()
    {
        if (player != null) return;
        var mainCamera = UnityEngine.Camera.main;
        if (mainCamera != null) player = mainCamera.transform;
        else
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) player = playerObject.transform;
        }
    }

    private void SetShowing(bool show)
    {
        isShowing = show;
        if (noteContentUI != null) noteContentUI.SetActive(show);
        if (promptUI != null) promptUI.SetActive(playerInRange && !show);
        UpdateAssignedText();
    }

    private void UpdateAssignedText()
    {
        if (promptLabel != null) promptLabel.text = prompt;
        if (contentLabels == null || contentLabels.Length == 0) return;
        if (contentLabels.Length == 1) contentLabels[0].text = title + "\n\n" + description;
        else
        {
            contentLabels[0].text = title;
            contentLabels[1].text = description;
        }
    }

    private void OnGUI()
    {
        if (playerInRange && !isShowing && promptUI == null)
        {
            var promptStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
            GUI.Box(new Rect(Screen.width * .5f - 135, Screen.height - 112, 270, 52), prompt, promptStyle);
        }

        if (isShowing && noteContentUI == null)
        {
            float width = Mathf.Min(680, Screen.width - 48);
            float height = Mathf.Min(420, Screen.height - 48);
            var panel = new Rect((Screen.width - width) * .5f, (Screen.height - height) * .5f, width, height);
            GUI.Box(panel, GUIContent.none);
            var headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
            var bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                alignment = TextAnchor.UpperLeft,
                wordWrap = true
            };
            var hintStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                alignment = TextAnchor.MiddleCenter
            };
            GUI.Label(new Rect(panel.x + 28, panel.y + 22, panel.width - 56, 58), title, headerStyle);
            GUI.Label(new Rect(panel.x + 36, panel.y + 96, panel.width - 72, panel.height - 154), description, bodyStyle);
            GUI.Label(new Rect(panel.x + 20, panel.yMax - 48, panel.width - 40, 28), "Press E or Esc to close", hintStyle);
        }
    }
}
