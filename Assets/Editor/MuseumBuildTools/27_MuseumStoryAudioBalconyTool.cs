using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// 1) Story: humans are extinct; every object was collected by the aliens. All labels rewritten in the past tense.
// 2) Audio: footsteps (Starter Assets sounds, muted while floating in the spacesuit) + a looping space ambience.
// 3) Balcony: a viewing deck through a new opening in the north-west glass wall, with a last exhibit, "Where Earth Was".
// Nothing is deleted: the old glass wall is hidden, old texts are overwritten only on the label fields.
public static class MuseumStoryAudioBalconyTool
{
    struct Label { public string title, meta, body, theme; }

    static readonly Dictionary<string,Label> Labels=new Dictionary<string,Label>{
        {"Exhibit_00_RecordPlayer",new Label{title="Turntable: Sounds of Earth",
            meta="Recovered from a human dwelling  ·  Humans sent records into space in 1977",
            body="Humans stored their music as grooves cut into vinyl discs, read back by a needle. To listen, a human needed both the object and a machine to play it. Later the same songs travelled invisibly to the phones they carried.\n\nIn 1977 they sent two gold-plated records into space on Voyager 1 and 2, carrying greetings, sounds and music, in case anyone ever found them. We did.",
            theme="Our collection begins with the humans' own message to the stars. Choose a record: it keeps playing while you explore what else they left behind."}},
        {"Exhibit_08_Synthesizer",new Label{title="Modular Synthesizer",
            meta="Recovered from a recording studio  ·  Modular synthesizers spread in the 1960s",
            body="A synthesizer made sound from electricity instead of strings, reeds or skins. Oscillators created waves, filters shaped them, and patch cables connected the modules in endless combinations.\n\nWhen Robert Moog and others began selling modular systems in the 1960s, these machines moved from laboratories into studios. Electronic sound became the sound humans imagined for their own future, for robots and for space.",
            theme="The turntable stored human music; the synthesizer invented sounds that had never existed. Their technology did not only record their lives, it changed what those lives sounded like."}},
        {"Exhibit_01_iPhone",new Label{title="iPhone (1st generation)",
            meta="Recovered in great numbers  ·  Apple Inc.  ·  Released 29 June 2007 (US)",
            body="We found more of these than any other human object. The first iPhone combined a phone, a music player and an internet device in one slab of glass. Instead of a keypad it used a 3.5-inch multi-touch screen, so its controls were software that could change after it was bought.\n\nWhen the App Store opened in July 2008, the device began absorbing other objects: the camera, the paper map, the music player, the flashlight and eventually the wallet. Most humans carried one everywhere; directions, payment, photos and conversation all passed through it.",
            theme="Our route follows human technology as it left the desk, moved into the pocket, onto the body, and finally off their planet. The small chips, batteries and sensors made for phones later flew in space (stage 3)."}},
        {"Exhibit_02_MacBook",new Label{title="MacBook Pro (laptop)",
            meta="Recovered from a café table  ·  First MacBook Pro announced January 2006",
            body="A laptop folded a whole computer (screen, keyboard, battery and processor) into something a human could carry under one arm. Work, study and art no longer needed a fixed desk: a café, a train or a bed could become an office or a studio.",
            theme="After the phone in the pocket came the computer in the bag. Their technology began following them wherever they went, instead of humans going to the machine."}},
        {"Exhibit_03_Watch",new Label{title="On the Wrist: Wearables",
            meta="Recovered from a human wrist  ·  Apple Watch released April 2015",
            body="Smartwatches moved the computer from the pocket onto the skin. Their optical sensors read a human's pulse, motion sensors counted steps and detected falls, and some could record a simple electrocardiogram. The device no longer only served its owner; it constantly measured them.",
            theme="Here the line between tool and body began to blur. Measuring a body was also the first step to keeping one alive away from their planet."}},
        {"Exhibit_04_SpaceSuit",new Label{title="The Suit as a Spacecraft",
            meta="Recovered in orbit  ·  First moonwalk: Apollo 11, 20 July 1969",
            body="A spacesuit was the most personal machine humans ever built. It supplied oxygen, removed carbon dioxide, held pressure, controlled temperature and shielded its wearer from radiation and dust: a tiny spacecraft shaped like a human.\n\nThe suit beside this plinth is complete. Choose \"Try on the suit\" to see how it fits a body that is not human.",
            theme="Wearables measured the body; the suit replaced the planet around it. To leave Earth, humans had to carry a piece of it with them."}},
        {"Exhibit_05_ISS",new Label{title="International Space Station",
            meta="Recovered in orbit  ·  First module launched November 1998  ·  Crewed continuously from November 2000",
            body="The International Space Station was a laboratory the size of a football field, orbiting about 400 km above Earth, built together by the USA, Russia, Europe, Japan and Canada. Humans lived on board without a break for many years, testing how bodies, plants and machines coped with months of weightlessness.",
            theme="The station was their rehearsal room: the first place where living off the planet became a daily routine rather than a short trip."}},
        {"Exhibit_06_Lander",new Label{title="Landing on Mars",
            meta="Stylised reconstruction  ·  First successful Mars landing: Viking 1, 20 July 1976",
            body="Landing on Mars was famously hard: the air was too thin to slow a spacecraft easily, but thick enough to burn it. Human landers combined heat shields, parachutes and rockets in a few minutes they called the \"seven minutes of terror\". Every settlement they dreamed of depended on getting this right.",
            theme="Before any human could live on Mars, their machines had to arrive safely and prepare the way. This is the doorway to the final room."}},
        {"Exhibit_07_Perseverance",new Label{title="Perseverance Rover",
            meta="Recovered from Jezero Crater, Mars  ·  Landed 18 February 2021  ·  NASA/JPL-Caltech",
            body="Perseverance was a car-sized robot geologist, sent to search for signs of ancient life and to collect rock samples. On board, the MOXIE experiment turned carbon dioxide from the Martian air into breathable oxygen, a small test of how settlers might make what they needed on another world.\n\nWe found it still standing on Mars, long after its makers were gone.",
            theme="The journey that began with a phone in a human hand ends here: the same chips, cameras and sensors, working on another planet. Their machines travelled further than they did."}},
        {"Exhibit_09_Cockpit757",new Label{title="Boeing 757 Flight Deck",
            meta="Recovered from an airfield  ·  Boeing 757 first flew in 1982, entered service 1983",
            body="The 757 was one of the first airliners designed from the start for a crew of two. Alongside classic dials, its flight deck used colour screens to show attitude, navigation and engine data, with computers sharing the work of flying.\n\nAeroplanes like this made crossing an ocean an ordinary trip. For most humans, flying was the closest they ever came to leaving the ground.",
            theme="Before rockets, the aeroplane made the sky routine. The same story as the phone (buttons and dials turning into screens, humans trusting computers) continued on the way to space."}},
    };

