using UnityEngine;

public class MenuSign : MonoBehaviour
{
    public MenuController menuController;

    void OnMouseDown()
    {
        menuController.OpenMenu();
    }
}
