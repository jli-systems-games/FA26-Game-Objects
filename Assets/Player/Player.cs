using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int health;

    private SpriteRenderer playerSprite;

    public GameObject gameScreen;
    public GameObject endScreen;


    // Start is called before the first frame update
    void Start()
    {
        health = 5;
        playerSprite = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        
    }
        
        IEnumerator GotHurt()
        {
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = Color.white;
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Player collided with Enemy");
            health--;
            

            StartCoroutine(GotHurt());


            Debug.Log("Player health: " + health);

            if (health <= 0)
            {
                gameScreen.SetActive(false);
                endScreen.SetActive(true);
                health = 5;

                playerSprite.color = Color.white; 
            }
            

        }
}

