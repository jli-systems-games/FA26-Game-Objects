using UnityEngine;

public class NoteCard : MonoBehaviour
{
    [Header("这幅画专属介绍UI")]
    public GameObject noteContentUI;
    [Header("靠近提示：Press E to view note")]
    public GameObject promptUI;

    private bool playerInRange;
    private bool isUnlocked;
    private bool isShowing;

    void Start()
    {
        noteContentUI.SetActive(false);
        promptUI.SetActive(false);
        isUnlocked = false;
        isShowing = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isUnlocked)
            {
                // 第一次按E：解锁笔记并打开
                isUnlocked = true;
                isShowing = true;
                noteContentUI.SetActive(true);
                promptUI.SetActive(false);
            }
            else
            {
                // 已经解锁：按E切换显示/隐藏
                isShowing = !isShowing;
                noteContentUI.SetActive(isShowing);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 没解锁才显示提示
            if (!isUnlocked)
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }
}