using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad { 
public class KeypadInteractionFPV : MonoBehaviour
{
    public static bool CursorUnlockRequested { get; private set; }

    private Camera cam;
    private Keypad keypad;
    private PuzzlePanel puzzlePanel;
    [SerializeField] private float interactionRange = 2.5f;

    private void Awake()
    {
        cam = Camera.main;
        keypad = FindObjectOfType<Keypad>();
        puzzlePanel = FindObjectOfType<PuzzlePanel>();
    }

    private void Update()
    {
        if (cam == null)
            cam = Camera.main;
        if (keypad == null)
            keypad = FindObjectOfType<Keypad>();
        if (puzzlePanel == null)
            puzzlePanel = FindObjectOfType<PuzzlePanel>();

        bool nearKeypad = cam != null && keypad != null &&
                          Vector3.Distance(cam.transform.position, keypad.transform.position) <= interactionRange;
        CursorUnlockRequested = nearKeypad;

        GameUIManager manager = GameUIManager.Instance;
        bool anotherUiNeedsCursor = (manager != null && manager.CursorShouldBeUnlocked) ||
                                    (puzzlePanel != null && puzzlePanel.IsPuzzleOpen);

        if (nearKeypad || anotherUiNeedsCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (keypad != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Only allow keypad mouse input while the player is close enough.
        if (!nearKeypad || cam == null)
            return;

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

    private void OnDisable()
    {
        CursorUnlockRequested = false;
    }
}
}
