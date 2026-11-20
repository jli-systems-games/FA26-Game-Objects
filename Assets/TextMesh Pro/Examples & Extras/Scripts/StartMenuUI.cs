using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public GameObject descriptionPanel;

    void Start()
    {
        if (descriptionPanel != null)
        {
            descriptionPanel.SetActive(false);
        }
    }

    void Update()
    {
        // click to close
        if (descriptionPanel != null &&
            descriptionPanel.activeSelf &&
            Input.GetMouseButtonDown(0))
        {
            descriptionPanel.SetActive(false);
        }
    }

    public void ShowDescription()
    {
        if (descriptionPanel == null)
        {
            Debug.LogError("Description Panel has not been assigned.");
            return;
        }

        descriptionPanel.SetActive(true);
        descriptionPanel.transform.SetAsLastSibling();
    }
}