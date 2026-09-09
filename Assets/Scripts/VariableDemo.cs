using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VariableDemo : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 moveInput;
    public InputActionReference moveAction;

    public float upSpeed;
    public float upInput;
    public InputActionReference upAction;

    public float downSpeed;
    public float downInput;
    public InputActionReference downAction;

    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;

        upInput = upAction.action.ReadValue<float>();

        transform.position += new Vector3(0, upInput, 0) * upSpeed * Time.deltaTime;

        downInput = downAction.action.ReadValue<float>();

        transform.position += new Vector3(0, -downInput, 0) * downSpeed * Time.deltaTime;
    }
}

