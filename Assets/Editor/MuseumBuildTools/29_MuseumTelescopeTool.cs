using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Balcony telescope pointing at a textured Earth far out in space (NASA Visible Earth image, public domain).
// Seen with the naked eye Earth is a tiny dot; through the telescope (E) it fills the lens.
public static class MuseumTelescopeTool
{
    [MenuItem("Tools/Museum/Balcony Telescope + Earth")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var balcony=GameObject.Find("11_Balcony")?.transform;var canvas=GameObject.Find("Canvas");
        if(balcony==null||canvas==null){Debug.LogError("Run the balcony tool (27) first.");return;}
        if(balcony.Find("Telescope")!=null){Debug.Log("Telescope already exists.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeTelescope.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        Material M(string n)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/"+n+".asset");
        var dark=M("Frame_Obsidian");var silver=M("Frame_Titanium");var cyan=M("Light_Cyan");

        // ----- Earth -----
        const string texPath="Assets/MuseumSky/Earth_NASA_VisibleEarth.jpg";
        if(AssetImporter.GetAtPath(texPath) is TextureImporter ti){ti.maxTextureSize=1024;ti.textureCompression=TextureImporterCompression.Compressed;ti.wrapModeU=TextureWrapMode.Repeat;ti.SaveAndReimport();}
        var em=new Material(Shader.Find("Universal Render Pipeline/Unlit")){name="Earth"};
        em.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(texPath));em.SetColor("_BaseColor",new Color(1.15f,1.2f,1.35f));
        AssetDatabase.CreateAsset(em,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/Earth.mat"));
        var earth=GameObject.CreatePrimitive(PrimitiveType.Sphere);earth.name="Earth";Object.DestroyImmediate(earth.GetComponent<Collider>());
        Undo.RegisterCreatedObjectUndo(earth,"earth");earth.transform.SetParent(balcony,false);
        earth.transform.localPosition=new Vector3(0,9f,120f);earth.transform.localScale=Vector3.one*1.3f;earth.transform.localRotation=Quaternion.Euler(-15,40,0);
        earth.GetComponent<Renderer>().sharedMaterial=em;earth.GetComponent<Renderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
        var dot=balcony.Find("PaleBlueDot");if(dot!=null){Undo.RecordObject(dot.gameObject,"hide");dot.gameObject.SetActive(false);}

        // ----- Telescope (primitives) on the right side of the deck -----
        var scope=new GameObject("Telescope").transform;Undo.RegisterCreatedObjectUndo(scope.gameObject,"scope");
        scope.SetParent(balcony,false);scope.localPosition=new Vector3(2.4f,0,1.6f);
        for(int i=0;i<3;i++){float a=i*120*Mathf.Deg2Rad;var foot=new Vector3(Mathf.Cos(a)*0.45f,0,Mathf.Sin(a)*0.45f);
            Beam(scope,"Leg_"+i,foot,new Vector3(0,1.15f,0),0.04f,silver);}
        var head=new GameObject("Head").transform;head.SetParent(scope,false);head.localPosition=new Vector3(0,1.25f,0);
        head.rotation=Quaternion.LookRotation(earth.transform.position-head.position);
        var tube=GameObject.CreatePrimitive(PrimitiveType.Cylinder);tube.name="Tube";Object.DestroyImmediate(tube.GetComponent<Collider>());
        tube.transform.SetParent(head,false);tube.transform.localRotation=Quaternion.Euler(90,0,0);tube.transform.localPosition=new Vector3(0,0,0.25f);tube.transform.localScale=new Vector3(0.16f,0.55f,0.16f);
        tube.GetComponent<Renderer>().sharedMaterial=dark;
        var ring=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ring.name="LensRing";Object.DestroyImmediate(ring.GetComponent<Collider>());
        ring.transform.SetParent(head,false);ring.transform.localRotation=Quaternion.Euler(90,0,0);ring.transform.localPosition=new Vector3(0,0,0.8f);ring.transform.localScale=new Vector3(0.2f,0.02f,0.2f);
        ring.GetComponent<Renderer>().sharedMaterial=cyan;
        var eyeGO=new GameObject("Eye");eyeGO.transform.SetParent(head,false);eyeGO.transform.localPosition=new Vector3(0,0.05f,-0.35f);
        var col=scope.gameObject.AddComponent<CapsuleCollider>();col.center=new Vector3(0,0.7f,0);col.height=1.4f;col.radius=0.35f;

        // ----- UI: prompt + lens view -----
        var menu=canvas.transform.Find("MuseumMenu");int idx=menu!=null?menu.GetSiblingIndex():canvas.transform.childCount;
        var prompt=Panel(canvas.transform,"TelescopePrompt",new Vector2(0.36f,0.2f),new Vector2(0.64f,0.27f),new Color(0.02f,0.03f,0.07f,0.9f));
        Text(prompt.transform,"<b>[E]</b>  LOOK THROUGH THE TELESCOPE",30,new Color(1f,0.75f,0.45f),TextAlignmentOptions.Center);
        var view=Panel(canvas.transform,"TelescopeView",Vector2.zero,Vector2.one,new Color(0,0,0,0));
        var lensGO=new GameObject("Lens",typeof(RectTransform));lensGO.transform.SetParent(view.transform,false);
        var lr=(RectTransform)lensGO.transform;lr.anchorMin=Vector2.zero;lr.anchorMax=Vector2.one;lr.offsetMin=lr.offsetMax=Vector2.zero;
        var lens=lensGO.AddComponent<RawImage>();lens.raycastTarget=false;
        var cap=Panel(view.transform,"Caption",new Vector2(0.2f,0.04f),new Vector2(0.8f,0.16f),new Color(0,0,0,0));
        Text(cap.transform,"<b>EARTH</b>  ·  third planet from its star  ·  home of every human who ever lived\n<size=70%>[E] step back   ·   move the mouse to look around</size>",28,new Color(0.85f,0.95f,1f),TextAlignmentOptions.Center);
        prompt.transform.SetSiblingIndex(idx);view.transform.SetSiblingIndex(idx);
        prompt.SetActive(false);view.SetActive(false);

        var t=scope.gameObject.AddComponent<MuseumTelescope>();
        var v=Object.FindFirstObjectByType<MuseumVisitorController>();
        t.visitor=v;t.interactor=v!=null?v.GetComponent<MuseumExhibitInteractor>():null;t.eye=eyeGO.transform;t.target=earth.transform;
        t.prompt=prompt;t.view=view;t.lens=lens;

        // Point the plaque text at the telescope.
        var plaque=GameObject.Find("Exhibit_10_WhereEarthWas")?.GetComponent<MuseumExhibit>();
        if(plaque!=null){Undo.RecordObject(plaque,"text");plaque.body=plaque.body.Replace("find the small blue light beyond the railing.","with your own eyes Earth is only a small blue light. Use the telescope on this deck to see it closer.");
            plaque.credit+=" Earth image: NASA Goddard Space Flight Center, Visible Earth (public domain).";}

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Telescope + Earth added to the balcony.");
    }

    static void Beam(Transform p,string n,Vector3 a,Vector3 b,float w,Material m)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;Object.DestroyImmediate(g.GetComponent<Collider>());g.transform.SetParent(p,false);
        g.transform.localPosition=(a+b)/2;g.transform.localRotation=Quaternion.LookRotation(b-a);g.transform.localScale=new Vector3(w,w,(b-a).magnitude);
        g.GetComponent<Renderer>().sharedMaterial=m;
    }
    static GameObject Panel(Transform p,string n,Vector2 min,Vector2 max,Color c)
    {
        var g=new GameObject(n,typeof(RectTransform));Undo.RegisterCreatedObjectUndo(g,n);g.transform.SetParent(p,false);
        var r=(RectTransform)g.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;
        var i=g.AddComponent<Image>();i.color=c;i.raycastTarget=false;return g;
    }
    static void Text(Transform p,string s,float size,Color c,TextAlignmentOptions a)
    {
        var g=new GameObject("Text",typeof(RectTransform));g.transform.SetParent(p,false);
        var r=(RectTransform)g.transform;r.anchorMin=Vector2.zero;r.anchorMax=Vector2.one;r.offsetMin=new Vector2(16,6);r.offsetMax=new Vector2(-16,-6);
        var t=g.AddComponent<TextMeshProUGUI>();t.text=s;t.fontSize=size;t.color=c;t.alignment=a;t.textWrappingMode=TextWrappingModes.Normal;t.raycastTarget=false;
        var pixel=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/MuseumFonts/PixelifySans SDF.asset");if(pixel!=null)t.font=pixel;
    }
}
