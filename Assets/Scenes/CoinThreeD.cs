using UnityEngine;

public class CoinThreeD : MonoBehaviour
{
    public int coinCollected;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
            coinCollected++;
            this.gameObject.SetActive(false);
        }
            
    }
}
