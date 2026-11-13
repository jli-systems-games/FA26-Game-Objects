using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// 1) Spaceship: explicit size and position (no automatic bounds) so the hull swallows the cabin.
//    Numbers below are easy to tweak: ShipScale = overall size, HullFront = how far the nose comes toward the bridge.
// 2) Turntable: Aphex Twin "#21" becomes record 1 and plays by default.
// 3) Story: the museum was built after humans went extinct (start screen + welcome text).
public static class MuseumStoryAndShipFixTool
{
    const float ShipScale=6f;      // model is ~2.8 x 1.4 x 2.7 units -> ~17 x 8 x 16 m
    const float HullFront=-1.5f;   // metres past the hatch toward the bridge (negative = covers the door frame)
    const float FloorAt=0.35f;     // cabin floor sits at 35% of the hull height

    // Model bounds in its own units (read from AlienSpaceship.glb).
    static readonly Vector3 ModelMin=new Vector3(-1.42f,0.25f,-1.33f),ModelMax=new Vector3(1.41f,1.62f,1.38f);

    [MenuItem("Tools/Museum/Fix Ship Position + Aphex First + Extinction Story")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeStoryFix.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // ----- 1. Ship -----
        var a=GameObject.Find("08_ShipAirlock")?.transform;var root=GameObject.Find("Visitor_Spaceship")?.transform;
        var model=root!=null&&root.childCount>0?root.GetChild(0):null;
        if(a!=null&&root!=null&&model!=null){
            if(root.parent!=a)Undo.SetTransformParent(root,a,"ship");
            Undo.RecordObject(root,"ship");Undo.RecordObject(model,"ship");
            model.localPosition=Vector3.zero;model.localRotation=Quaternion.identity;model.localScale=Vector3.one*ShipScale;
            root.localRotation=Quaternion.identity;root.localScale=Vector3.one;
            Vector3 min=ModelMin*ShipScale,max=ModelMax*ShipScale;
            float height=max.y-min.y;
            root.localPosition=new Vector3(-(min.x+max.x)/2,-FloorAt*height-min.y,HullFront-min.z);
        } else Debug.LogError("Need 08_ShipAirlock and Visitor_Spaceship with its model child.");

        // ----- 2. Aphex Twin first -----
        var ex=GameObject.Find("Exhibit_00_RecordPlayer")?.GetComponent<MuseumExhibit>();
        if(ex!=null && ex.musicSource!=null){
            int i=System.Array.FindIndex(ex.modes,m=>m.music!=null && m.music.name.Contains("Aphex"));
            if(i>0){
                Undo.RecordObject(ex,"order");var list=new System.Collections.Generic.List<MuseumExhibit.Mode>(ex.modes);
                var aphex=list[i];list.RemoveAt(i);list.Insert(0,aphex);ex.modes=list.ToArray();
                for(int k=0;k<ex.modes.Length;k++){ // renumber captions: "Record N — ... Press N again to stop."
                    var c=ex.modes[k].caption;int dash=c.IndexOf('—');int press=c.LastIndexOf("Press ");
                    if(dash>0&&press>dash)ex.modes[k].caption="Record "+(k+1)+" "+c.Substring(dash,press-dash)+"Press "+(k+1)+" again to stop.";
                }
            }
            Undo.RecordObject(ex.musicSource,"default");ex.musicSource.clip=ex.modes[0].music;ex.musicSource.playOnAwake=true;
        }

        // ----- 3. Story text -----
        Set("MuseumMenu/StartScreen/Subtitle","a museum about humans, built after they were gone");
        Set("MuseumMenu/StartScreen/Intro",
            "You are an alien visitor. The humans are extinct.\n\nAfter they were gone, this glass museum was built in orbit from the things they left behind. "+
            "It tells their story through their machines: how technology moved into their pockets, onto their bodies, and finally carried them off their planet.");
        Set("AirlockWelcome/Text",
            "<size=150%><b>WELCOME, VISITOR</b></size>\n\nThe humans are gone. This museum was built after their extinction, from what they left behind: "+
            "how their machines changed their lives, and how those machines carried them off their planet.");

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Ship moved forward and enlarged; Aphex Twin #21 is record 1 and plays first; story text updated.");
    }

    static void Set(string path,string text)
    {
        var canvas=GameObject.Find("Canvas");var t=canvas!=null?canvas.transform.Find(path):null;
        var tmp=t!=null?t.GetComponent<TMP_Text>():null;
        if(tmp==null){Debug.LogWarning("Text not found: Canvas/"+path);return;}
        Undo.RecordObject(tmp,"story");tmp.text=text;
    }
}
