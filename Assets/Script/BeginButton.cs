using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginButton : MonoBehaviour
{
    public void BeginGame()
    {
        SceneManager.LoadScene("Game");
    }
}
