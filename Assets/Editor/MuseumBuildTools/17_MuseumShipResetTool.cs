using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Fixes the tilted / shifted spaceship.
// Cause: one part inside the spaceship model had been moved and rotated in the scene (a prefab override far from the rest),
// which tilted the ship and broke every automatic size calculation.
// This tool reverts those inner overrides (the model goes back to how it was imported), measures the real ship with
// Renderer bounds in the airlock's own frame, and wraps the hull around the cabin.
public static class MuseumShipResetTool
{
    const float HullLength=16f;    // ship size along its longest side, in metres
    const float HullFront=-1.2f;   // nose past the hatch toward the bridge (negative = covers the door frame)
    const float FloorAt=0.35f;     // cabin floor at 35% of the hull height

    [MenuItem("Tools/Museum/Reset Spaceship Model + Wrap Cabin")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var a=GameObject.Find("08_ShipAirlock")?.transform;var root=GameObject.Find("Visitor_Spaceship")?.transform;
        var model=root!=null&&root.childCount>0?root.GetChild(0).gameObject:null;
        if(a==null||root==null||model==null){Debug.LogError("Need 08_ShipAirlock and Visitor_Spaceship with its model.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeShipReset.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // 1) Revert overrides on every object INSIDE the model (keep the model root's own transform).
        foreach(var t in model.GetComponentsInChildren<Transform>(true))
            if(t.gameObject!=model && PrefabUtility.IsPartOfPrefabInstance(t))
                PrefabUtility.RevertObjectOverride(t,InteractionMode.UserAction);

        Undo.RecordObject(root,"ship");Undo.RecordObject(model.transform,"ship");
        if(root.parent!=a)Undo.SetTransformParent(root,a,"ship");
        root.localPosition=Vector3.zero;root.localRotation=Quaternion.identity;root.localScale=Vector3.one;
        model.transform.localPosition=Vector3.zero;model.transform.localRotation=Quaternion.identity;model.transform.localScale=Vector3.one;

        // 2) Measure in the airlock frame: temporarily align the airlock with the world axes.
        var keepRot=a.rotation;var keepPos=a.position;a.SetPositionAndRotation(Vector3.zero,Quaternion.identity);
        var b=WorldBounds(model);
        // Long side across the hatch (along local X) so the door sits on the ship's flank.
        if(b.size.z>b.size.x){root.localRotation=Quaternion.Euler(0,90,0);b=WorldBounds(model);}
        float k=HullLength/Mathf.Max(b.size.x,b.size.z);
        model.transform.localScale=Vector3.one*k;b=WorldBounds(model);
        root.localPosition+=new Vector3(-b.center.x,-FloorAt*b.size.y-b.min.y,HullFront-b.min.z);
        a.SetPositionAndRotation(keepPos,keepRot);

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Spaceship reset: inner overrides reverted, scaled x"+k.ToString("0.00")+" ("+b.size.ToString("0.0")+" m) and wrapped around the cabin.");
    }

    static Bounds WorldBounds(GameObject g)
    {
        var rs=g.GetComponentsInChildren<Renderer>();var b=new Bounds();bool first=true;
        foreach(var r in rs){if(!r.enabled||!r.gameObject.activeInHierarchy)continue;if(first){b=r.bounds;first=false;}else b.Encapsulate(r.bounds);}
        return b;
    }
}