    [MenuItem("Tools/Museum/Extinction Story + Footsteps + Ambience + Balcony")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeStoryBalcony.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // ----- 1. Story -----
        foreach(var ex in Object.FindObjectsByType<MuseumExhibit>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            if(Labels.TryGetValue(ex.name,out var l)){Undo.RecordObject(ex,"label");ex.title=l.title;ex.meta=l.meta;ex.body=l.body;ex.themeLink=l.theme;}
        SetText("MuseumMenu/StartScreen/Subtitle","a museum about humans, collected after they were gone");
        SetText("MuseumMenu/StartScreen/Intro","You are an alien visitor. The humans are extinct.\n\nWe collected what they left behind, from the pockets of their clothes to the surface of Mars, and built this glass museum in orbit to remember them.");
        SetText("AirlockWelcome/Text","<size=150%><b>WELCOME, VISITOR</b></size>\n\nThe humans are gone. We gathered their machines from the ruins of their planet and the edges of their solar system, and brought them here. Step through the hatch to see how their technology changed their lives, and how it carried them off their planet.");

        // ----- 2. Audio -----
        var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        if(visitor!=null){
            Undo.RecordObject(visitor,"steps");
            if(visitor.footstepSource==null){var src=Undo.AddComponent<AudioSource>(visitor.gameObject);src.playOnAwake=false;src.spatialBlend=0;visitor.footstepSource=src;}
            var clips=new List<AudioClip>();
            for(int i=1;i<=10;i++){var c=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Starter Assets/Runtime/ThirdPersonController/Character/Sfx/Player_Footstep_"+i.ToString("00")+".wav");if(c!=null)clips.Add(c);}
            visitor.footstepClips=clips.ToArray();
            visitor.landClip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Starter Assets/Runtime/ThirdPersonController/Character/Sfx/Player_Land.wav");
        }
        if(GameObject.Find("SpaceAmbience")==null){
            var amb=new GameObject("SpaceAmbience");Undo.RegisterCreatedObjectUndo(amb,"amb");
            var a=amb.AddComponent<AudioSource>();a.clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/MuseumAudio/Ambience_StarsBlinking.mp3");
            a.loop=true;a.playOnAwake=true;a.volume=0.18f;a.spatialBlend=0;   // quiet bed under the turntable music
        }

        // ----- 3. Balcony on the north-west wall (edge from (-12,0,10) to (-4,0,18)) -----
        var arch=GameObject.Find("SpaceMuseum_Architecture");
        if(arch!=null && arch.transform.Find("11_Balcony")==null) BuildBalcony(arch.transform);

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Story rewritten (extinct humans, collected by aliens); footsteps + ambience added; balcony built. Re-bake lighting: Window > Rendering > Lighting > Generate Lighting.");
    }

    static void BuildBalcony(Transform arch)
    {
        Material M(string n)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/"+n+".asset");
        var dark=M("Frame_Obsidian");var silver=M("Frame_Titanium");var rose=M("Glass_Rose");var ice=M("Glass_Ice");var cyan=M("Light_Cyan");var violet=M("Light_Violet");var panel=M("Panel_Gradient_Ribbed");
        Vector3 a=new Vector3(-12,0,10),b=new Vector3(-4,0,18),mid=(a+b)/2,outward=new Vector3(-1,0,1).normalized;
        var root=new GameObject("11_Balcony").transform;Undo.RegisterCreatedObjectUndo(root.gameObject,"balcony");
        root.SetParent(arch,false);root.localPosition=mid;root.localRotation=Quaternion.LookRotation(outward); // local X runs along the wall
        float half=Vector3.Distance(a,b)/2;const float H=7.4f,gap=1.5f;

        // Replace the single glass pane with two panes and an opening.
        var oldPane=arch.Find("01_GlassAndFrames/GlassWall_2");if(oldPane!=null){Undo.RecordObject(oldPane.gameObject,"hide");oldPane.gameObject.SetActive(false);}
        float w=half-gap;
        Box("Pane_A",root,new Vector3(-(gap+w/2),H/2,0),new Vector3(w,H,0.09f),rose,true);
        Box("Pane_B",root,new Vector3(gap+w/2,H/2,0),new Vector3(w,H,0.09f),rose,true);
        Box("Transom",root,new Vector3(0,(3f+H)/2,0),new Vector3(gap*2,H-3f,0.09f),rose,false);
        Box("Post_A",root,new Vector3(-gap,1.5f,0),new Vector3(0.14f,3f,0.18f),silver,true);
        Box("Post_B",root,new Vector3(gap,1.5f,0),new Vector3(0.14f,3f,0.18f),silver,true);
        Box("Lintel_Glow",root,new Vector3(0,3.02f,0),new Vector3(gap*2,0.05f,0.2f),cyan,false);

        // Deck + glass railing (colliders keep the visitor on the deck).
        const float DW=7f,DD=5.2f;
        Box("Deck",root,new Vector3(0,-0.25f,DD/2),new Vector3(DW,0.5f,DD),dark,true);
        Box("Deck_Inlay",root,new Vector3(0,0.005f,DD/2),new Vector3(DW-0.6f,0.01f,DD-0.6f),panel,false);
        Box("Rail_Left",root,new Vector3(-DW/2,0.6f,DD/2),new Vector3(0.06f,1.2f,DD),ice,true);
        Box("Rail_Right",root,new Vector3(DW/2,0.6f,DD/2),new Vector3(0.06f,1.2f,DD),ice,true);
        Box("Rail_Front",root,new Vector3(0,0.6f,DD),new Vector3(DW,1.2f,0.06f),ice,true);
        Box("Handrail_Front",root,new Vector3(0,1.22f,DD),new Vector3(DW,0.05f,0.08f),silver,false);
        Box("Handrail_Left",root,new Vector3(-DW/2,1.22f,DD/2),new Vector3(0.08f,0.05f,DD),silver,false);
        Box("Handrail_Right",root,new Vector3(DW/2,1.22f,DD/2),new Vector3(0.08f,0.05f,DD),silver,false);
        Box("EdgeGlow_Front",root,new Vector3(0,0.03f,DD-0.05f),new Vector3(DW,0.04f,0.04f),violet,false);
        Box("EdgeGlow_Left",root,new Vector3(-DW/2+0.05f,0.03f,DD/2),new Vector3(0.04f,0.04f,DD),cyan,false);
        Box("EdgeGlow_Right",root,new Vector3(DW/2-0.05f,0.03f,DD/2),new Vector3(0.04f,0.04f,DD),cyan,false);
        Box("Bench",root,new Vector3(0,0.23f,1.8f),new Vector3(2.4f,0.46f,0.55f),silver,true);

        var lg=new GameObject("BalconyLight");lg.transform.SetParent(root,false);lg.transform.localPosition=new Vector3(0,3f,DD/2);
        var l=lg.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(0.75f,0.65f,1f);l.intensity=2.5f;l.range=8;l.lightmapBakeType=LightmapBakeType.Baked;

        // A tiny blue dot out in space: "where Earth was".
        var dot=GameObject.CreatePrimitive(PrimitiveType.Sphere);dot.name="PaleBlueDot";Object.DestroyImmediate(dot.GetComponent<Collider>());
        dot.transform.SetParent(root,false);dot.transform.localPosition=new Vector3(0,4f,45f);dot.transform.localScale=Vector3.one*0.35f;
        var dm=new Material(Shader.Find("Universal Render Pipeline/Unlit")){name="PaleBlueDot"};dm.SetColor("_BaseColor",new Color(0.55f,0.8f,1f));
        AssetDatabase.CreateAsset(dm,"Assets/MuseumGenerated/PaleBlueDot.mat");dot.GetComponent<Renderer>().sharedMaterial=dm;

        // Final exhibit: a plaque at the railing.
        var stand=Box("Plaque_Stand",root,new Vector3(0,0.5f,DD-0.7f),new Vector3(0.9f,1f,0.35f),dark,true);
        Box("Plaque_Face",root,new Vector3(0,1.02f,DD-0.75f),new Vector3(0.85f,0.04f,0.4f),cyan,false).transform.localRotation=Quaternion.Euler(-25,0,0);
        var exGO=new GameObject("Exhibit_10_WhereEarthWas");exGO.transform.SetParent(root,false);exGO.transform.localPosition=new Vector3(0,0,DD-0.7f);
        var ex=exGO.AddComponent<MuseumExhibit>();ex.interactRange=2.6f;ex.modes=new MuseumExhibit.Mode[0];
        ex.title="Where Earth Was";
        ex.meta="Viewing deck  ·  Voyager 1 photographed Earth from about 6 billion km on 14 February 1990";
        ex.body="From here you can look back toward the humans' home system: find the small blue light beyond the railing.\n\nIn 1990, as Voyager 1 left the planets behind, its cameras turned around and photographed Earth as a single pale blue point, smaller than a pixel. The astronomer Carl Sagan called it a \"pale blue dot\": every human who ever lived, on one speck of light.";
        ex.themeLink="The museum ends looking outward, the way the humans' last machines did. Everything in these rooms came from that dot.";
        ex.credit="Balcony and plaque built from Unity primitives. Fact source: NASA, Voyager 1 \"Pale Blue Dot\" image (1990).";
        var ring=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ring.name="FocusRing";Object.DestroyImmediate(ring.GetComponent<Collider>());
        ring.transform.SetParent(exGO.transform,false);ring.transform.localPosition=new Vector3(0,0.006f,-0.6f);ring.transform.localScale=new Vector3(1.6f,0.003f,1.2f);
        ring.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");ring.SetActive(false);ex.focusHighlight=ring;

        var flags=StaticEditorFlags.ContributeGI|StaticEditorFlags.BatchingStatic|StaticEditorFlags.ReflectionProbeStatic;
        foreach(var r in root.GetComponentsInChildren<MeshRenderer>(true))
            if(r.sharedMaterial!=null && r.sharedMaterial.renderQueue<3000 && r.gameObject!=dot && r.transform.parent!=exGO.transform)GameObjectUtility.SetStaticEditorFlags(r.gameObject,flags);
    }

    static GameObject Box(string name,Transform p,Vector3 pos,Vector3 size,Material m,bool collider)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(p,false);
        g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;
        if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
    }
    static void SetText(string path,string text)
    {
        var c=GameObject.Find("Canvas");var t=c!=null?c.transform.Find(path):null;var tmp=t!=null?t.GetComponent<TMP_Text>():null;
        if(tmp==null){Debug.LogWarning("Text not found: Canvas/"+path);return;}Undo.RecordObject(tmp,"story");tmp.text=text;
    }
}
