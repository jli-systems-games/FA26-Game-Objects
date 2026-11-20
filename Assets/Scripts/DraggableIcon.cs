using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform iconRect;
    public Canvas canvas;
    [HideInInspector] public Transform originalParent;
    public int iconID;

    private Image image;

    private void Awake()
    {
        if (iconRect == null)
            iconRect = GetComponent<RectTransform>();
        if (image == null)
            image = GetComponent<Image>();
        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
        if (originalParent == null)
            originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (iconRect == null)
            return;

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError($"{name} cannot be dragged because it has no parent Canvas.", this);
            return;
        }

        if (originalParent == null)
            originalParent = transform.parent;

        if (image != null)
            image.raycastTarget = false;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (iconRect == null || canvas == null || canvas.scaleFactor <= 0f)
            return;

        iconRect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Resolve the target while this icon is still excluded from UI raycasts.
        Transform grid = FindGridUnderPointer(eventData);

        if (image != null)
            image.raycastTarget = true;

        if (grid == null)
        {
            ReturnToOriginalParent();
            return;
        }

        PuzzleChecker checker = PuzzleChecker.Instance;
        if (checker == null)
            checker = FindObjectOfType<PuzzleChecker>();

        if (checker == null)
        {
            Debug.LogError("An icon reached a puzzle grid, but no PuzzleChecker is in the scene.");
            ReturnToOriginalParent();
            return;
        }

        if (!checker.IsCorrectGridForIcon(iconID, grid))
        {
            Debug.Log($"{name} belongs in its assigned grid, not {grid.name}; returning it to the icon tray.");
            ReturnToOriginalParent();
            return;
        }

        transform.SetParent(grid, false);
        iconRect.anchoredPosition = Vector2.zero;
        iconRect.localRotation = Quaternion.identity;
        iconRect.localScale = Vector3.one;
        checker.CheckAllMatches();
    }

    private Transform FindGridUnderPointer(PointerEventData eventData)
    {
        Transform grid = FindGridInParents(eventData.pointerCurrentRaycast.gameObject);
        if (grid != null)
            return grid;

        grid = FindGridInParents(eventData.pointerEnter);
        if (grid != null)
            return grid;

        // Some UI modules leave pointerEnter on the dragged object on release.
        // Use the pointer's screen position against the grid rectangles as a fallback.
        PuzzleChecker checker = PuzzleChecker.Instance;
        if (checker == null)
            checker = FindObjectOfType<PuzzleChecker>();
        if (checker == null)
            return null;

        Camera eventCamera = canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay
            ? canvas.worldCamera
            : null;

        RectTransform[] rects = checker.GetComponentsInChildren<RectTransform>(true);
        foreach (RectTransform rect in rects)
        {
            if (!IsPuzzleGrid(rect))
                continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(rect, eventData.position, eventCamera))
                return rect;
        }

        return null;
    }

    private static Transform FindGridInParents(GameObject hitObject)
    {
        Transform candidate = hitObject != null ? hitObject.transform : null;
        while (candidate != null)
        {
            if (IsPuzzleGrid(candidate))
                return candidate;
            candidate = candidate.parent;
        }

        return null;
    }

    private static bool IsPuzzleGrid(Transform candidate)
    {
        if (candidate == null)
            return false;

        return candidate.CompareTag("PuzzleGrid") ||
               candidate.name.StartsWith("Grid_", StringComparison.OrdinalIgnoreCase) ||
               candidate.name.StartsWith("grid_", StringComparison.OrdinalIgnoreCase);
    }

    private void ReturnToOriginalParent()
    {
        if (originalParent == null)
            return;

        transform.SetParent(originalParent, false);
        if (iconRect != null)
        {
            iconRect.anchoredPosition = Vector2.zero;
            iconRect.localRotation = Quaternion.identity;
            iconRect.localScale = Vector3.one;
        }
    }
}
