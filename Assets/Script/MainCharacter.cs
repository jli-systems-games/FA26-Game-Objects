using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacter : MonoBehaviour
{
    public float speed = 5f;
    public static Vector3 MainCharacterPosition;
    private InputAction move;

    void Start()
    {
        move = GetComponent<PlayerInput>().actions["Move"];
    }

    void Update()
    {
        Vector2 direction = move.ReadValue<Vector2>();
        transform.Translate(direction.x * speed * Time.deltaTime, 0, direction.y * speed * Time.deltaTime, Space.World);
        MainCharacterPosition = transform.position;
    }
}