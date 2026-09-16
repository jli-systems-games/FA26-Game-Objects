using UnityEngine;

public class PrizeFinishScript : MonoBehaviour
{
    public Rigidbody prizeBody;
    public GameObject prizeText;

    private bool finished = false;

    private void Start()
    {
        if (prizeText != null)
        {
            prizeText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finished || other.attachedRigidbody != prizeBody)
            return;

        finished = true;
        prizeText.SetActive(true);

        Debug.Log("PRIZE DISPENSED!");
    }
}