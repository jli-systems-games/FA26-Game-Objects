using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Makes the exhibit models turn slowly on their plinths (MuseumExhibit already rotates its "model").
// Each model is put inside a "SpinPivot" at its own centre, so it turns on the spot instead of swinging around.
// Left still on purpose: turntable, synthesizer, Boeing cockpit (and the iPhone keeps its gentle sway).
public static class MuseumSpinExhibitsTool
{
    static readonly string[] Skip={"Exhibit_00_RecordPlayer","Exhibit_08_Synthesizer","Exhibit_09_Cockpit757","Exhibit_01_iPhone","Exhibit_00b_InfoDesk","Exhibit_10_WhereEarthWas"};

    [MenuItem("Tools/Museum/Slowly Rotate Exhibits")]
    public static void Apply()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();int n=0;
        foreach(var ex in Object.FindObjectsByType<MuseumExhibit>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
            if(System.Array.IndexOf(Skip,ex.name)>=0||ex.model!=null)continue;
            // The downloaded model is the prefab instance directly under the exhibit (the spacesuit's is the helmet).
            Transform model=null;
            foreach(Transform c in ex.transform)
                if(PrefabUtility.IsAnyPrefabInstanceRoot(c.gameObject)&&c.gameObject.activeSelf&&c.name!="SuitFigure_JJ"&&c.name!="Astronaut_Figure"){model=c;break;}
            if(model==null)continue;
            var rs=model.GetComponentsInChildren<Renderer>();if(rs.Length==0)continue;
            var b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
            var pivot=new GameObject("SpinPivot").transform;Undo.RegisterCreatedObjectUndo(pivot.gameObject,"pivot");
            pivot.SetParent(ex.transform,false);pivot.position=new Vector3(b.center.x,model.position.y,b.center.z);
            Undo.SetTransformParent(model,pivot,"pivot");
            Undo.RecordObject(ex,"spin");ex.model=pivot;ex.idleSpin=ex.name.Contains("Perseverance")?6f:12f;n++;
        }
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log(n+" exhibits now rotate slowly.");
    }
}
