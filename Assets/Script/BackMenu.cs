using UnityEngine;
using UnityEngine.SceneManagement;
public class BackMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Open");
    }
}
