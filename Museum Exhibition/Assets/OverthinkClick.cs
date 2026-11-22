 using UnityEngine;

public class OverthinkingClick : MonoBehaviour
{
    public OverthinkingController overthinkingController;

    void OnMouseDown()
    {
        overthinkingController.OpenPanel();
    }
}
