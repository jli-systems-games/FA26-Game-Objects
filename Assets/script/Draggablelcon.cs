using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform iconRect;
    public Canvas canvas;
    [HideInInspector] public Transform originalParent;
    // 加上这一行！public 才能在Inspector显示
    public int iconID;

    private Image img;

    private void Awake()
    {
        iconRect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        img.raycastTarget = false;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        iconRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        img.raycastTarget = true;
        GameObject dropTarget = eventData.pointerEnter;
        Debug.Log($"【拖拽结束】鼠标下物体：{(dropTarget != null ? dropTarget.name : "无")}");

        if (dropTarget != null && dropTarget.CompareTag("PuzzleGrid"))
        {
            Debug.Log($"✅ 识别到格子：{dropTarget.name}");
            transform.SetParent(dropTarget.transform);
            iconRect.anchoredPosition = Vector2.zero;
            PuzzleChecker.Instance.CheckAllMatches();
        }
        else
        {
            Debug.Log("❌ 识别失败，退回图标池");
            transform.SetParent(originalParent);
            iconRect.anchoredPosition = Vector2.zero;
        }
    }
}