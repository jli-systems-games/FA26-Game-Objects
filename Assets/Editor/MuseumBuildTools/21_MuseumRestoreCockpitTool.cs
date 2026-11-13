using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The Boeing 757 cockpit model had been removed from Exhibit_09_Cockpit757 (only its label, ring and collider were left).
// This puts the model back on the same spot, 3.6 m long, standing on the floor, and resizes the collider to match.
public static class MuseumRestoreCockpitTool
{
    [MenuItem("Tools/Museum/Restore Cockpit Model")]
    public static void Restore()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var ex=GameObject.Find("Exhibit_09_Cockpit757");var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/Cockpit757.glb");
        if(ex==null||asset==null){Debug.LogError("Need Exhibit_09_Cockpit757 and Cockpit757.glb");return;}
        if(ex.transform.Find("Cockpit757")!=null){Debug.Log("Cockpit model is already there.");return;}
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,ex.transform);Undo.RegisterCreatedObjectUndo(g,"cockpit");
        g.name="Cockpit757";g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
        var b=B(g);float max=Mathf.Max(b.size.x,b.size.y,b.size.z);if(max>0.0001f)g.transform.localScale*=3.6f/max;
        b=B(g);var p=ex.transform.position;g.transform.position+=new Vector3(p.x-b.center.x,p.y-b.min.y,p.z-b.center.z);
        var box=ex.GetComponent<BoxCollider>();
        if(box!=null){b=B(g);Undo.RecordObject(box,"collider");box.center=ex.transform.InverseTransformPoint(b.center);
            var s=ex.transform.InverseTransformVector(b.size);box.size=new Vector3(Mathf.Abs(s.x),Mathf.Abs(s.y),Mathf.Abs(s.z));}
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Cockpit model restored ("+b.size.ToString("0.0")+" m).");
    }

    // After you rotate the Cockpit757 model by hand: put it back on the floor, centred on its spot, and refit the collider.
    [MenuItem("Tools/Museum/Re-seat Cockpit After Rotating")]
    public static void Reseat()
    {
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var ex=GameObject.Find("Exhibit_09_Cockpit757");var m=ex!=null?ex.transform.Find("Cockpit757"):null;
        if(m==null){Debug.LogError("Cockpit757 model not found");return;}
        Undo.RecordObject(m,"reseat");
        var b=B(m.gameObject);var p=ex.transform.position;m.position+=new Vector3(p.x-b.center.x,p.y-b.min.y,p.z-b.center.z);
        var box=ex.GetComponent<BoxCollider>();
        if(box!=null){b=B(m.gameObject);Undo.RecordObject(box,"collider");box.center=ex.transform.InverseTransformPoint(b.center);
            var s=ex.transform.InverseTransformVector(b.size);box.size=new Vector3(Mathf.Abs(s.x),Mathf.Abs(s.y),Mathf.Abs(s.z));}
        EditorSceneManager.MarkSceneDirty(ex.scene);EditorSceneManager.SaveScene(ex.scene);
        Debug.Log("Cockpit re-seated on the floor; collider refitted.");
    }
    static Bounds B(GameObject g){var b=new Bounds();bool f=true;foreach(var r in g.GetComponentsInChildren<Renderer>()){if(f){b=r.bounds;f=false;}else b.Encapsulate(r.bounds);}return b;}
}
