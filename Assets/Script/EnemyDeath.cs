using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Collider bat;
    public GameObject alive, blood;

    void OnTriggerEnter(Collider other)
    {
        if (other == bat)
        {

            blood.SetActive(true); 
            Destroy(gameObject);
        }
    }
}