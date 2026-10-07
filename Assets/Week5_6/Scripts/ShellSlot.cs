using UnityEngine;
using UnityEngine.EventSystems;

public class ShellSlot : MonoBehaviour, IDropHandler
{
    public int slotID;
    private bool occupied = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (occupied)
            return;

        DraggableShell shell =
            eventData.pointerDrag.GetComponent<DraggableShell>();

        if (shell == null)
            return;

        if (shell.shellID == slotID)
        {
            occupied = true;
            shell.PlaceInSlot(transform);

            ShellGameManager.instance.ShellPlacedCorrectly();
        }
    }
}