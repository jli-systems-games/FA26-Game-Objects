using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The Jungle Jim astronaut was stood up the wrong way round (upside down).
// Checks where the helmet visor is: if it is below the middle of the body, turns the model 180 degrees and puts it back on the floor.
// Applies to the exhibit figure and to the wearable suit on the visitor.
public static class MuseumSuitUprightTool
{
    [MenuItem("Tools/Museum/Turn Spacesuit Right Way Up")]
    public static void Fix()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        int n=0;
        foreach(var path in new[]{"Exhibit_04_SpaceSuit/SuitFigure_JJ","SuitBody_Visual/AstronautJJ"}){
            Transform t=Find(path);if(t==null){Debug.LogWarning("Not found: "+path);continue;}
            bool wasActive=t.gameObject.activeInHierarchy;var top=t.parent;
            if(!wasActive)top.gameObject.SetActive(true); // bounds need an active object
            var all=B(t,null);var visor=B(t,"Visor");
            if(visor.HasValue && visor.Value.center.y<all.Value.center.y){
                Undo.RecordObject(t,"upright");
                float floor=all.Value.min.y;var c=all.Value.center;
                t.RotateAround(c,top.forward,180f);           // flip head over heels around the body centre
                var nb=B(t,null).Value;t.position+=Vector3.up*(floor-nb.min.y); // feet back on the same floor height
                n++;
            }
            if(!wasActive)top.gameObject.SetActive(false);
        }
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log(n==0?"Spacesuit already upright.":"Turned "+n+" spacesuit model(s) right way up.");
    }
    static Transform Find(string path)
    {
        var parts=path.Split('/');var root=GameObject.Find(parts[0]);
        if(root==null){var v=Object.FindFirstObjectByType<MuseumVisitorController>();if(v!=null)root=v.transform.Find(parts[0])?.gameObject;}
        return root!=null?root.transform.Find(parts[1]):null;
    }
    static Bounds? B(Transform t,string nameContains)
    {
        Bounds? b=null;
        foreach(var r in t.GetComponentsInChildren<Renderer>(true)){
            if(nameContains!=null && r.name.IndexOf(nameContains,System.StringComparison.OrdinalIgnoreCase)<0)continue;
            if(b==null)b=r.bounds;else{var x=b.Value;x.Encapsulate(r.bounds);b=x;}
        }
        return b;
    }
}
