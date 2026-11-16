using UnityEngine;

public class PlayerKeyManager2 : MonoBehaviour
{
    [Header("钥匙需求数量：6")]
    public int requiredKeys = 6;
    public int currentKeys = 0;

    [Header("交互范围")]
    public float interactRange = 2.0f;

    private GameObject nearbyKey;
    private GameObject nearbyPuzzlePanel;

    void Update()
    {
        // E拾取钥匙
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyKey != null)
            {
                CollectKey();
            }
        }
        // F 打开谜题面板（需要凑够6钥匙）
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (nearbyPuzzlePanel != null)
            {
                if (currentKeys >= requiredKeys)
                {
                    nearbyPuzzlePanel.GetComponent<PuzzlePanel>().OpenPuzzleUI();
                }
                else
                {
                    Debug.Log("钥匙不够，还需要：" + (requiredKeys - currentKeys));
                }
            }
        }
    }

    void CollectKey()
    {
        currentKeys++;
        Destroy(nearbyKey);
        nearbyKey = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key2"))
        {
            nearbyKey = other.gameObject;
        }
        if (other.CompareTag("PuzzlePanel"))
        {
            nearbyPuzzlePanel = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key2"))
        {
            nearbyKey = null;
        }
        if (other.CompareTag("PuzzlePanel"))
        {
            nearbyPuzzlePanel = null;
        }
    }
}
