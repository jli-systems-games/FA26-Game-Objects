using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// Puts the airlock cabin INSIDE the imported spaceship model (Visitor_Spaceship):
// the ship is moved/scaled so its hull wraps around the cabin, with the hatch on the hull facing the bridge.
// Also moves the welcome text + hatch prompt from 3D text (pixelated by the render texture) to sharp screen UI.
public static class MuseumShipInteriorTool
{
    [MenuItem("Tools/Museum/Put Airlock Inside Spaceship + Sharp Welcome Text")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var airlock=GameObject.Find("08_ShipAirlock");var ship=GameObject.Find("Visitor_Spaceship");var canvas=GameObject.Find("Canvas");
        if(airlock==null||ship==null||canvas==null){Debug.LogError("Need 08_ShipAirlock, Visitor_Spaceship and Canvas.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeShipInterior.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var a=airlock.transform;var s=ship.transform;

        // ----- 1. Wrap the ship around the cabin -----
        Undo.SetTransformParent(s,a,"ship");Undo.RecordObject(s,"ship");
        s.localPosition=Vector3.zero;s.localScale=Vector3.one;s.localRotation=Quaternion.identity;
        var b=LocalBounds(s,a);
        if(b.size.z>b.size.x){s.localRotation=Quaternion.Euler(0,90,0);b=LocalBounds(s,a);} // long side runs along the bridge end, hatch on its flank
        // Cabin is 4.4 wide x 3 high x 4.4 deep: make the hull clearly bigger in every direction.
        float k=Mathf.Max(1f,Mathf.Max(9f/b.size.x,Mathf.Max(4.8f/b.size.y,6.2f/b.size.z)));
        s.localScale=Vector3.one*k;b=LocalBounds(s,a);
        // Centre it over the cabin; hull front just behind the hatch so the door frame sticks out of the hull.
        s.localPosition+=new Vector3(-b.center.x,1.5f-b.center.y,0.15f-b.min.z);

        foreach(var n in new[]{"DockingTube","DockingTubeRing"}){var t=a.Find(n);if(t!=null){Undo.RecordObject(t.gameObject,"hide");t.gameObject.SetActive(false);}}

        // ----- 2. Sharp screen-space welcome text + prompt -----
        var door=airlock.GetComponent<MuseumSlidingDoor>();
        foreach(var n in new[]{"Hatch/WelcomeText","HatchPrompt"}){var t=a.Find(n);if(t!=null){Undo.RecordObject(t.gameObject,"hide");t.gameObject.SetActive(false);}}
        var welcome=UI("AirlockWelcome",canvas.transform);
        var menu=canvas.transform.Find("MuseumMenu");welcome.transform.SetSiblingIndex(menu!=null?menu.GetSiblingIndex():canvas.transform.childCount-1); // below the menus
        Rect(welcome,new Vector2(0.25f,0.6f),new Vector2(0.75f,0.92f));
        var bg=welcome.AddComponent<UnityEngine.UI.Image>();bg.color=new Color(0.01f,0.02f,0.06f,0.82f);bg.raycastTarget=false;
        var wt=Txt(welcome.transform,"Text",
            "<size=150%><b>WELCOME, VISITOR</b></size>\n\nOutside this hatch is a museum the humans built about themselves: how their machines changed their lives, and how those machines carried them off their planet.",
            30,new Color(0.85f,0.95f,1f));
        ((RectTransform)wt.transform).offsetMin=new Vector2(30,20);((RectTransform)wt.transform).offsetMax=new Vector2(-30,-20);
        var promptGO=UI("HatchPromptUI",canvas.transform);promptGO.transform.SetSiblingIndex(welcome.transform.GetSiblingIndex()+1);
        Rect(promptGO,new Vector2(0.38f,0.2f),new Vector2(0.62f,0.27f));
        var pbg=promptGO.AddComponent<UnityEngine.UI.Image>();pbg.color=new Color(0.02f,0.03f,0.07f,0.9f);pbg.raycastTarget=false;
        var pt=Txt(promptGO.transform,"Text","<b>[E]</b>  OPEN HATCH",32,new Color(1f,0.75f,0.45f));
        if(door!=null){Undo.RecordObject(door,"ui");door.prompt=pt;door.promptRoot=promptGO;door.welcomeUI=welcome;}
        promptGO.SetActive(false);welcome.SetActive(false);

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Airlock is now inside the spaceship (scale x"+k.ToString("0.0")+"). Welcome text is sharp screen UI. Fine-tune Visitor_Spaceship by hand if the hull clips into the cabin.");
    }

    // Exact bounds of all meshes under 'root', in the local space of 'space'.
    static Bounds LocalBounds(Transform root,Transform space)
    {
        bool first=true;var b=new Bounds();
        foreach(var mf in root.GetComponentsInChildren<MeshFilter>()){
            if(mf.sharedMesh==null)continue;var m=mf.transform.localToWorldMatrix;var mb=mf.sharedMesh.bounds;
            for(int i=0;i<8;i++){
                var c=mb.center+Vector3.Scale(mb.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1));
                var p=space.InverseTransformPoint(m.MultiplyPoint3x4(c));
                if(first){b=new Bounds(p,Vector3.zero);first=false;}else b.Encapsulate(p);
            }
        }
        return b;
    }
    static GameObject UI(string n,Transform p){var g=new GameObject(n,typeof(RectTransform));Undo.RegisterCreatedObjectUndo(g,n);g.transform.SetParent(p,false);return g;}
    static void Rect(GameObject g,Vector2 min,Vector2 max){var r=(RectTransform)g.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;}
    static TextMeshProUGUI Txt(Transform p,string n,string text,float size,Color c)
    {
        var g=UI(n,p);Rect(g,Vector2.zero,Vector2.one);var t=g.AddComponent<TextMeshProUGUI>();t.font=TMP_Settings.defaultFontAsset;
        t.text=text;t.fontSize=size;t.color=c;t.alignment=TextAlignmentOptions.Center;t.textWrappingMode=TextWrappingModes.Normal;t.raycastTarget=false;return t;
    }
}
