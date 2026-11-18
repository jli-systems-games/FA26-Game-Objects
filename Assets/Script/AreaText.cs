using UnityEngine;
using TMPro;

public class AreaText : MonoBehaviour
{
    public TMP_Text text;
    public string message;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = message;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.text = "";
        }
    }
}