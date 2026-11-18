using UnityEngine;
using UnityEngine.UI;

// The museum renders to a fixed pixel-art texture. Fit that texture, never stretch it.
[RequireComponent(typeof(RawImage))]
public class MuseumComfortView : MonoBehaviour
{
    RectTransform view;
    RawImage image;
    int previousFrameRate;
    void OnEnable()
    {
        previousFrameRate=Application.targetFrameRate;
        Application.targetFrameRate=60;
    }
    void OnDisable(){Application.targetFrameRate=previousFrameRate;}
    void Awake(){view=(RectTransform)transform;image=GetComponent<RawImage>();}
    void LateUpdate()
    {
        var parent=view.parent as RectTransform;if(parent==null || !image.texture)return;
        float aspect=image.texture.width/(float)image.texture.height;
        Vector2 size=parent.rect.size;
        if(size.x/Mathf.Max(1,size.y)>aspect)size.x=size.y*aspect;else size.y=size.x/aspect;
        view.anchorMin=view.anchorMax=new Vector2(.5f,.5f);view.pivot=new Vector2(.5f,.5f);
        view.anchoredPosition=Vector2.zero;view.sizeDelta=size;
    }
}
