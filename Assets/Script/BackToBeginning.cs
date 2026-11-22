using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToBeginning : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}