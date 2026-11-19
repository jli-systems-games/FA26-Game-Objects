using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad { 
public class KeypadInteractionFPV : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float interactionRange = 2.5f;

    private void Awake() => cam = Camera.main;

    private void Update()
    {
        if (cam == null) return;

        // Only allow keypad mouse input while the player is close enough.
        var keypad = FindObjectOfType<Keypad>();
        if (keypad == null || Vector3.Distance(cam.transform.position, keypad.transform.position) > interactionRange)
        {
            return;
        }

        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                {
                    keypadButton.PressButton();
                }
            }
        }
    }
}
}
