using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Places the three Sketchfab models the student chose (see MuseumCredits.md):
//  - Turntable with speakers replaces the small record player (same exhibit, same records; old model hidden)
//  - Modular synthesizer: second "sound" exhibit in the lobby
//  - Boeing 757 cockpit: large floor exhibit between the lobby screen and the central podium
// Positions are in Museum-local metres; move them by hand if they feel crowded.
public static class MuseumSketchfabModelsTool
{
    [MenuItem("Tools/Museum/Place Turntable, Synth and Cockpit")]
    public static void Place()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var museum=GameObject.Find("Museum");var arch=GameObject.Find("SpaceMuseum_Architecture");
        if(museum==null||arch==null){Debug.LogError("Museum not found");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforeSketchfab.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        Transform space=arch.transform;Vector3 door=new Vector3(4,0,-12);

        // ----- Turntable replaces the record player -----
        var rp=GameObject.Find("Exhibit_00_RecordPlayer");
        if(rp!=null && rp.transform.Find("Turntable")==null){
            var t=Load("Turntable");
            if(t!=null){
                Undo.RecordObject(rp.transform,"move");
                Vector3 local=new Vector3(-1.5f,0,-12.5f);
                rp.transform.position=space.TransformPoint(local);
                rp.transform.rotation=space.rotation*Quaternion.LookRotation(door-local);
                var old=rp.transform.Find("RecordPlayer");if(old!=null){Undo.RecordObject(old.gameObject,"hide");old.gameObject.SetActive(false);}
                var g=Fit(t,rp.transform,2.4f);g.name="Turntable";Solid(rp,g);
                var ring=rp.transform.Find("FocusRing");if(ring!=null)ring.localScale=new Vector3(3f,0.003f,2f);
                var e=rp.GetComponent<MuseumExhibit>();
                if(e!=null){Undo.RecordObject(e,"credit");e.interactRange=3.2f;e.title="Turntable: Sounds of Earth";
                    e.credit="Model: \"Turntable with stands and speakers\" by charliebrown, CC BY 4.0, via Sketchfab. Music (CC0, OpenGameArt): Starfield Romance – Yoiyami; Steller Dreams – Synth-thetic; Floating in Space – Umplix.";}
            }
        }

        // ----- Synthesizer -----
        Add(space,"Exhibit_08_Synthesizer","Synthesizer",new Vector3(-5.5f,0,-11f),door,2.0f,
            "Modular Synthesizer","Electronic instrument  ·  Modular synthesizers spread in the mid-1960s",
            "A synthesizer makes sound from electricity instead of strings, reeds or skins. Oscillators create waves, filters shape them, and patch cables connect the modules in endless combinations.\n\n"+
            "When Robert Moog and others began selling modular systems in the 1960s, these machines moved from laboratories into studios. Electronic sound soon became the sound people imagined for the future, for robots and for space.",
            "The turntable stores human music; the synthesizer invents sounds that never existed. Technology does not only record our lives — it changes what they sound like.",
            "Model: \"Gary's Synthesizer\" by Fredrik (kird3rf), CC BY 4.0, via Sketchfab.");

        // ----- Cockpit -----
        Add(space,"Exhibit_09_Cockpit757","Cockpit757",new Vector3(0,0,-3.4f),new Vector3(0,0,-12),3.6f,
            "Boeing 757 Flight Deck","Airliner cockpit  ·  Boeing 757 first flew in 1982, entered service 1983",
            "The 757 was one of the first airliners designed from the start for a crew of two. Alongside classic dials, its flight deck used colour screens to show attitude, navigation and engine data, with computers sharing the work of flying.\n\n"+
            "Aeroplanes like this made crossing oceans an ordinary weekend trip. For most people, flying is the closest they will come to leaving the ground.",
            "Before rockets, the aeroplane made the sky routine. The same story you saw on the phone — buttons and dials turning into screens, humans trusting computers — continues on the way to space.",
            "Model: \"Boeing 757-200 Icelandair Cockpit\" by VTX, CC BY-NC-SA 4.0, via Sketchfab. Used non-commercially in a student project.");

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Turntable, synthesizer and cockpit placed. Check rotations in the Scene view.");
    }

    static void Add(Transform space,string id,string file,Vector3 local,Vector3 lookAt,float size,string title,string meta,string body,string theme,string credit)
    {
        if(GameObject.Find(id)!=null)return;var asset=Load(file);if(asset==null){Debug.LogError(file+".glb missing");return;}
        var root=new GameObject(id);Undo.RegisterCreatedObjectUndo(root,id);root.transform.SetParent(space,false);
        root.transform.localPosition=local;root.transform.localRotation=Quaternion.LookRotation(lookAt-local);
        var g=Fit(asset,root.transform,size);Solid(root,g);
        var e=root.AddComponent<MuseumExhibit>();e.title=title;e.meta=meta;e.body=body;e.themeLink=theme;e.credit=credit;
        e.modes=new MuseumExhibit.Mode[0];e.interactRange=size+0.8f;
        var ring=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ring.name="FocusRing";Object.DestroyImmediate(ring.GetComponent<Collider>());
        ring.transform.SetParent(root.transform,false);ring.transform.localPosition=new Vector3(0,0.006f,0);ring.transform.localScale=new Vector3(size*1.2f,0.003f,size*1.2f);
        ring.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");
        ring.SetActive(false);e.focusHighlight=ring;
    }
    static GameObject Load(string f)=>AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/"+f+".glb");
    static Bounds B(GameObject g){var rs=g.GetComponentsInChildren<Renderer>();var b=rs.Length>0?rs[0].bounds:new Bounds(g.transform.position,Vector3.zero);foreach(var r in rs)b.Encapsulate(r.bounds);return b;}
    // Largest side = size, standing on the parent's origin.
    static GameObject Fit(GameObject asset,Transform parent,float size)
    {
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,parent);g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
        var b=B(g);float max=Mathf.Max(b.size.x,b.size.y,b.size.z);if(max>0.0001f)g.transform.localScale*=size/max;
        b=B(g);g.transform.position+=new Vector3(parent.position.x-b.center.x,parent.position.y-b.min.y,parent.position.z-b.center.z);return g;
    }
    // One box collider around the model so the visitor cannot walk through it.
    static void Solid(GameObject root,GameObject model)
    {
        var b=B(model);var box=root.GetComponent<BoxCollider>();if(box==null)box=root.AddComponent<BoxCollider>();
        box.center=root.transform.InverseTransformPoint(b.center);
        var s=root.transform.InverseTransformVector(b.size);box.size=new Vector3(Mathf.Abs(s.x),Mathf.Abs(s.y),Mathf.Abs(s.z));
    }
}
