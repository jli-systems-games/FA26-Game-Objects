using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The rotate tool (35) only turned the helmet of the spacesuit exhibit; the full astronaut figure stood still.
// This makes the astronaut figure (SuitFigure_JJ) the exhibit's turning model, on the spot around its own centre.
// "Try on the suit" still works: it hides/shows the same figure object.
public static class MuseumSpinSuitFigureTool
{
    [MenuItem("Tools/Museum/Rotate Spacesuit Figure")]
    public static void Apply()
    {
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var ex=GameObject.Find("Exhibit_04_SpaceSuit")?.GetComponent<MuseumExhibit>();
        var fig=ex!=null?ex.transform.Find("SuitFigure_JJ"):null;
        if(fig==null){Debug.LogError("Exhibit_04_SpaceSuit/SuitFigure_JJ not found");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var rs=fig.GetComponentsInChildren<Renderer>(true);var b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
        var pivot=new GameObject("FigureSpinPivot").transform;Undo.RegisterCreatedObjectUndo(pivot.gameObject,"pivot");
        pivot.SetParent(ex.transform,false);pivot.position=new Vector3(b.center.x,b.min.y,b.center.z);
        Undo.SetTransformParent(fig,pivot,"pivot");
        Undo.RecordObject(ex,"model");ex.model=pivot;ex.idleSpin=12f;
        EditorSceneManager.MarkSceneDirty(ex.gameObject.scene);EditorSceneManager.SaveScene(ex.gameObject.scene);
        Undo.CollapseUndoOperations(undo);
        Debug.Log("The astronaut figure now turns slowly on its spot.");
    }
}
