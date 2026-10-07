using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableImage : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger("Clicked");
    }
}