using UnityEngine;

public class PuzzlePanel : MonoBehaviour
{
    public static PuzzlePanel Instance;
    [Header("拼图UI面板（Canvas）")]
    public GameObject puzzleUI;
    private bool isPuzzleOpen = false;
    public bool puzzleFinished = false;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (isPuzzleOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzleUI();
        }
    }

    public void OpenPuzzleUI()
    {
        if (isPuzzleOpen || puzzleFinished) return;
        isPuzzleOpen = true;
        puzzleUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PuzzleComplete()
    {
        puzzleFinished = true;
        isPuzzleOpen = false;
        puzzleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        LockDoor door = FindAnyObjectByType<LockDoor>();
        if (door != null)
        {
            door.OpenDoor();
            Debug.Log("✅ 拼图完成，调用OpenDoor，门开始移动");
        }
        else
        {
            Debug.LogError("❌ 场景找不到 LockDoor 脚本！检查门上是否挂载");
        }
    }

    public void ClosePuzzleUI()
    {
        isPuzzleOpen = false;
        puzzleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}