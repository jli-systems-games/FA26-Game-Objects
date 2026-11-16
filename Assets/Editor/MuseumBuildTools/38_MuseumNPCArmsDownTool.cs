using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// The two floating NPCs (Cosmic Dweller, Little alien guy) have no skeleton, so their arms are frozen in a T-pose.
// They are swapped for models WITH animations (arms move naturally): the old models are hidden, lines are kept.
//  - NPC_TwoHeads (spacesuit)  -> Quaternius alien creature (Idle / No)
//  - NPC_SaucerParker (lander) -> Quaternius "Goleling", a little winged head (Flying_Idle / Yes), CC0
public static class MuseumNPCArmsDownTool
{
    [MenuItem("Tools/Museum/Swap T-Pose NPCs For Animated Ones")]
    public static void Swap()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        Do("NPC_TwoHeads","AlienNPC_Creature",1.0f,"Idle","No",false);
        Do("NPC_SaucerParker","AlienNPC_Goleling",0.9f,"Flying_Idle","Yes",true);
        EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("T-pose NPCs swapped for animated aliens.");
    }
    static void Do(string npcName,string model,float size,string idle,string react,bool floats)
    {
        var root=GameObject.Find(npcName);var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/"+model+".glb");
        if(root==null||asset==null){Debug.LogWarning("Skipped "+npcName);return;}
        if(root.transform.Find(model)!=null)return;
        foreach(Transform c in root.transform){Undo.RecordObject(c.gameObject,"hide");c.gameObject.SetActive(false);}
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,root.transform);Undo.RegisterCreatedObjectUndo(g,"npc");
        g.name=model;g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
        var b=MuseumAlienNPCTool.Bounds(g);if(b.size.y>0.001f)g.transform.localScale*=size/b.size.y;
        b=MuseumAlienNPCTool.Bounds(g);var p=root.transform.position;g.transform.position+=new Vector3(p.x-b.center.x,p.y-b.min.y,p.z-b.center.z);
        var npc=root.GetComponent<MuseumAlienNPC>();Undo.RecordObject(npc,"npc");
        npc.floats=floats;npc.bubbleHeight=size+(floats?0.8f:0.4f);npc.animator=MuseumAlienNPCTool.SetupAnimator(g,model,idle,react);
        var col=root.GetComponent<CapsuleCollider>();if(col!=null){Undo.RecordObject(col,"col");col.height=size;col.center=new Vector3(0,size/2,0);}
    }
}
