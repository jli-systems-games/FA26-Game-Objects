using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 1) Alien nebula skybox (equirectangular image taken from Jungle Jim's Sketchfab skybox, CC BY 4.0) -> Skybox/Panoramic.
// 2) The alien visitor's spaceship parked beside the arrival bridge, hovering in space.
// The previous skybox material is kept in the project; only RenderSettings.skybox changes.
public static class MuseumSkyShipTool
{
    [MenuItem("Tools/Museum/Alien Nebula Sky + Parked Spaceship")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforeSky.unity"));

        // ----- Sky -----
        const string tex="Assets/MuseumSky/AlienNebula_Equirect.png";
        var imp=(TextureImporter)AssetImporter.GetAtPath(tex);
        if(imp!=null){imp.wrapModeU=TextureWrapMode.Repeat;imp.wrapModeV=TextureWrapMode.Clamp;imp.mipmapEnabled=false;imp.maxTextureSize=2048;imp.SaveAndReimport();}
        var sky=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumSky/AlienNebula_Sky.mat");
        if(sky==null){
            sky=new Material(Shader.Find("Skybox/Panoramic")){name="AlienNebula_Sky"};
            sky.SetTexture("_MainTex",AssetDatabase.LoadAssetAtPath<Texture2D>(tex));
            sky.SetFloat("_Mapping",1);sky.SetFloat("_ImageType",0);sky.SetFloat("_Exposure",1.1f);sky.SetFloat("_Rotation",200);
            AssetDatabase.CreateAsset(sky,"Assets/MuseumSky/AlienNebula_Sky.mat");
        }
        RenderSettings.skybox=sky;DynamicGI.UpdateEnvironment();

        // ----- Spaceship -----
        var arch=GameObject.Find("SpaceMuseum_Architecture");
        var ship=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/AlienSpaceship.glb");
        if(arch!=null && ship!=null && GameObject.Find("Visitor_Spaceship")==null){
            Vector3 door=new Vector3(4,0,-12),outward=new Vector3(0.6f,0,-0.8f),side=new Vector3(0.8f,0,0.6f);
            Vector3 local=door+outward*9f+side*7.5f+Vector3.down*1.2f;
            var root=new GameObject("Visitor_Spaceship");Undo.RegisterCreatedObjectUndo(root,"ship");
            root.transform.SetParent(arch.transform,false);root.transform.localPosition=local;
            root.transform.localRotation=Quaternion.LookRotation(-side); // nose pointing back along the museum front
            var g=(GameObject)PrefabUtility.InstantiatePrefab(ship,root.transform);g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
            var rs=g.GetComponentsInChildren<Renderer>();var b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
            float max=Mathf.Max(b.size.x,b.size.y,b.size.z);if(max>0.0001f)g.transform.localScale*=9f/max;
            rs=g.GetComponentsInChildren<Renderer>();b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);
            g.transform.position+=root.transform.position-new Vector3(b.center.x,b.min.y,b.center.z);
        }
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Alien nebula skybox set; spaceship parked by the bridge. Adjust Visitor_Spaceship position/rotation by hand if needed.");
    }
}
