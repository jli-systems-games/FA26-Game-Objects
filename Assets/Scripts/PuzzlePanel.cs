using TMPro;
using UnityEngine;

public class PuzzlePanel : MonoBehaviour
{
    [Header("UI 引用")]
    public GameObject puzzleCanvas;
    public TMP_Text promptText;

    [Header("交互设置")]
    public float interactRange = 3f;
    public KeyCode openKey = KeyCode.F;
    public KeyCode closeKey = KeyCode.Escape;

    private bool puzzleIsOpen;
    private bool puzzleSolved;
    private GameObject player;
    private characterlook playerLook;
    private bool playerLookWasEnabled;
    private CursorLockMode previousCursorLockState;
    private bool previousCursorVisible;

    public bool IsPuzzleOpen => puzzleIsOpen;

    private void Start()
    {
        FindPlayer();

        if (promptText != null)
        {
            Canvas promptCanvas = promptText.GetComponentInParent<Canvas>(true);
            if (promptCanvas != null)
                promptCanvas.gameObject.SetActive(true);
        }

        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (puzzleSolved)
            return;

        if (player == null)
            FindPlayer();

        if (player == null)
            return;

        bool playerInRange = Vector3.Distance(transform.position, player.transform.position) <= interactRange;

        if (puzzleIsOpen)
        {
            if (Input.GetKeyDown(closeKey))
                ClosePuzzleUI();
            return;
        }

        if (promptText != null)
        {
            promptText.gameObject.SetActive(playerInRange);
            if (playerInRange)
                promptText.text = "Press F to open";
        }

        if (playerInRange && Input.GetKeyDown(openKey))
            OpenPuzzleUI();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerLook = player.GetComponent<characterlook>();
            if (playerLook == null)
                playerLook = player.GetComponentInChildren<characterlook>();
        }
    }

    public void OpenPuzzleUI()
    {
        if (puzzleSolved || puzzleCanvas == null)
            return;

        previousCursorLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;
        if (playerLook != null)
        {
            playerLookWasEnabled = playerLook.enabled;
            playerLook.enabled = false;
        }

        puzzleIsOpen = true;
        puzzleCanvas.SetActive(true);

        if (promptText != null)
            promptText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePuzzleUI()
    {
        puzzleIsOpen = false;
        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);

        if (playerLook != null && playerLookWasEnabled)
            playerLook.enabled = true;

        Cursor.lockState = previousCursorLockState;
        Cursor.visible = previousCursorVisible;
    }

    public void OnPuzzleCompleted()
    {
        if (puzzleSolved)
            return;

        puzzleSolved = true;
        puzzleIsOpen = false;

        if (playerLook != null)
            playerLook.enabled = false;

        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);
        if (promptText != null)
            promptText.gameObject.SetActive(false);

        ShowVictoryCanvas();
        Debug.Log("Puzzle solved!");
    }

    public void PuzzleComplete()
    {
        OnPuzzleCompleted();
    }

    private void ShowVictoryCanvas()
    {
        GameUIManager manager = GameUIManager.Instance;
        if (manager == null)
            manager = FindObjectOfType<GameUIManager>();

        if (manager == null)
        {
            Debug.LogError("Puzzle solved, but no GameUIManager is present in the active scene.");
            return;
        }

        manager.WinGame();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
