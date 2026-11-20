using UnityEngine;
using TMPro;

public class AreaText : MonoBehaviour
{
    [TextArea]
    public TMP_Text text;
    [TextArea]
    public string message;
    public GameObject blackBackground;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = message;
            blackBackground.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = "";
            blackBackground.SetActive(false);
        }
    }
}