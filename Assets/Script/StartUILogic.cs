using UnityEngine;
using UnityEngine.InputSystem;

public class StartUILogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
void Update()
{
    if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
        (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame))
    {
        MainLogic.scene = 0;
        Destroy(gameObject);
    }
}
}
