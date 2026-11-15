using UnityEngine;
using UnityEngine.InputSystem;

public class StartUI : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Time.timeScale = 1;
            Destroy(gameObject);
        }
    }
}