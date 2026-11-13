using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// Builds the start screen and the pause menu (uGUI on the existing Canvas, above the pixel + noise layers)
// and wires their buttons to MuseumMenu.
public static class MuseumMenuTool
{
    const string Controls=
        "<b>CONTROLS</b>\n"+
        "WASD / Arrows   move\nMouse   look\nShift   run\nE   inspect / open\n1–4   choose in a panel\nScroll   read more\nV   first / third person\nF   fullscreen\nTAB   menu";

    [MenuItem("Tools/Museum/Start Screen + Pause Menu")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var canvas=GameObject.Find("Canvas");var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        if(canvas==null||visitor==null){Debug.LogError("Need Canvas and MuseumVisitor.");return;}
        if(canvas.transform.Find("MuseumMenu")!=null){Debug.Log("Menu already exists.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeMenu.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        var root=UI("MuseumMenu",canvas.transform);Stretch(root);root.transform.SetAsLastSibling();
        var menu=root.AddComponent<MuseumMenu>();menu.visitor=visitor;menu.interactor=visitor.GetComponent<MuseumExhibitInteractor>();

        // ----- Start screen -----
        var start=UI("StartScreen",root.transform);Stretch(start);Img(start,new Color(0.01f,0.015f,0.05f,0.93f));
        Txt("Title",start.transform,new Vector2(0.08f,0.62f),new Vector2(0.92f,0.86f),"HUMAN MUSEUM",120,new Color(0.45f,0.95f,1f),TextAlignmentOptions.Center,true);
        Txt("Subtitle",start.transform,new Vector2(0.15f,0.54f),new Vector2(0.85f,0.62f),"a museum about humans, for visitors from elsewhere",34,new Color(1f,0.6f,0.85f),TextAlignmentOptions.Center,false);
        Txt("Intro",start.transform,new Vector2(0.1f,0.3f),new Vector2(0.5f,0.52f),
            "You are an alien visitor. Your ship has docked beside a glass museum floating in space.\n\nInside, the humans tell their own story: how their machines moved into their pockets, onto their bodies, and finally carried them off their planet.",
            28,new Color(0.85f,0.92f,1f),TextAlignmentOptions.TopLeft,false);
        Txt("Controls",start.transform,new Vector2(0.58f,0.22f),new Vector2(0.9f,0.52f),Controls,24,new Color(0.85f,0.92f,1f),TextAlignmentOptions.TopLeft,false);
        Button(start.transform,"StartButton","START",new Vector2(0.4f,0.1f),new Vector2(0.6f,0.18f),new Color(0.25f,0.85f,0.9f),menu.StartGame);
        Txt("Credits",start.transform,new Vector2(0.1f,0.02f),new Vector2(0.9f,0.07f),"Models, music and sky: free assets, credited in each exhibit panel and on the itch.io page.",18,new Color(0.55f,0.6f,0.75f),TextAlignmentOptions.Center,false);

        // ----- Pause menu -----
        var pause=UI("PauseMenu",root.transform);Stretch(pause);Img(pause,new Color(0.01f,0.015f,0.05f,0.8f));
        Txt("Paused",pause.transform,new Vector2(0.2f,0.72f),new Vector2(0.8f,0.86f),"PAUSED",80,new Color(0.45f,0.95f,1f),TextAlignmentOptions.Center,true);
        Button(pause.transform,"ResumeButton","RESUME",new Vector2(0.18f,0.52f),new Vector2(0.42f,0.6f),new Color(0.25f,0.85f,0.9f),menu.Resume);
        Button(pause.transform,"RestartButton","RESTART",new Vector2(0.18f,0.41f),new Vector2(0.42f,0.49f),new Color(0.67f,0.45f,1f),menu.Restart);
        Button(pause.transform,"ExitButton","EXIT",new Vector2(0.18f,0.3f),new Vector2(0.42f,0.38f),new Color(1f,0.5f,0.78f),menu.Exit);
        Txt("Controls",pause.transform,new Vector2(0.55f,0.24f),new Vector2(0.85f,0.62f),Controls,26,new Color(0.85f,0.92f,1f),TextAlignmentOptions.TopLeft,false);
        Txt("Hint",pause.transform,new Vector2(0.2f,0.14f),new Vector2(0.8f,0.2f),"TAB to resume",22,new Color(0.55f,0.6f,0.75f),TextAlignmentOptions.Center,false);
        pause.SetActive(false);

        menu.startScreen=start;menu.pauseMenu=pause;
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Start screen + pause menu added (TAB / P).");
    }

    static GameObject UI(string n,Transform p){var g=new GameObject(n,typeof(RectTransform));Undo.RegisterCreatedObjectUndo(g,n);g.transform.SetParent(p,false);return g;}
    static void Stretch(GameObject g){var r=(RectTransform)g.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=r.offsetMax=Vector2.zero;}
    static Image Img(GameObject g,Color c){var i=g.AddComponent<Image>();i.color=c;return i;}
    static TextMeshProUGUI Txt(string n,Transform p,Vector2 min,Vector2 max,string text,float size,Color c,TextAlignmentOptions a,bool bold)
    {
        var g=UI(n,p);var r=(RectTransform)g.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;
        var t=g.AddComponent<TextMeshProUGUI>();t.font=TMP_Settings.defaultFontAsset;t.text=text;t.fontSize=size;t.color=c;t.alignment=a;
        t.textWrappingMode=TextWrappingModes.Normal;t.raycastTarget=false;t.lineSpacing=8;if(bold)t.fontStyle=FontStyles.Bold;return t;
    }
    static void Button(Transform p,string n,string label,Vector2 min,Vector2 max,Color c,UnityAction onClick)
    {
        var g=UI(n,p);var r=(RectTransform)g.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;
        Img(g,c);var b=g.AddComponent<Button>();
        UnityEventTools.AddPersistentListener(b.onClick,onClick);   // shows up in the Button's OnClick list in the Inspector
        var t=Txt("Label",g.transform,Vector2.zero,Vector2.one,label,34,new Color(0.04f,0.04f,0.1f),TextAlignmentOptions.Center,true);
    }
}
