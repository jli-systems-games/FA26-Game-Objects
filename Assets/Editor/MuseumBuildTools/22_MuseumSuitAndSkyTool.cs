using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;

// 1) Sky: 4096x2048 "Alien Space Nebula 2" (Jungle Jim, CC BY 4.0) replaces the 1024 image in the nebula sky material.
// 2) Spacesuit exhibit: Jungle Jim's astronaut replaces the Quaternius figure (hidden, kept).
// 3) The suit can be worn: exhibit panel gets two choices, "Stay an alien" / "Try on the suit".
public static class MuseumSuitAndSkyTool
{
    [MenuItem("Tools/Museum/HD Nebula Sky + Wearable Spacesuit")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeSuit.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // ----- 1. Sky -----
        const string tex="Assets/MuseumSky/AlienNebula2_Equirect_4k.png";
        if(AssetImporter.GetAtPath(tex) is TextureImporter ti){ti.wrapModeU=TextureWrapMode.Repeat;ti.wrapModeV=TextureWrapMode.Clamp;ti.mipmapEnabled=false;ti.maxTextureSize=4096;ti.textureCompression=TextureImporterCompression.CompressedHQ;ti.SaveAndReimport();}
        var sky=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumSky/AlienNebula_Sky.mat");
        if(sky!=null){Undo.RecordObject(sky,"sky");sky.SetTexture("_MainTex",AssetDatabase.LoadAssetAtPath<Texture2D>(tex));EditorUtility.SetDirty(sky);RenderSettings.skybox=sky;DynamicGI.UpdateEnvironment();}

        // ----- 2. Exhibit figure -----
        var exhibitGO=GameObject.Find("Exhibit_04_SpaceSuit");var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/AstronautJJ.glb");
        var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        if(exhibitGO==null||asset==null||visitor==null){Debug.LogError("Need Exhibit_04_SpaceSuit, AstronautJJ.glb and the visitor.");return;}
        var ex=exhibitGO.transform;var old=ex.Find("Astronaut_Figure");
        Vector3 spot=old!=null?old.position:ex.position+ex.right*1.6f;
        if(old!=null){Undo.RecordObject(old.gameObject,"hide");old.gameObject.SetActive(false);}
        var floorY=ex.parent!=null?ex.parent.position.y:0;
        GameObject figure=ex.Find("SuitFigure_JJ")?.gameObject;
        if(figure==null){figure=Standing(asset,ex,"SuitFigure_JJ",1.9f);var b=B(figure);figure.transform.position+=new Vector3(spot.x-b.center.x,floorY-b.min.y,spot.z-b.center.z);}

        // ----- 3. Wearable body on the visitor (hidden until chosen) -----
        var v=visitor.transform;GameObject suit=v.Find("SuitBody_Visual")?.gameObject;
        if(suit==null){
            suit=new GameObject("SuitBody_Visual");Undo.RegisterCreatedObjectUndo(suit,"suit");suit.transform.SetParent(v,false);
            var body=Standing(asset,suit.transform,"AstronautJJ",1.85f);var b=B(body);
            body.transform.position+=new Vector3(v.position.x-b.center.x,v.position.y-b.min.y,v.position.z-b.center.z);
            suit.SetActive(false);
        }
        var alien=v.Find("AlienBody_Visual")?.gameObject;
        var wear=exhibitGO.GetComponent<MuseumWearableSuit>();if(wear==null)wear=Undo.AddComponent<MuseumWearableSuit>(exhibitGO);
        wear.visitor=visitor;wear.alienBody=alien;wear.suitBody=suit;wear.exhibitFigure=figure;

        var e=exhibitGO.GetComponent<MuseumExhibit>();Undo.RecordObject(e,"modes");
        e.keepModeOnOpen=true;
        e.modes=new[]{
            new MuseumExhibit.Mode{label="Stay an alien",caption="You look at the suit from outside, as a visitor from elsewhere.",screenColor=Color.white},
            new MuseumExhibit.Mode{label="Try on the suit",caption="You are wearing a human spacesuit. Press V to see yourself in third person. Choose 1 to take it off.",screenColor=Color.white},
        };
        if(e.onModeChanged==null)e.onModeChanged=new UnityEngine.Events.UnityEvent<int>();
        for(int i=e.onModeChanged.GetPersistentEventCount()-1;i>=0;i--)UnityEventTools.RemovePersistentListener(e.onModeChanged,i);
        UnityEventTools.AddPersistentListener(e.onModeChanged,wear.OnModeChanged);
        e.body=e.body.Replace("Look down: as a visitor of this museum, you are already wearing one.","Choose \"Try on the suit\" to wear it yourself.")
                     .Replace("The figure beside this plinth wears the full suit: notice how much of a human it hides.","The suit beside this plinth is complete. Choose \"Try on the suit\" to wear it yourself.");
        e.credit="Models: \"Space Helmet - Model 1b\" by Alan Zimmerman, CC BY 3.0 (Poly Pizza); \"Astronaut by Jungle Jim\", CC BY 4.0 (Sketchfab).";

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("HD nebula sky set; Jungle Jim astronaut placed; suit can be worn from the exhibit panel (1 / 2).");
    }

    // Instantiate and turn it so its tallest side points up (Sketchfab models are sometimes lying down), then scale to 'height'.
    static GameObject Standing(GameObject asset,Transform parent,string name,float height)
    {
        var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,parent);g.name=name;g.transform.localPosition=Vector3.zero;
        Vector3[] tries={Vector3.zero,new Vector3(90,0,0),new Vector3(-90,0,0),new Vector3(0,0,90),new Vector3(0,0,-90)};
        Quaternion best=Quaternion.identity;float bestH=-1;
        foreach(var t in tries){g.transform.localRotation=Quaternion.Euler(t);var h=B(g).size.y;if(h>bestH+0.01f){bestH=h;best=g.transform.localRotation;}}
        g.transform.localRotation=best;if(bestH>0.0001f)g.transform.localScale*=height/bestH;return g;
    }
    static Bounds B(GameObject g){var b=new Bounds();bool f=true;foreach(var r in g.GetComponentsInChildren<Renderer>(true)){if(f){b=r.bounds;f=false;}else b.Encapsulate(r.bounds);}return b;}
}
