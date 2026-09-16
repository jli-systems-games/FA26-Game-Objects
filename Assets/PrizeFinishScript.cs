using UnityEngine;

public class PrizeFinishScript : MonoBehaviour
{
    public Rigidbody prizeBody;
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (finished || other.attachedRigidbody != prizeBody)
            return;

        finished = true;
        Debug.Log("Prize reached the finish!");
    }
}