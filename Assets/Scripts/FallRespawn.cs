using UnityEngine;

public class FallRespawn : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Transform respawnPoint;

    [TextArea(2, 4)]
    public string[] fallLines;

    bool playerIsFalling = false;
    Rigidbody playerRigidbody;
    Transform playerTransform;

    void OnTriggerEnter(Collider other)
    {
        PlayerMove player = other.GetComponentInParent<PlayerMove>();

        // if (player == null || playerIsFalling)
        //     return;

        // playerIsFalling = true;

        // playerTransform = player.transform;
        // playerRigidbody = player.GetComponent<Rigidbody>();

        // // Stop the player falling farther while reading.
        // if (playerRigidbody != null)
        // {
        //     playerRigidbody.linearVelocity = Vector3.zero;
        //     playerRigidbody.angularVelocity = Vector3.zero;
        //     playerRigidbody.isKinematic = true;
        // }

        if (dialogueManager == null)
    {
     Debug.LogError("FallRespawn: Dialogue Manager is not assigned.");
    return;
    }

    if (fallLines == null || fallLines.Length == 0)
    {
        Debug.LogError("FallRespawn: Fall Lines is empty.");
        return;
    }

    if (dialogueManager.IsDialogueOpen)
    {
        Debug.LogWarning("FallRespawn: A dialogue is already open.");
        return;
    }

    playerIsFalling = true;

    playerTransform = player.transform;
    playerRigidbody = player.GetComponent<Rigidbody>();

    if (playerRigidbody != null)
    {
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        playerRigidbody.isKinematic = true;
    }


        dialogueManager.StartDialogue(fallLines, RespawnPlayer);
    }

    void RespawnPlayer()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
            playerRigidbody.position = respawnPoint.position;
            playerRigidbody.rotation = respawnPoint.rotation;
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }
        else if (playerTransform != null)
        {
            playerTransform.position = respawnPoint.position;
            playerTransform.rotation = respawnPoint.rotation;
        }

        playerIsFalling = false;
    }
}