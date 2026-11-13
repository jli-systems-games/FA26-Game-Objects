using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The downloaded smartphone looked like a black stick: it is a thin dark slab that kept spinning, so you mostly saw its edge.
// This tool: stands the phone up, turns its flat face toward the front of the plinth, adds a glowing screen
// (it changes colour with the 3 stages), and makes it sway gently left/right instead of spinning.
public static class MuseumPhoneFaceTool
{
    [MenuItem("Tools/Museum/iPhone Faces Visitor + Glowing Screen")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var ex=GameObject.Find("Exhibit_01_iPhone")?.GetComponent<MuseumExhibit>();
        var holder=ex!=null?ex.transform.Find("Model_Smartphone"):null;
        var phone=holder!=null&&holder.childCount>0?holder.GetChild(0):null;
        if(phone==null){Debug.LogError("Exhibit_01_iPhone/Model_Smartphone not found");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforePhoneFace.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        Undo.RecordObject(holder,"phone");Undo.RecordObject(phone,"phone");
        holder.localRotation=Quaternion.identity;

        // 1) Orientation in the holder's frame: tallest side up, thinnest side facing the visitor (local Z).
        var keep=ex.transform.rotation;ex.transform.rotation=Quaternion.identity; // measure along clean axes
        Quaternion best=phone.localRotation;float bestScore=float.MinValue;
        foreach(var e in new[]{Vector3.zero,new Vector3(90,0,0),new Vector3(-90,0,0),new Vector3(0,0,90),new Vector3(0,90,0),new Vector3(90,90,0),new Vector3(0,90,90)}){
            phone.localRotation=Quaternion.Euler(e);var s=B(phone).size;
            float score=s.y*2-s.z*3+s.x*0.5f;   // tall, thin in depth, reasonably wide
            if(score>bestScore){bestScore=score;best=phone.localRotation;}
        }
        phone.localRotation=best;
        var b=B(phone);phone.position+=holder.position-b.center;b=B(phone);   // centred on the holder

        // 2) Glowing screen on the visitor side (-Z).
        var old=holder.Find("Screen_Glow");if(old!=null)Undo.DestroyObjectImmediate(old.gameObject);
        var q=GameObject.CreatePrimitive(PrimitiveType.Quad);q.name="Screen_Glow";Object.DestroyImmediate(q.GetComponent<Collider>());
        Undo.RegisterCreatedObjectUndo(q,"screen");q.transform.SetParent(holder,false);
        var localMin=holder.InverseTransformPoint(b.min);var localSize=holder.InverseTransformVector(b.size);
        q.transform.localPosition=new Vector3(0,0,localMin.z-0.004f);
        q.transform.localScale=new Vector3(Mathf.Abs(localSize.x)*0.86f,Mathf.Abs(localSize.y)*0.88f,1);
        q.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_PhoneScreen.mat");
        ex.transform.rotation=keep;

        Undo.RecordObject(ex,"exhibit");ex.screen=q.GetComponent<Renderer>();ex.sway=true;ex.swayAngle=28f;
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("iPhone now faces the visitor, sways instead of spinning, and has a glowing screen.");
    }
    static Bounds B(Transform t){var b=new Bounds();bool f=true;foreach(var r in t.GetComponentsInChildren<Renderer>()){if(f){b=r.bounds;f=false;}else b.Encapsulate(r.bounds);}return b;}
}
