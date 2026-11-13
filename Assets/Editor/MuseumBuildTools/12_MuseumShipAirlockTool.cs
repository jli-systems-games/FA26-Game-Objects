using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// Opening of the game: the alien visitor starts inside a small airlock cabin attached to its spaceship,
// at the far end of the arrival bridge. Press E at the hatch -> it slides open -> walk across the bridge into the museum.
// Built from Unity primitives + existing museum materials. The old glass guard at the bridge end is hidden (kept).
public static class MuseumShipAirlockTool
{
    const string Folder="Assets/MuseumGenerated";
    static Material M(string n)=>AssetDatabase.LoadAssetAtPath<Material>(Folder+"/"+n+".asset");

    static GameObject Box(string name,Transform parent,Vector3 pos,Vector3 size,Material m,bool collider=true)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent,false);
        g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;
        if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
    }
    static TextMeshPro Text(string name,Transform parent,Vector3 pos,string text,float size,Vector2 box,Color c)
    {
        var g=new GameObject(name,typeof(TextMeshPro));g.transform.SetParent(parent,false);
        g.transform.localPosition=pos;g.transform.localRotation=Quaternion.Euler(0,180,0); // read from inside the cabin
        var t=g.GetComponent<TextMeshPro>();t.text=text;t.fontSize=size;t.color=c;t.alignment=TextAlignmentOptions.Center;
        t.rectTransform.sizeDelta=box;t.textWrappingMode=TextWrappingModes.Normal;return t;
    }

    [MenuItem("Tools/Museum/Start Inside The Ship (Airlock)")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var arch=GameObject.Find("SpaceMuseum_Architecture");var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        if(arch==null||visitor==null){Debug.LogError("Need SpaceMuseum_Architecture and MuseumVisitor.");return;}
        if(arch.transform.Find("08_ShipAirlock")!=null){Debug.Log("Airlock already exists.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath(Folder+"/SceneBackups/MidtermMuseum_BeforeAirlock.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        var dark=M("Frame_Obsidian");var silver=M("Frame_Titanium");var panel=M("Panel_Gradient_Ribbed");
        var glass=M("Glass_Ice");var cyan=M("Light_Cyan");var violet=M("Light_Violet");

        // Local frame: origin = bridge end (front wall / hatch), +Z points away from the museum.
        Vector3 museumDoor=new Vector3(4,0,-12),outward=new Vector3(0.6f,0,-0.8f);
        var root=new GameObject("08_ShipAirlock").transform;Undo.RegisterCreatedObjectUndo(root.gameObject,"airlock");
        root.SetParent(arch.transform,false);root.localPosition=museumDoor+outward*8f;root.localRotation=Quaternion.LookRotation(outward);

        const float W=4.4f,D=4.4f,H=3f,T=0.2f;
        Box("Floor",root,new Vector3(0,-0.25f,D/2),new Vector3(W,0.5f,D),dark);
        Box("Ceiling",root,new Vector3(0,H+0.05f,D/2),new Vector3(W+T,0.1f,D+T),panel);
        Box("BackWall",root,new Vector3(0,H/2,D),new Vector3(W+T,H,T),panel);
        Box("Wall_ToShip",root,new Vector3(-W/2,H/2,D/2),new Vector3(T,H,D+T),panel);
        Box("Window_Space",root,new Vector3(W/2,H/2,D/2),new Vector3(T*0.5f,H,D+T),glass); // look out at the nebula
        // Front wall with a 1.6 x 2.4 m doorway
        Box("FrontWall_A",root,new Vector3(-1.5f,H/2,0),new Vector3(1.4f,H,T),panel);
        Box("FrontWall_B",root,new Vector3(1.5f,H/2,0),new Vector3(1.4f,H,T),panel);
        Box("FrontWall_Lintel",root,new Vector3(0,2.7f,0),new Vector3(1.6f,0.6f,T),panel);
        Box("DoorGlow_Top",root,new Vector3(0,2.42f,0.11f),new Vector3(1.7f,0.05f,0.04f),cyan,false);
        Box("DoorGlow_L",root,new Vector3(-0.83f,1.2f,0.11f),new Vector3(0.05f,2.4f,0.04f),cyan,false);
        Box("DoorGlow_R",root,new Vector3(0.83f,1.2f,0.11f),new Vector3(0.05f,2.4f,0.04f),cyan,false);
        // Ceiling light strips + one soft interior light
        Box("CeilingStrip_L",root,new Vector3(-1.4f,H-0.02f,D/2),new Vector3(0.06f,0.04f,D-0.4f),violet,false);
        Box("CeilingStrip_R",root,new Vector3(1.4f,H-0.02f,D/2),new Vector3(0.06f,0.04f,D-0.4f),violet,false);
        var lg=new GameObject("CabinLight");lg.transform.SetParent(root,false);lg.transform.localPosition=new Vector3(0,2.5f,D/2);
        var l=lg.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(0.75f,0.6f,1f);l.intensity=1.6f;l.range=6;l.shadows=LightShadows.None;
        // Docking tube toward the parked spaceship (outside, decoration only)
        Box("DockingTube",root,new Vector3(-W/2-1.8f,1.3f,D/2),new Vector3(3.6f,1.8f,1.8f),silver,false);
        Box("DockingTubeRing",root,new Vector3(-W/2-0.4f,1.3f,D/2),new Vector3(0.2f,2.1f,2.1f),cyan,false);

        // Hatch: an empty parent so the panel and its text slide together.
        var hatch=new GameObject("Hatch").transform;hatch.SetParent(root,false);hatch.localPosition=Vector3.zero;
        Box("HatchPanel",hatch,new Vector3(0,1.2f,0),new Vector3(1.6f,2.4f,0.12f),silver);
        Box("HatchStripe",hatch,new Vector3(0,1.2f,0.07f),new Vector3(0.05f,2.2f,0.02f),cyan,false);
        Text("WelcomeText",hatch,new Vector3(0,1.45f,0.08f),
            "<b>WELCOME, VISITOR</b>\n\nOutside this hatch is a museum the humans built about themselves: how their machines changed their lives, and how those machines carried them off their planet.",
            1.1f,new Vector2(1.4f,1.4f),new Color(0.85f,0.95f,1f));
        var prompt=Text("HatchPrompt",root,new Vector3(0,0.45f,0.2f),"<b>[E]</b>  OPEN HATCH",1.4f,new Vector2(1.6f,0.4f),new Color(1f,0.75f,0.45f));

        var door=root.gameObject.AddComponent<MuseumSlidingDoor>();
        door.door=hatch;door.openOffset=new Vector3(1.45f,0,0);door.range=2.6f;door.prompt=prompt;door.visitor=visitor.transform;
        prompt.gameObject.SetActive(false);

        // The cabin's front wall replaces the old glass guard at the bridge end.
        var guard=GameObject.Find("ArrivalEndGuard");if(guard!=null){Undo.RecordObject(guard,"hide");guard.SetActive(false);}

        // Start the visitor inside the cabin, facing the hatch.
        Undo.RecordObject(visitor.transform,"spawn");
        visitor.transform.position=root.TransformPoint(new Vector3(0,0.08f,3.0f));
        visitor.transform.rotation=root.rotation*Quaternion.Euler(0,180,0);

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Airlock built: the visitor now starts inside the ship. Press E at the hatch to open it.");
    }
}
