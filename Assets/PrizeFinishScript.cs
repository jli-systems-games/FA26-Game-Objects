using UnityEngine;

public class PrizeFinishScript : MonoBehaviour
{
    public Rigidbody prizeBody;
    public GameObject prizeText;
    public ParticleSystem prizeConfetti;

    private bool finished = false;

    private void Start()
    {
        if (prizeText != null)
            prizeText.SetActive(false);

        if (prizeConfetti != null)
            prizeConfetti.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finished || other.attachedRigidbody != prizeBody)
            return;

        finished = true;

        if (prizeText != null)
            prizeText.SetActive(true);

        if (prizeConfetti != null)
            prizeConfetti.Play();

        Debug.Log("PRIZE DISPENSED!");
    }
}