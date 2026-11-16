using UnityEngine;

public class PuzzlePanel : MonoBehaviour
{
    [Header("拼图UI面板（Canvas）")]
    public GameObject puzzleUI;
    private bool isPuzzleOpen = false;

    void Update()
    {
        // 按ESC退出拼图
        if (isPuzzleOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzleUI();
        }
    }

    public void OpenPuzzleUI()
    {
        if (isPuzzleOpen) return;
        isPuzzleOpen = true;
        puzzleUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 拼图成功完成
    public void PuzzleComplete()
    {
        isPuzzleOpen = false;
        puzzleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<LockDoor>().OpenDoor();
    }

    // ESC / 关闭按钮调用
    public void ClosePuzzleUI()
    {
        isPuzzleOpen = false;
        puzzleUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}