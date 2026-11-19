using UnityEngine;
using UnityEngine.InputSystem;

public class LightBrightness : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.equalsKey.wasPressedThisFrame || Keyboard.current.numpadPlusKey.wasPressedThisFrame)
            GetComponent<Light>().intensity += 3f;

        if (Keyboard.current.minusKey.wasPressedThisFrame || Keyboard.current.numpadMinusKey.wasPressedThisFrame)
            GetComponent<Light>().intensity -= 3f;
    }
}