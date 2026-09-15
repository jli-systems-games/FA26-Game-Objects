using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public GameObject winMessage;
    private bool finishTriggered = false;

// Set the main camera to active in the beginning, set the switch camera and win message to inactive.
    void Start()
    {
        if (mainCameraRef != null)
        {
            mainCameraRef.SetActive(true);
        }

        if (switchCamera != null)
        {
            switchCamera.SetActive(false);
        }
        
        if (winMessage != null)
        {
            winMessage.SetActive(false);
        }
    }

// Once detected the player, check if the player has won or lost the game.
    private void OnTriggerEnter(Collider other)
    {
        if (finishTriggered)
            return;
        Rigidbody playerRb = other.attachedRigidbody;

        if (playerRb != null &&
            playerRb.CompareTag("Player"))
        {
            finishTriggered = true;

            if (GameSetting.Instance == null)
                return;

            if (GameSetting.Instance.CheckWin())
            {
                Win();
            }
            else
            {
                Lose();
            }
        }
    }

// If the player wins, switch to the win scene.
    private void Win()
    {
        GameSetting.Instance.WinGame();

        if (mainCameraRef != null)
        {
            mainCameraRef.SetActive(false);
        }

        if (switchCamera != null)
        {
            switchCamera.SetActive(true);
        }

        if (winMessage != null)
        {
            winMessage.SetActive(true);
        }
    }

// If the player loses, reset the game.
    private void Lose()
    {
        GameSetting.Instance.ResetGame();
    }
}