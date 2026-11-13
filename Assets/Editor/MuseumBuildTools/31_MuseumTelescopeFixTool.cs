using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// Telescope fixes:
//  - Earth was 120 m away: the distance fog washed it into a blue blur. Now 40 m away and smaller,
//    so it is still a tiny dot to the naked eye but clear in the telescope (zoom recomputed, Earth spins slowly).
//  - Telescope moved to the right-hand railing of the balcony.
//  - In-game hints no longer tell you to press Esc (in a browser Esc exits fullscreen); E or Q close things.
public static class MuseumTelescopeFixTool
{
    [MenuItem("Tools/Museum/Fix Telescope + Earth + Esc Hints")]
    public static void Fix()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var balcony=GameObject.Find("11_Balcony")?.transform;
        var scope=balcony!=null?balcony.Find("Telescope"):null;var earth=balcony!=null?balcony.Find("Earth"):null;
        if(scope==null||earth==null){Debug.LogError("Run the telescope tool (29) first.");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        Undo.RecordObject(earth,"earth");earth.localPosition=new Vector3(0,4.5f,40f);earth.localScale=Vector3.one*0.45f;earth.localRotation=Quaternion.Euler(-20,0,0);
        Undo.RecordObject(scope,"scope");scope.localPosition=new Vector3(3.0f,0,3.1f);   // against the right railing
        var head=scope.Find("Head");if(head!=null){Undo.RecordObject(head,"aim");head.rotation=Quaternion.LookRotation(earth.position-head.position);}
        var t=scope.GetComponent<MuseumTelescope>();if(t!=null){Undo.RecordObject(t,"zoom");t.zoomFov=0.95f;t.lookAround=0.6f;t.earthSpin=8f;}

        // Hints: E (or Q) instead of Esc.
        Replace("CloseButton","CLOSE  [E / ESC]","CLOSE  [E]");
        Replace("KeysHint","E close","E / Q close");
        Replace("Caption/Text","[E] step back","[E] or [Q] step back");

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Earth moved closer (clear in the telescope), telescope at the railing, Esc removed from hints.");
    }
    static void Replace(string objectPath,string from,string to)
    {
        foreach(var tmp in Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
            string path=tmp.transform.parent!=null?tmp.transform.parent.name+"/"+tmp.name:tmp.name;
            if((tmp.name==objectPath||path.EndsWith(objectPath)||tmp.transform.parent!=null&&tmp.transform.parent.name==objectPath)&&tmp.text.Contains(from)){
                Undo.RecordObject(tmp,"hint");tmp.text=tmp.text.Replace(from,to);}
        }
    }
}
