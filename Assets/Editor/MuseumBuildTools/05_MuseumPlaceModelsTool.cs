using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Places the downloaded models (Assets/MuseumModels, see MuseumCredits.md) on the plinths.
// Each new exhibit is "simple": press E to open/close its label (no interaction modes).
// Safe to run twice: exhibits that already exist are skipped. Nothing is deleted.
public static class MuseumPlaceModelsTool
{
    struct Spec
    {
        public string id, file, anchor; public float size; public Vector3 offset;
        public string title, meta, body, theme, credit;
    }

    static readonly Spec[] Specs={
        new Spec{id="Exhibit_02_MacBook",file="MacBookPro",anchor="DisplaySurface_5",size=0.9f,
            title="MacBook Pro (laptop)",meta="Apple Inc.  ·  First MacBook Pro announced January 2006",
            body="A laptop folds a whole desktop computer — screen, keyboard, battery and processor — into something you can carry under your arm. "+
                 "Work, study and making art stopped needing a fixed desk: a café, a train or a dorm bed could become an office or a studio.",
            theme="After the phone in your pocket comes the computer in your bag. Technology starts following people wherever they go, instead of people going to the machine.",
            credit="Model: \"Laptop / MacBook Pro\" by Alex Safayan, CC BY 3.0, via Poly Pizza. Simplified low-poly model."},
        new Spec{id="Exhibit_03_Watch",file="Watch",anchor="DisplaySurface_4",size=0.6f,
            title="On the Wrist: Wearables",meta="Smartwatches  ·  Apple Watch released April 2015",
            body="Smartwatches moved the computer from the pocket onto the skin. Their optical sensors read your pulse, their motion sensors count steps and detect falls, "+
                 "and some can record a simple electrocardiogram. The device no longer only serves you — it constantly measures you.",
            theme="This is the 'body and technology' room: the boundary between tool and body starts to blur. Measuring a body is also the first step to keeping one alive in space.",
            credit="Model: \"Watch\" by Poly by Google, CC BY 3.0, via Poly Pizza. A generic watch, not a specific product."},
        new Spec{id="Exhibit_04_SpaceSuit",file="SpaceHelmet",anchor="DisplaySurface_3",size=0.7f,
            title="The Suit as a Spacecraft",meta="Spacesuits  ·  First moonwalk: Apollo 11, 20 July 1969",
            body="A spacesuit is the most personal machine ever built. It supplies oxygen, removes carbon dioxide, holds pressure, controls temperature and shields its wearer from radiation and dust — "+
                 "a tiny spacecraft shaped like a human. Look down: as a visitor of this museum, you are already wearing one.",
            theme="Wearables measured the body; the suit replaces the planet around it. To leave Earth, humans have to carry a piece of Earth with them.",
            credit="Models: \"Space Helmet - Model 1b\" by Alan Zimmerman, CC BY 3.0; \"Astronaut\" by Quaternius, CC0. Both via Poly Pizza."},
        new Spec{id="Exhibit_05_ISS",file="ISS",anchor="DisplaySurface_2",size=1.5f,offset=new Vector3(0,0.35f,0),
            title="International Space Station",meta="First module launched November 1998  ·  Continuously crewed since November 2000",
            body="The ISS is a laboratory the size of a football field orbiting about 400 km above Earth, built by the USA, Russia, Europe, Japan and Canada. "+
                 "People have lived on board without a break since 2000, testing how bodies, plants and machines cope with months in weightlessness.",
            theme="The station is humanity's rehearsal room: the first place where living off the planet became a daily routine rather than a short trip.",
            credit="Model: \"International Space Station\" by Poly by Google, CC BY 3.0, via Poly Pizza. Simplified, not to scale."},
        new Spec{id="Exhibit_06_Lander",file="LanderB",anchor="DisplaySurface_1",size=0.9f,
            title="Landing on Mars",meta="Stylised lander  ·  First successful Mars landing: Viking 1, 20 July 1976",
            body="Landing on Mars is famously hard: the air is too thin to slow a spacecraft easily, but thick enough to burn it. "+
                 "Landers combine heat shields, parachutes and rockets in a few minutes known as the 'seven minutes of terror'. Every future settlement depends on getting this right.",
            theme="Before anyone can live on Mars, machines have to arrive safely and prepare the way. This is the doorway to the final room.",
            credit="Model: \"Lander B\" by Kay Lousberg, CC0, via Poly Pizza. Stylised, not a model of a real mission."},
        new Spec{id="Exhibit_07_Perseverance",file="Perseverance",anchor="CentralFeaturePodium",size=3f,
            title="Perseverance Rover",meta="NASA/JPL-Caltech  ·  Landed in Jezero Crater, 18 February 2021",
            body="Perseverance is a car-sized robot geologist searching for signs of ancient life and collecting rock samples for a future return to Earth. "+
                 "On board, the MOXIE experiment turned carbon dioxide from the Martian air into breathable oxygen — a small test of how future settlers could make what they need on site.",
            theme="The journey that started with a phone in your hand ends here: the same chips, cameras and sensors, now working on another planet to make human life there possible.",
            credit="Model: Mars Perseverance Rover 3D model, NASA/JPL-Caltech (science.nasa.gov)."},
    };

