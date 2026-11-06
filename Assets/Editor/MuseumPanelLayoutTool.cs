using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Makes the exhibit panel readable in fullscreen WebGL:
//  - Canvas scales with screen size (reference 1920x1080) instead of fixed pixels, so text is not tiny on big screens.
//  - The panel is narrower (right ~36%) so the exhibit stays visible.
//  - All text lives in a vertical scroll view (mouse wheel / trackpad); Close + hint stay fixed at the bottom.
// Reuses the existing text objects, so MuseumExhibitInteractor references stay valid.
public static class MuseumPanelLayoutTool
{
    [MenuItem("Tools/Museum/Readable Scrollable Exhibit Panel")]
    public static void Apply()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var it=Object.FindFirstObjectByType<MuseumExhibitInteractor>();
        if(it==null||it.panelRoot==null){Debug.LogError("Exhibit panel not found");return;}
        var panel=(RectTransform)it.panelRoot.transform;var content=panel.Find("Content");
        if(panel.Find("Scroll")!=null){Debug.Log("Panel already scrollable.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforePanel.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // 1) Canvas scaling
        var scaler=panel.GetComponentInParent<CanvasScaler>();
        if(scaler!=null){Undo.RecordObject(scaler,"scaler");scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution=new Vector2(1920,1080);scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;scaler.matchWidthOrHeight=0.5f;}

        // 2) Narrower panel on the right
        Undo.RecordObject(panel,"panel");panel.anchorMin=new Vector2(0.62f,0.1f);panel.anchorMax=new Vector2(0.98f,0.92f);panel.offsetMin=panel.offsetMax=Vector2.zero;

        // 3) Scroll view
        var scrollGO=New("Scroll",panel);Stretch(scrollGO,new Vector2(28,86),new Vector2(-14,-18));
        var viewport=New("Viewport",scrollGO.transform);Stretch(viewport,Vector2.zero,new Vector2(-14,0));viewport.AddComponent<RectMask2D>();
        var list=New("List",viewport.transform);var lr=(RectTransform)list.transform;
        lr.anchorMin=new Vector2(0,1);lr.anchorMax=new Vector2(1,1);lr.pivot=new Vector2(0.5f,1);lr.offsetMin=lr.offsetMax=Vector2.zero;
        var v=list.AddComponent<VerticalLayoutGroup>();v.spacing=18;v.childControlWidth=v.childControlHeight=true;v.childForceExpandWidth=true;v.childForceExpandHeight=false;v.padding=new RectOffset(0,8,4,24);
        list.AddComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
        var sr=scrollGO.AddComponent<ScrollRect>();sr.horizontal=false;sr.vertical=true;sr.viewport=(RectTransform)viewport.transform;sr.content=lr;
        sr.movementType=ScrollRect.MovementType.Clamped;sr.scrollSensitivity=40;
        var bar=New("Scrollbar",scrollGO.transform);var br=(RectTransform)bar.transform;br.anchorMin=new Vector2(1,0);br.anchorMax=Vector2.one;br.pivot=new Vector2(1,0.5f);br.offsetMin=new Vector2(-6,0);br.offsetMax=Vector2.zero;
        bar.AddComponent<Image>().color=new Color(1,1,1,0.08f);
        var handle=New("Handle",bar.transform);Stretch(handle,Vector2.zero,Vector2.zero);var hi=handle.AddComponent<Image>();hi.color=new Color(0.25f,0.85f,0.9f,0.8f);
        var sb=bar.AddComponent<Scrollbar>();sb.direction=Scrollbar.Direction.BottomToTop;sb.handleRect=(RectTransform)handle.transform;sb.targetGraphic=hi;
        sr.verticalScrollbar=sb;sr.verticalScrollbarVisibility=ScrollRect.ScrollbarVisibility.AutoHide;

        // Move texts (bigger fixed sizes; reference 1080p) and the mode buttons into the list, in reading order.
        Text(it.titleText,list.transform,46,true);Text(it.metaText,list.transform,24,false);Text(it.bodyText,list.transform,28,false);
        Text(it.themeText,list.transform,26,false);
        var row=New("ModeButtons",list.transform);var h=row.AddComponent<HorizontalLayoutGroup>();h.spacing=8;h.childControlWidth=h.childControlHeight=true;h.childForceExpandWidth=true;h.childForceExpandHeight=true;
        row.AddComponent<LayoutElement>().preferredHeight=78;
        foreach(var b in it.modeButtons){if(b==null)continue;Undo.SetTransformParent(b.transform,row.transform,"btn");
            var l=b.GetComponentInChildren<TMP_Text>();if(l!=null){Undo.RecordObject(l,"btn text");l.enableAutoSizing=true;l.fontSizeMin=16;l.fontSizeMax=22;}}
        Text(it.captionText,list.transform,26,false);Text(it.creditText,list.transform,18,false);

        // Footer stays fixed
        var close=(RectTransform)it.closeButton.transform;Undo.RecordObject(close,"close");Undo.SetTransformParent(close,panel,"close");
        close.anchorMin=new Vector2(0.55f,0);close.anchorMax=new Vector2(1,0);close.pivot=new Vector2(0.5f,0);close.offsetMin=new Vector2(0,16);close.offsetMax=new Vector2(-16,74);
        var cl=close.GetComponentInChildren<TMP_Text>();if(cl!=null){Undo.RecordObject(cl,"t");cl.fontSize=24;}
        var hint=content!=null?content.Find("KeysHint"):null;
        if(hint!=null){var hr=(RectTransform)hint;Undo.SetTransformParent(hr,panel,"hint");hr.anchorMin=new Vector2(0,0);hr.anchorMax=new Vector2(0.55f,0);hr.pivot=new Vector2(0,0);hr.offsetMin=new Vector2(28,16);hr.offsetMax=new Vector2(-8,74);
            var ht=hint.GetComponent<TMP_Text>();Undo.RecordObject(ht,"t");ht.fontSize=20;ht.text="Scroll for more  ·  1–4 choose  ·  E close";}
        if(content!=null){Undo.RecordObject(content.gameObject,"hide");content.gameObject.SetActive(false);} // now empty, kept

        Undo.RecordObject(it,"scroll");it.scroll=sr;
        // Prompt text a little bigger too
        if(it.promptText!=null){Undo.RecordObject(it.promptText,"p");it.promptText.fontSize=30;}
        if(it.visitor!=null && it.visitor.controlsText!=null){Undo.RecordObject(it.visitor.controlsText,"hud");it.visitor.controlsText.fontSize=24;}

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Exhibit panel is now scrollable, narrower and scales with screen size.");
    }
    static GameObject New(string n,Transform p){var g=new GameObject(n,typeof(RectTransform));Undo.RegisterCreatedObjectUndo(g,n);g.transform.SetParent(p,false);return g;}
    static void Stretch(GameObject g,Vector2 min,Vector2 max){var r=(RectTransform)g.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=min;r.offsetMax=max;}
    static void Text(TMP_Text t,Transform parent,float size,bool bold)
    {
        if(t==null)return;Undo.SetTransformParent(t.transform,parent,"text");Undo.RecordObject(t,"text");
        t.enableAutoSizing=false;t.fontSize=size;t.textWrappingMode=TextWrappingModes.Normal;t.alignment=TextAlignmentOptions.TopLeft;
        if(bold)t.fontStyle=FontStyles.Bold;t.lineSpacing=6;
    }
}
