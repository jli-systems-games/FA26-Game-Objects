using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuBook;

    public void OpenMenu()
    {
        menuBook.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        menuBook.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
