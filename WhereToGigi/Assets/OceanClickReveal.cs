
using UnityEngine;
using UnityEngine.EventSystems;

public class OceanClickReveal : MonoBehaviour, IPointerClickHandler
{
    public Animator oceanAnimator;

    public void OnPointerClick(PointerEventData eventData)
    {
        oceanAnimator.SetTrigger("Reveal");
    }
}
