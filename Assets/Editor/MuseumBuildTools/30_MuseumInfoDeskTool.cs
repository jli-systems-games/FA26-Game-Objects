using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The lobby desk (LobbyInformationPlinth) became empty when the turntable moved to the floor.
// This turns it into the museum's Information Desk: a small glowing hologram on top and an E panel that
// introduces the museum and lists the route through the rooms. It strengthens the "lobby" requirement.
public static class MuseumInfoDeskTool
{
    [MenuItem("Tools/Museum/Lobby Information Desk")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var desk=GameObject.Find("LobbyInformationPlinth");
        if(desk==null){Debug.LogError("LobbyInformationPlinth not found");return;}
        if(GameObject.Find("Exhibit_00b_InfoDesk")!=null){Debug.Log("Info desk already exists.");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var top=desk.GetComponent<Renderer>().bounds;
        var root=new GameObject("Exhibit_00b_InfoDesk");Undo.RegisterCreatedObjectUndo(root,"desk");
        root.transform.SetParent(desk.transform.parent,false);root.transform.position=new Vector3(top.center.x,top.max.y,top.center.z);

        // Hologram: a slowly turning ring of light over the desk (uses MuseumExhibit's model spin).
        var holo=new GameObject("Hologram").transform;holo.SetParent(root.transform,false);holo.localPosition=new Vector3(0,0.45f,0);
        var cyan=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Light_Cyan.asset");
        var violet=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Light_Violet.asset");
        for(int i=0;i<3;i++){
            var r=GameObject.CreatePrimitive(PrimitiveType.Cylinder);r.name="Ring_"+i;Object.DestroyImmediate(r.GetComponent<Collider>());
            r.transform.SetParent(holo,false);r.transform.localScale=new Vector3(0.5f-i*0.12f,0.006f,0.5f-i*0.12f);
            r.transform.localRotation=Quaternion.Euler(i*35,i*60,0);r.GetComponent<Renderer>().sharedMaterial=i%2==0?cyan:violet;
        }
        var core=GameObject.CreatePrimitive(PrimitiveType.Sphere);core.name="Core";Object.DestroyImmediate(core.GetComponent<Collider>());
        core.transform.SetParent(holo,false);core.transform.localScale=Vector3.one*0.12f;core.GetComponent<Renderer>().sharedMaterial=cyan;

        var ex=root.AddComponent<MuseumExhibit>();ex.model=holo;ex.interactRange=2.6f;ex.modes=new MuseumExhibit.Mode[0];
        ex.title="Information Desk";
        ex.meta="Lobby  ·  Start here";
        ex.body="Welcome, visitor. The humans who made these objects are extinct. We collected what they left behind and arranged it as a journey: how their technology moved closer and closer to their bodies, and finally carried them off their planet.\n\n"+
            "<b>Route</b>\n"+
            "1  Lobby: the Turntable and the Synthesizer, sounds of Earth\n"+
            "2  In the pocket: iPhone, MacBook\n"+
            "3  On the body: Wearables, the Spacesuit (try it on)\n"+
            "4  Leaving the ground: the 757 Flight Deck, the Space Station\n"+
            "5  Mars: the Lander and the Perseverance rover at the centre\n"+
            "6  The balcony: Where Earth Was, with a telescope\n\n"+
            "Walk up to any glowing ring and press E to read. Allow about 15 minutes.";
        ex.themeLink="Every object here was once ordinary for a human. Together they tell how a species turned its tools into a way off its own world.";
        ex.credit="Desk and hologram built from Unity primitives.";
        var ring=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ring.name="FocusRing";Object.DestroyImmediate(ring.GetComponent<Collider>());
        ring.transform.SetParent(root.transform,false);ring.transform.position=new Vector3(top.center.x,desk.transform.parent.position.y+0.006f,top.center.z);
        ring.transform.localScale=new Vector3(2.6f,0.003f,1.8f);
        ring.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");ring.SetActive(false);ex.focusHighlight=ring;

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Lobby desk is now the Information Desk (E for intro + route).");
    }
}
