using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 2f;

    private Vector3 startPosition;
    private Vector3 originalScale;
    private float timer;
    private bool isBig;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 随时间左右移动
        timer += Time.deltaTime;
        float offset = Mathf.Sin(timer * speed) * distance;
        transform.position = startPosition + Vector3.right * offset;

        // 按 Space 切换大小
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isBig = !isBig;
            transform.localScale = originalScale * (isBig ? 1.5f : 1f);
        }
    }
}