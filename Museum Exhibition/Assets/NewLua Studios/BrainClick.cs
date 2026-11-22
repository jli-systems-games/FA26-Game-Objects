using UnityEngine;

public class BrainClick : MonoBehaviour
{
    public WelcomeController welcomeController;

    void OnMouseDown()
    {
        welcomeController.OpenWelcome();
    }
}