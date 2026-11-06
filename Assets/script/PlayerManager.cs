using UnityEngine;

public class PlayerKeyManager : MonoBehaviour
{
    [Header("钥匙设置")]
    public int requiredKeys = 4;
    public int currentKeys = 0;

    [Header("检测范围")]
    public float interactRange = 2f;

    private GameObject nearbyKey;
    private GameObject nearbyDoor;

    void Update()
    {
        // E 拾取钥匙
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (nearbyKey != null)
            {
                CollectKey();
            }
        }

        // F 开门
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (nearbyDoor != null && currentKeys >= requiredKeys)
            {
                nearbyDoor.GetComponent<LockDoor>().OpenDoor();
            }
        }
    }

    void CollectKey()
    {
        currentKeys++;
        Destroy(nearbyKey);
        nearbyKey = null;
        Debug.Log($"拿到钥匙！当前：{currentKeys} / {requiredKeys}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            nearbyKey = other.gameObject;
        }
        if (other.CompareTag("Door"))
        {
            nearbyDoor = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key") && other.gameObject == nearbyKey)
        {
            nearbyKey = null;
        }
        if (other.CompareTag("Door") && other.gameObject == nearbyDoor)
        {
            nearbyDoor = null;
        }
    }
}
