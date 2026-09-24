using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MovingObj : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 5f;

    public Vector2 moveInput;


    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    public TMP_Text scoreText;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        UpdateScore();
    }

    void Update()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();

        moveInput = moveAction.action.ReadValue<Vector2>();

        if (jumpAction.action.WasPressedThisFrame())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        rb.MovePosition(
            rb.position + movement * moveSpeed * Time.fixedDeltaTime
        );
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Win"))
        {
            WinGame();
        }

        if (other.CompareTag("Lava"))
        {
            PlayerDied();
        }
    }

    void WinGame()
    {
        
        int wins = PlayerPrefs.GetInt("Wins", 0);


        wins++;

    
        PlayerPrefs.SetInt("Wins", wins);
        PlayerPrefs.Save();

        Debug.Log("YOU WIN! Total Wins: " + wins);

    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlayerDied()
    {

        int deaths = PlayerPrefs.GetInt("Deaths", 0);

        deaths++;

        PlayerPrefs.SetInt("Deaths", deaths);
        PlayerPrefs.Save();

        Debug.Log("YOU DIED! Total Deaths: " + deaths);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateScore()
    {

        int wins = PlayerPrefs.GetInt("Wins", 0);
        int deaths = PlayerPrefs.GetInt("Deaths", 0);


        scoreText.text = "Wins: " + wins + "\nDeaths: " + deaths;
    }
}