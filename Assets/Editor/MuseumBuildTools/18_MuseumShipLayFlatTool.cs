using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The imported spaceship stands nose-up. This tool lays it flat automatically:
// it tries the basic 90-degree orientations and keeps the one where the ship is lowest (flattest),
// turns its long side across the hatch, scales it and wraps the hull around the cabin.
// If it ends up upside down, set Visitor_Spaceship rotation Z to 180 by hand.
public static class MuseumShipLayFlatTool
{
    const float HullLength=16f, HullFront=-1.2f, FloorAt=0.35f;

    [MenuItem("Tools/Museum/Lay Spaceship Flat + Wrap Cabin")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var a=GameObject.Find("08_ShipAirlock")?.transform;var root=GameObject.Find("Visitor_Spaceship")?.transform;
        var model=root!=null&&root.childCount>0?root.GetChild(0):null;
        if(a==null||root==null||model==null){Debug.LogError("Need 08_ShipAirlock and Visitor_Spaceship with its model.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeLayFlat.unity"));
        Undo.RecordObject(root,"ship");Undo.RecordObject(model,"ship");

        var keepRot=a.rotation;var keepPos=a.position;a.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
        root.localPosition=Vector3.zero;root.localScale=Vector3.one;model.localPosition=Vector3.zero;model.localRotation=Quaternion.identity;model.localScale=Vector3.one;

        // 1) Flattest orientation = smallest height.
        Vector3[] tries={Vector3.zero,new Vector3(90,0,0),new Vector3(-90,0,0),new Vector3(0,0,90),new Vector3(0,0,-90)};
        Quaternion best=Quaternion.identity;float bestH=float.MaxValue;
        foreach(var e in tries){root.localRotation=Quaternion.Euler(e);float h=Bounds(model).size.y;if(h<bestH-0.01f){bestH=h;best=root.localRotation;}}
        root.localRotation=best;
        // 2) Long side across the hatch (local X).
        var b=Bounds(model);if(b.size.z>b.size.x){root.localRotation=Quaternion.Euler(0,90,0)*best;b=Bounds(model);}
        // 3) Scale + wrap.
        float k=HullLength/Mathf.Max(b.size.x,b.size.z);model.localScale=Vector3.one*k;b=Bounds(model);
        root.localPosition+=new Vector3(-b.center.x,-FloorAt*b.size.y-b.min.y,HullFront-b.min.z);
        a.SetPositionAndRotation(keepPos,keepRot);

        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Spaceship laid flat ("+b.size.ToString("0.0")+" m) and wrapped around the cabin. Upside down? Set Visitor_Spaceship rotation Z = 180.");
    }
    static Bounds Bounds(Transform g)
    {
        var b=new Bounds();bool first=true;
        foreach(var r in g.GetComponentsInChildren<Renderer>()){if(!r.enabled||!r.gameObject.activeInHierarchy)continue;if(first){b=r.bounds;first=false;}else b.Encapsulate(r.bounds);}
        return b;
    }
}
