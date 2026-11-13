using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

// Replaces the hard pink ellipse under each exhibit with a soft round glow that fades out at the edges,
// and adds a small light that makes the object itself light up while you can press E.
// The old rings are hidden, not deleted.
public static class MuseumSoftGlowTool
{
    [MenuItem("Tools/Museum/Soft Focus Glow For Exhibits")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeSoftGlow.unity"));
        var mat=GlowMaterial();
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();int n=0;
        foreach(var ex in Object.FindObjectsByType<MuseumExhibit>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
            var old=ex.focusHighlight;if(old==null||old.name=="FocusGlow")continue;
            // Size and floor height from the old ring; the new glow is round and a bit bigger, because it fades out.
            float size=Mathf.Max(old.transform.lossyScale.x,old.transform.lossyScale.z)*1.5f;
            var g=new GameObject("FocusGlow");Undo.RegisterCreatedObjectUndo(g,"glow");g.transform.SetParent(ex.transform,false);
            var quad=GameObject.CreatePrimitive(PrimitiveType.Quad);quad.name="Glow";Object.DestroyImmediate(quad.GetComponent<Collider>());
            quad.transform.SetParent(g.transform,false);quad.transform.position=old.transform.position+Vector3.up*0.004f;
            quad.transform.rotation=Quaternion.Euler(90,0,0);quad.transform.localScale=Vector3.one;
            quad.transform.localScale=new Vector3(size/quad.transform.lossyScale.x,size/quad.transform.lossyScale.y,1);
            var r=quad.GetComponent<Renderer>();r.sharedMaterial=mat;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=false;
            // Small light on the object itself.
            var lg=new GameObject("ObjectLight");lg.transform.SetParent(g.transform,false);
            Vector3 c=ex.model!=null?Center(ex.model):ex.transform.position+Vector3.up*1f;
            lg.transform.position=c+Vector3.up*0.9f;
            var l=lg.AddComponent<Light>();l.type=LightType.Point;l.color=new Color(1f,0.7f,0.9f);l.intensity=2.5f;l.range=2.8f;
            l.shadows=LightShadows.None;l.lightmapBakeType=LightmapBakeType.Realtime;
            g.SetActive(false);
            Undo.RecordObject(old,"hide");old.SetActive(false);
            Undo.RecordObject(ex,"glow");ex.focusHighlight=g;n++;
        }
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Soft glow added to "+n+" exhibits.");
    }

    static Vector3 Center(Transform t){var rs=t.GetComponentsInChildren<Renderer>(true);if(rs.Length==0)return t.position;var b=rs[0].bounds;foreach(var r in rs)b.Encapsulate(r.bounds);return b.center;}

    // Radial gradient (bright centre -> transparent edge), saved as a PNG so it is part of the build.
    static Material GlowMaterial()
    {
        const string texPath="Assets/MuseumGenerated/SoftGlow.png",matPath="Assets/MuseumGenerated/SoftGlow.mat";
        var m=AssetDatabase.LoadAssetAtPath<Material>(matPath);if(m!=null)return m;
        const int S=256;var t=new Texture2D(S,S,TextureFormat.RGBA32,false);
        for(int y=0;y<S;y++)for(int x=0;x<S;x++){
            float d=Vector2.Distance(new Vector2(x,y),new Vector2(S/2f,S/2f))/(S/2f);
            float a=Mathf.Pow(Mathf.Clamp01(1-d),2.2f);          // smooth falloff, zero at the edge
            t.SetPixel(x,y,new Color(1,1,1,a));
        }
        File.WriteAllBytes(texPath,t.EncodeToPNG());AssetDatabase.ImportAsset(texPath);
        var imp=(TextureImporter)AssetImporter.GetAtPath(texPath);imp.alphaIsTransparency=true;imp.wrapMode=TextureWrapMode.Clamp;imp.SaveAndReimport();
        m=new Material(Shader.Find("Universal Render Pipeline/Unlit")){name="SoftGlow"};
        m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(texPath));m.SetColor("_BaseColor",new Color(1f,0.55f,0.85f,0.9f));
        // Transparent + additive: it only adds light, never darkens the floor.
        m.SetFloat("_Surface",1);m.SetFloat("_Blend",2);m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);m.SetFloat("_DstBlend",(float)BlendMode.One);
        m.SetFloat("_ZWrite",0);m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.SetOverrideTag("RenderType","Transparent");m.renderQueue=3000;
        AssetDatabase.CreateAsset(m,matPath);return m;
    }
}
