using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// WebGL size fix. The Build Report showed textures = 77% of the game (400 MB uncompressed):
// textures inside the downloaded .glb files are imported UNCOMPRESSED and at full size (often 4K).
// This tool makes optimised copies of the materials used in the scene by the big models:
// every texture is resized to at most 1024 px and saved as a normal, compressed Unity texture.
// The scene then uses the copies (the original .glb files are untouched; revert = Prefab > Revert on the model).
public static class MuseumOptimizeTexturesTool
{
    static int MaxSize=2048;
    const string OutDir="Assets/MuseumGenerated/Optimized";
    static readonly string[] Models={"Turntable","Cockpit757","Synthesizer","Perseverance","AlienSpaceship","ISS","LanderB","Watch","Smartphone","AstronautJJ","MacBookPro","SpaceHelmet"};

    // 2048: textures keep (almost) all their detail, just compressed. 1024: smallest download.
    [MenuItem("Tools/Museum/Optimize Model Textures For WebGL (2048, keep quality)")]
    public static void Run2048(){MaxSize=2048;Run();}
    [MenuItem("Tools/Museum/Optimize Model Textures For WebGL (1024, smallest)")]
    public static void Run1024(){MaxSize=1024;Run();}

    public static void Run()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeOptimize.unity"));
        Directory.CreateDirectory(OutDir);
        var matCopies=new Dictionary<Material,Material>();var texCopies=new Dictionary<Texture,Texture2D>();
        int renderers=0;
        foreach(var r in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
            var src=PrefabUtility.GetCorrespondingObjectFromOriginalSource(r.gameObject);
            string path=src!=null?AssetDatabase.GetAssetPath(src):"";
            if(!System.Array.Exists(Models,m=>path.EndsWith("/"+m+".glb")))continue;
            var mats=r.sharedMaterials;bool changed=false;
            for(int i=0;i<mats.Length;i++){
                var m=mats[i];if(m==null||AssetDatabase.GetAssetPath(m).StartsWith(OutDir))continue;
                if(!matCopies.TryGetValue(m,out var copy)){copy=CopyMaterial(m,texCopies);matCopies[m]=copy;}
                mats[i]=copy;changed=true;
            }
            if(changed){Undo.RecordObject(r,"optimize");r.sharedMaterials=mats;renderers++;}
            EditorUtility.DisplayProgressBar("Optimizing textures",r.name,(float)renderers/400f);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Optimized "+texCopies.Count+" textures and "+matCopies.Count+" materials on "+renderers+" renderers (max "+MaxSize+" px, compressed). Rebuild WebGL to see the new size.");
    }

    static Material CopyMaterial(Material m,Dictionary<Texture,Texture2D> texCopies)
    {
        var copy=new Material(m){name=Safe(m.name)};
        foreach(var prop in m.GetTexturePropertyNames()){
            var t=m.GetTexture(prop);if(t==null||!(t is Texture2D))continue;
            if(!texCopies.TryGetValue(t,out var small)){small=Shrink((Texture2D)t,prop);texCopies[t]=small;}
            if(small!=null)copy.SetTexture(prop,small);
        }
        string p=AssetDatabase.GenerateUniqueAssetPath(OutDir+"/"+copy.name+".mat");AssetDatabase.CreateAsset(copy,p);return copy;
    }

    // Read the (non-readable) texture through the GPU, downscale, save as PNG and import it compressed.
    static Texture2D Shrink(Texture2D t,string prop)
    {
        string lower=prop.ToLower();
        bool normal=lower.Contains("normal")||lower.Contains("bump");
        bool linear=normal||lower.Contains("metallic")||lower.Contains("occlusion")||lower.Contains("rough");
        float k=Mathf.Min(1f,(float)MaxSize/Mathf.Max(t.width,t.height));
        int w=Mathf.Max(4,Mathf.RoundToInt(t.width*k)),h=Mathf.Max(4,Mathf.RoundToInt(t.height*k));
        var rt=RenderTexture.GetTemporary(w,h,0,RenderTextureFormat.ARGB32,linear?RenderTextureReadWrite.Linear:RenderTextureReadWrite.sRGB);
        Graphics.Blit(t,rt);var prev=RenderTexture.active;RenderTexture.active=rt;
        var tex=new Texture2D(w,h,TextureFormat.RGBA32,false,linear);tex.ReadPixels(new Rect(0,0,w,h),0,0);tex.Apply();
        RenderTexture.active=prev;RenderTexture.ReleaseTemporary(rt);
        string file=AssetDatabase.GenerateUniqueAssetPath(OutDir+"/"+Safe(t.name.Length>0?t.name:"tex")+".png");
        File.WriteAllBytes(file,tex.EncodeToPNG());Object.DestroyImmediate(tex);AssetDatabase.ImportAsset(file);
        var imp=(TextureImporter)AssetImporter.GetAtPath(file);
        imp.maxTextureSize=MaxSize;imp.textureCompression=TextureImporterCompression.Compressed;imp.mipmapEnabled=true;
        imp.sRGBTexture=!linear; // normal maps stay "Default + linear", exactly like the glTF shader expects
        imp.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Texture2D>(file);
    }
    static string Safe(string s){foreach(var c in Path.GetInvalidFileNameChars())s=s.Replace(c,'_');return s.Replace('|','_').Replace(' ','_');}
}
