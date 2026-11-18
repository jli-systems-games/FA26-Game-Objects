
using UnityEngine;

public class AppleEat : MonoBehaviour
{
    public GameObject dialogue;
    private float timer = 0;

    void Start(){
        dialogue.SetActive(false);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
            Destroy(gameObject);

        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
            timer = 1.5f;
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                dialogue.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