    [MenuItem("Tools/Museum/Place Downloaded Exhibit Models")]
    public static void Place()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying || scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum first.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforeModels.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var ring=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");
        int placed=0;

        foreach(var s in Specs){
            if(GameObject.Find(s.id)!=null){Debug.Log(s.id+" already exists, skipped.");continue;}
            var anchor=GameObject.Find(s.anchor);var model=Load(s.file);
            if(anchor==null||model==null){Debug.LogError("Missing "+(anchor==null?s.anchor:s.file+".glb (is glTFast installed?)"));continue;}
            var top=anchor.GetComponent<Renderer>().bounds;
            var root=new GameObject(s.id);Undo.RegisterCreatedObjectUndo(root,s.id);
            root.transform.SetParent(anchor.transform.parent,false);
            root.transform.position=new Vector3(top.center.x,top.max.y,top.center.z);
            var m=Fit(model,root.transform,s.size,s.offset);
            if(s.file=="Perseverance"){var box=root.AddComponent<BoxCollider>();var b=Bounds(m);box.center=root.transform.InverseTransformPoint(b.center);box.size=b.size;}
            var e=root.AddComponent<MuseumExhibit>();
            e.title=s.title;e.meta=s.meta;e.body=s.body;e.themeLink=s.theme;e.credit=s.credit;
            e.interactRange=s.file=="Perseverance"?4.2f:3f;e.modes=new MuseumExhibit.Mode[0];
            e.focusHighlight=Ring(root.transform,s.file=="Perseverance"?4.6f:2.2f,s.file=="Perseverance"?4.6f:1.25f,ring);
            if(s.file=="SpaceHelmet"){
                var astro=Load("Astronaut");
                if(astro!=null){var a=Fit(astro,root.transform,1.8f,Vector3.zero);a.name="Astronaut_Figure";
                    var ab=Bounds(a);a.transform.position+=new Vector3(top.max.x+0.7f-ab.center.x,anchor.transform.parent.position.y-ab.min.y,top.center.z-ab.center.z);}
            }
            placed++;
        }

