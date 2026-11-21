using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public GameObject floatingPrompt;
    public GameObject confirmPanel;

    public string endSceneName = "EndScene";

    private bool playerNear = false;

    private Transform mainCamera;

    void Start()
    {
        floatingPrompt.SetActive(false);
        confirmPanel.SetActive(false);

        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        if (floatingPrompt.activeSelf && mainCamera != null)
        {
            floatingPrompt.transform.LookAt(
                floatingPrompt.transform.position + mainCamera.forward,
                mainCamera.up
            );
        }

        if (playerNear)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                confirmPanel.SetActive(true);
                floatingPrompt.SetActive(false);
            }
        }

        if (confirmPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(endSceneName);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                confirmPanel.SetActive(false);

                if (playerNear)
                {
                    floatingPrompt.SetActive(true);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            floatingPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            floatingPrompt.SetActive(false);
            confirmPanel.SetActive(false);
        }
    }
}
