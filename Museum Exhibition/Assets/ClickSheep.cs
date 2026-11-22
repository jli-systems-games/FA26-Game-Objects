using UnityEngine;

public class SheepClick : MonoBehaviour
{
    public static int sheepRemaining = 7;
    public GameObject finalMessage;

    void OnMouseDown()
    {
        sheepRemaining--;

        gameObject.SetActive(false);

        if (sheepRemaining == 0)
        {
            finalMessage.SetActive(true);
        }
    }
}