        // The iPhone exhibit: show the downloaded CC0 smartphone, keep (hide) the primitive placeholder.
        var phone=GameObject.Find("Exhibit_01_iPhone");
        if(phone!=null && phone.transform.Find("Model_Smartphone")==null){
            var model=Load("Smartphone");var ex=phone.GetComponent<MuseumExhibit>();
            if(model!=null && ex!=null){
                var holder=new GameObject("Model_Smartphone").transform;Undo.RegisterCreatedObjectUndo(holder.gameObject,"phone");
                holder.SetParent(phone.transform,false);holder.localPosition=new Vector3(0,0.5f,0);
                var m=Fit(model,holder,0.75f,Vector3.zero);m.transform.localPosition=Vector3.zero;
                Undo.RecordObject(ex,"Use downloaded phone");ex.model=holder;ex.screen=null;
                ex.credit="Model: \"Smartphone\" by smallbigsquare, CC0, via Poly Pizza. Generic smartphone shown at 5x scale, not an official Apple model.";
                Hide(phone.transform.Find("PlaceholderPhone_5xScale"));Hide(phone.transform.Find("Label_Placeholder"));
            }
        }

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Placed "+placed+" downloaded exhibits. Check sizes in the Scene view; adjust each model's scale by hand if needed.");
    }


    // The visitor wears a full spacesuit: the Quaternius "Astronaut" model (CC0) replaces the visible body.
    // The Starter Assets character stays underneath (collider + skeleton) but its renderers are switched off.
    // The astronaut gets its own Animator: Idle / Walk / Run blended by "Speed", set by MuseumVisitorController.
    [MenuItem("Tools/Museum/Dress Visitor As Astronaut")]
    public static void DressVisitor()=>DressWith("Astronaut","AstronautSuit_Visual");

    // Concept: an alien visits a museum about humans. The astronaut suit goes back to being an exhibit.
    [MenuItem("Tools/Museum/Dress Visitor As Alien")]
    public static void DressAlien()
    {
        var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        if(visitor==null||EditorApplication.isPlaying)return;
        var suit=visitor.transform.Find("AstronautSuit_Visual");if(suit!=null)Hide(suit);
        DressWith("Alien","AlienBody_Visual");
        var exhibit=GameObject.Find("Exhibit_04_SpaceSuit");
        if(exhibit!=null){var fig=exhibit.transform.Find("Astronaut_Figure");
            if(fig!=null){Undo.RecordObject(fig.gameObject,"show");fig.gameObject.SetActive(true);}
            var ex=exhibit.GetComponent<MuseumExhibit>();
            if(ex!=null){Undo.RecordObject(ex,"text");
                ex.body=ex.body.Replace("Look down: as a visitor of this museum, you are already wearing one.","The figure beside this plinth wears the full suit: notice how much of a human it hides.");
                ex.credit="Models: \"Space Helmet - Model 1b\" by Alan Zimmerman, CC BY 3.0; \"Astronaut\" by Quaternius, CC0. Both via Poly Pizza.";}}
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    }

    static void DressWith(string file,string visualName)
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        var asset=Load(file);
        if(visitor==null||asset==null){Debug.LogError("Need MuseumVisitor and "+file+".glb");return;}
        if(visitor.transform.Find(visualName)!=null){Debug.Log(visualName+" already exists.");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // Earlier attempt (white mannequin + helmet): hide the helmet, keep it in the scene.
        foreach(var m in visitor.GetComponentsInChildren<MuseumHelmetMarker>(true))Hide(m.transform);
        // Hide the Starter Assets body (renderers off, nothing deleted).
        foreach(var r in visitor.GetComponentsInChildren<Renderer>()){Undo.RecordObject(r,"hide body");r.enabled=false;} // only active bodies

        // Looping copies of the clips we need (imported clips are read-only).
        var clips=new System.Collections.Generic.Dictionary<string,AnimationClip>();
        foreach(var o in AssetDatabase.LoadAllAssetsAtPath("Assets/MuseumModels/"+file+".glb"))
            if(o is AnimationClip c)foreach(var want in new[]{"Idle","Walk","Run"})
                if(c.name.EndsWith("|"+want)||c.name.EndsWith("_"+want)||c.name==want){
                    var copy=Object.Instantiate(c);copy.name=file+"_"+want;
                    var st=AnimationUtility.GetAnimationClipSettings(copy);st.loopTime=true;AnimationUtility.SetAnimationClipSettings(copy,st);
                    string cp="Assets/MuseumGenerated/"+file+"_"+want+".anim";AssetDatabase.DeleteAsset(cp);AssetDatabase.CreateAsset(copy,cp);clips[want]=copy;}
        if(clips.Count<3)Debug.LogWarning("Found clips: "+string.Join(",",clips.Keys)+" (expected Idle, Walk, Run)");

        string ctrlPath="Assets/MuseumGenerated/"+file+"Visitor.controller";AssetDatabase.DeleteAsset(ctrlPath);
        var ctrl=UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(ctrlPath);
        foreach(var n in new[]{"Speed","MotionSpeed"})ctrl.AddParameter(n,AnimatorControllerParameterType.Float);
        foreach(var n in new[]{"Grounded","Jump","FreeFall"})ctrl.AddParameter(n,AnimatorControllerParameterType.Bool);
        ctrl.CreateBlendTreeInController("Locomotion",out var tree);
        tree.blendParameter="Speed";tree.useAutomaticThresholds=false;
        if(clips.TryGetValue("Idle",out var idle))tree.AddChild(idle,0);
        if(clips.TryGetValue("Walk",out var walk))tree.AddChild(walk,visitor.walkSpeed);
        if(clips.TryGetValue("Run",out var run))tree.AddChild(run,visitor.runSpeed);

        var holder=new GameObject(visualName);Undo.RegisterCreatedObjectUndo(holder,"suit");
        holder.transform.SetParent(visitor.transform,false);
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,holder.transform);g.name=file;
        var b=Bounds(g);if(b.size.y>0.001f)holder.transform.localScale=Vector3.one*(1.8f/b.size.y);
        b=Bounds(g);g.transform.position+=new Vector3(visitor.transform.position.x-b.center.x,visitor.transform.position.y-b.min.y,visitor.transform.position.z-b.center.z);
        var anim=g.GetComponent<Animator>();if(anim==null)anim=g.AddComponent<Animator>();
        anim.runtimeAnimatorController=ctrl;anim.applyRootMotion=false;anim.cullingMode=AnimatorCullingMode.AlwaysAnimate;

        var exhibit=file=="Astronaut"?GameObject.Find("Exhibit_04_SpaceSuit"):null;
        if(exhibit!=null){foreach(var n in new[]{"Astronaut_Figure","Visitor_Figure"}){var t=exhibit.transform.Find(n);if(t!=null)Hide(t);}
            var ex=exhibit.GetComponent<MuseumExhibit>();
            if(ex!=null){Undo.RecordObject(ex,"text");
                ex.body=ex.body.Replace("The astronaut figure beside this plinth shows how the helmet fits the whole suit.","Look down: as a visitor of this museum, you are already wearing one.");
                ex.credit="Models: \"Space Helmet - Model 1b\" by Alan Zimmerman, CC BY 3.0; your suit: \"Astronaut\" by Quaternius, CC0. Both via Poly Pizza.";}}

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Visitor body is now "+file+". If it faces backwards, set MuseumVisitor/"+visualName+" rotation Y to 180.");
    }

    static GameObject Load(string file)=>AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/"+file+".glb");
    static void Hide(Transform t){if(t==null)return;Undo.RecordObject(t.gameObject,"hide");t.gameObject.SetActive(false);}
    static Bounds Bounds(GameObject g){var rs=g.GetComponentsInChildren<Renderer>();var b=rs.Length>0?rs[0].bounds:new Bounds(g.transform.position,Vector3.zero);foreach(var r in rs)b.Encapsulate(r.bounds);return b;}

    // Instantiate under parent, scale so the largest side = size, stand it on the parent's origin.
    static GameObject Fit(GameObject asset,Transform parent,float size,Vector3 offset)
    {
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,parent);g.name=asset.name;
        g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
        var b=Bounds(g);float max=Mathf.Max(b.size.x,b.size.y,b.size.z);
        if(max>0.0001f)g.transform.localScale*=size/max;
        b=Bounds(g);
        g.transform.position+=new Vector3(parent.position.x-b.center.x,parent.position.y-b.min.y,parent.position.z-b.center.z)+offset;
        return g;
    }
    static GameObject Ring(Transform parent,float x,float z,Material m)
    {
        var r=GameObject.CreatePrimitive(PrimitiveType.Cylinder);r.name="FocusRing";Object.DestroyImmediate(r.GetComponent<Collider>());
        r.transform.SetParent(parent,false);r.transform.localPosition=new Vector3(0,0.005f,0);r.transform.localScale=new Vector3(x,0.004f,z);
        if(m!=null)r.GetComponent<Renderer>().sharedMaterial=m;r.SetActive(false);return r;
    }
}
