using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableShell : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public int shellID;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector2 originalPosition;

    private bool placedCorrectly = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (placedCorrectly)
            return;

        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        // Dragged shell appears above everything else
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        // Allow the slot underneath to receive the drop
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (placedCorrectly)
            return;

        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!placedCorrectly)
        {
            ReturnToOriginalPosition();
        }
    }

    public void PlaceInSlot(Transform slotTransform)
    {
        placedCorrectly = true;

        transform.SetParent(slotTransform);
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;

        canvasGroup.blocksRaycasts = false;
    }

    public void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
    }
}
