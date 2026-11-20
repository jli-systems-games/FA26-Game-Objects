using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public void EnterMuseum()
    {
        PauseMenu.ReturnedToMenu = true;
        SceneManager.LoadScene("OCMuseum");
    }
}