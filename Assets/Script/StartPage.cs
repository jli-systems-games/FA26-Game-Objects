using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPage : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}