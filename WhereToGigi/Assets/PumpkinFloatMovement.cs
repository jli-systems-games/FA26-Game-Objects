
using UnityEngine;

public class PumpkinFloatMovement : MonoBehaviour
{
    public float distance = 200f;
    public float speed = 2f;

    private RectTransform rect;
    private Vector2 startPosition;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPosition = rect.anchoredPosition;
    }

    void Update()
    {
        float movement = (1f - Mathf.Cos(Time.time * speed)) / 2f;
        rect.anchoredPosition = startPosition + Vector2.up * movement * distance;
    }
}
