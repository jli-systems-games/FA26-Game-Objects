using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Automatic sliding glass doors in the museum entrance (between DoorPost_A and DoorPost_B).
public static class MuseumAutoDoorTool
{
    [MenuItem("Tools/Museum/Museum Entrance Auto Door")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var arch=GameObject.Find("SpaceMuseum_Architecture");
        if(arch==null){Debug.LogError("SpaceMuseum_Architecture not found");return;}
        if(arch.transform.Find("09_EntranceAutoDoor")!=null){Debug.Log("Auto door already exists.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeAutoDoor.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var glass=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Glass_Ice.asset");
        var silver=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Frame_Titanium.asset");
        var cyan=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Light_Cyan.asset");

        // Doorway centre and directions (same numbers as the architecture tool). Local X runs along the wall.
        Vector3 door=new Vector3(4,0,-12),outward=new Vector3(0.6f,0,-0.8f);
        var root=new GameObject("09_EntranceAutoDoor").transform;Undo.RegisterCreatedObjectUndo(root.gameObject,"door");
        root.SetParent(arch.transform,false);root.localPosition=door;root.localRotation=Quaternion.LookRotation(outward);

        var left=Panel(root,"Panel_Left",-1.1f,glass,silver,cyan);var right=Panel(root,"Panel_Right",1.1f,glass,silver,cyan);
        Box(root,"Transom_Glass",new Vector3(0,3.8f,0),new Vector3(4.4f,0.8f,0.06f),glass,false);
        Box(root,"Header_Light",new Vector3(0,3.42f,-0.06f),new Vector3(4.4f,0.05f,0.05f),cyan,false);
        Box(root,"Header_Light_In",new Vector3(0,3.42f,0.06f),new Vector3(4.4f,0.05f,0.05f),cyan,false);

        var d=root.gameObject.AddComponent<MuseumAutoDoor>();
        d.leftPanel=left;d.rightPanel=right;d.slide=2.1f;d.range=3.5f;
        var v=Object.FindFirstObjectByType<MuseumVisitorController>();if(v!=null)d.visitor=v.transform;

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Automatic entrance doors added.");
    }

    // One glass door leaf with a thin metal edge and a light line; it has a collider so closed doors block the way.
    static Transform Panel(Transform root,string name,float x,Material glass,Material frame,Material glow)
    {
        var p=new GameObject(name).transform;p.SetParent(root,false);p.localPosition=new Vector3(x,0,0);
        Box(p,"Glass",new Vector3(0,1.7f,0),new Vector3(2.18f,3.4f,0.06f),glass,true);
        Box(p,"Edge",new Vector3(-Mathf.Sign(x)*1.07f,1.7f,0),new Vector3(0.05f,3.4f,0.09f),frame,false);
        Box(p,"Handle_Light",new Vector3(-Mathf.Sign(x)*0.95f,1.1f,0),new Vector3(0.03f,0.9f,0.1f),glow,false);
        return p;
    }
    static GameObject Box(Transform parent,string name,Vector3 pos,Vector3 size,Material m,bool collider)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent,false);
        g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=m;
        g.GetComponent<Renderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
        if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
    }
}
