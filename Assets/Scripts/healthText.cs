using UnityEngine;
using TMPro; // Required for TextMeshPro UI components

public class healthText : MonoBehaviour
{
    private TMP_Text textBox;
    private Player playerScript; 

    void Start()
    {
        // 1. Get the TextMeshPro component attached to THIS GameObject
        textBox = GetComponent<TMP_Text>();

        // 2. Find the player in the scene and get their script component
        // (Make sure your Player object has the tag "Player")
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerScript = player.GetComponent<Player>();
        }
    }

    void Update()
    {
        // 3. Make sure we found the player and textbox components safely
        if (playerScript != null && textBox != null)
        {
            // 4. Update the text box with the current health value
            // (Replace ".health" with the exact name of your integer variable)
            textBox.text = "Health: " + playerScript.health.ToString();
        }
    }
}
