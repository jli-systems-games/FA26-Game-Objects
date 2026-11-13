using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The downloaded spaceship is only an outer shell (no rooms inside), so the visitor still needs a small floor + walls to stand in.
// This tool hides that room INSIDE a bigger spaceship:
//  - the ship is scaled up and moved so its hull covers the whole cabin; only the hatch sits on the hull, facing the bridge
//  - the cabin is re-skinned in dark ship metal (no purple box, no glass window), so it reads as the ship's interior
// Nothing is deleted. Tweak 08_ShipAirlock/Visitor_Spaceship by hand afterwards if a corner of the cabin still shows.
public static class MuseumShipHullTool
{
    [MenuItem("Tools/Museum/Hide Cabin Inside Bigger Spaceship")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var a=GameObject.Find("08_ShipAirlock")?.transform;var s=GameObject.Find("Visitor_Spaceship")?.transform;
        if(a==null||s==null){Debug.LogError("Need 08_ShipAirlock and Visitor_Spaceship.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeShipHull.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        if(s.parent!=a)Undo.SetTransformParent(s,a,"ship");
        Undo.RecordObject(s,"ship");
        var b=LocalBounds(s,a);
        // Cabin: 4.4 wide (x) x 3 high x 4.4 deep (z, away from the museum). Make the hull at least ~3x that.
        float k=Mathf.Max(13f/b.size.x,Mathf.Max(7.5f/b.size.y,12f/b.size.z));
        s.localScale*=k;b=LocalBounds(s,a);
        // Centre left/right on the hatch, floor at ~1/3 of the hull height, hull front just at the hatch.
        s.localPosition+=new Vector3(-b.center.x,(-0.35f*b.size.y)-b.min.y+0.0f,-0.3f-b.min.z);

        // Re-skin the cabin as ship interior.
        var dark=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Frame_Obsidian.asset");
        var silver=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Frame_Titanium.asset");
        foreach(var n in new[]{"Ceiling","BackWall","Wall_ToShip","Window_Space","FrontWall_A","FrontWall_B","FrontWall_Lintel"}){
            var t=a.Find(n);if(t==null)continue;var r=t.GetComponent<Renderer>();Undo.RecordObject(r,"skin");
            r.sharedMaterial=n.StartsWith("Front")?silver:dark;
        }
        foreach(var n in new[]{"DockingTube","DockingTubeRing"}){var t=a.Find(n);if(t!=null){Undo.RecordObject(t.gameObject,"hide");t.gameObject.SetActive(false);}}

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Spaceship scaled x"+k.ToString("0.00")+" around the cabin; cabin re-skinned as ship interior.");
    }

    static Bounds LocalBounds(Transform root,Transform space)
    {
        bool first=true;var b=new Bounds();
        foreach(var mf in root.GetComponentsInChildren<MeshFilter>()){
            if(mf.sharedMesh==null||!mf.gameObject.activeInHierarchy)continue;var m=mf.transform.localToWorldMatrix;var mb=mf.sharedMesh.bounds;
            for(int i=0;i<8;i++){
                var c=mb.center+Vector3.Scale(mb.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1));
                var p=space.InverseTransformPoint(m.MultiplyPoint3x4(c));
                if(first){b=new Bounds(p,Vector3.zero);first=false;}else b.Encapsulate(p);
            }
        }
        return b;
    }
}
