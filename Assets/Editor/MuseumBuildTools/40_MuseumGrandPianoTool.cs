using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MuseumGrandPianoTool
{
    const string Folder="Assets/MuseumModels/ClassicalPiano/";
    [MenuItem("Tools/Museum/Replace With Free Grand Piano")]
    public static void Build()
    {
        if(EditorApplication.isPlaying)throw new System.InvalidOperationException("Exit Play Mode before replacing the model.");
        var root=GameObject.Find("Exhibit_ClassicalPiano");
        if(!root)throw new System.InvalidOperationException("Open MidtermMuseum first.");
        if(root.transform.Find("GrandPianoModel"))return;
        var asset=AssetDatabase.LoadAssetAtPath<GameObject>(Folder+"GrandPiano.glb");
        if(!asset)throw new System.InvalidOperationException("GrandPiano.glb is still importing.");
        var model=(GameObject)PrefabUtility.InstantiatePrefab(asset,root.transform);model.name="GrandPianoModel";
        Undo.RegisterCreatedObjectUndo(model,"Replace piano model");
        model.transform.localPosition=Vector3.zero;
        // The source keyboard runs along X. Its depth points toward local +Z.
        model.transform.localRotation=Quaternion.Euler(-90,0,0);
        var renderers=model.GetComponentsInChildren<MeshRenderer>();
        var bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
        model.transform.localScale*=1.85f/bounds.size.x;
        bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
        model.transform.position+=new Vector3(root.transform.position.x-bounds.center.x,root.transform.position.y-bounds.min.y,root.transform.position.z-bounds.center.z);
        var ivory=Mat("Grand_Ivory",new Color(.92f,.89f,.78f),.1f);
        var ebony=Mat("Grand_Ebony",new Color(.018f,.02f,.028f),.1f);
        var wood=Mat("Grand_Wood",new Color(.12f,.055f,.03f),.25f);
        var brass=Mat("Grand_Brass",new Color(.57f,.37f,.12f),.7f);
        var keys=new List<Transform>();var report=new System.Text.StringBuilder();
        foreach(var renderer in renderers){
            var mesh=renderer.GetComponent<MeshFilter>().sharedMesh;var size=mesh.bounds.size;
            bool white=size.x>.9f && size.x<1.01f && size.y<.22f && size.z<.25f;
            bool black=size.x>.12f && size.x<.15f && size.y<.21f && size.z>.65f && size.z<.75f;
            var material=white?ivory:black?ebony:renderer.sharedMaterial.name.StartsWith("Material.012")?brass:wood;
            var materials=renderer.sharedMaterials;for(int i=0;i<materials.Length;i++)materials[i]=material;renderer.sharedMaterials=materials;
            if(white||black)keys.Add(renderer.transform);
            report.AppendLine(renderer.name+" key="+(white||black)+" bounds="+renderer.bounds);
        }
        if(keys.Count<20)throw new System.InvalidOperationException("Not enough independent keys detected: "+keys.Count);
        var animation=model.AddComponent<MuseumPianoKeys>();animation.piano=root.GetComponent<MuseumPiano>();animation.keys=keys.ToArray();
        // Preserve the old geometry as an inactive backup; the same interaction and media stay wired.
        var backup=new GameObject("OriginalPiano_Backup");backup.transform.SetParent(root.transform,false);Undo.RegisterCreatedObjectUndo(backup,"Keep original piano");
        var originals=new List<Transform>();foreach(Transform child in root.transform)if(child!=model.transform && child!=backup.transform && child.name!="ScreenFrame" && child.name!="PerformanceScreen" && child.name!="PerformanceStatus")originals.Add(child);
        foreach(var child in originals)Undo.SetTransformParent(child,backup.transform,"Keep original piano");backup.SetActive(false);
        var collision=model.AddComponent<BoxCollider>();var localBounds=new Bounds();bool first=true;
        foreach(var renderer in renderers){var b=renderer.bounds;for(int i=0;i<8;i++){var point=model.transform.InverseTransformPoint(b.center+Vector3.Scale(b.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1)));if(first){localBounds=new Bounds(point,Vector3.zero);first=false;}else localBounds.Encapsulate(point);}}
        collision.center=localBounds.center;collision.size=localBounds.size;
        var exhibit=root.GetComponent<MuseumExhibit>();Undo.RecordObject(exhibit,"Credit piano artist");
        exhibit.credit="Piano: Rigged, Cell-Shaded Grand Piano by Stekapinko, via Sketchfab, CC Attribution. Adapted: Unity materials, scale and decorative key motion.\nhttps://sketchfab.com/3d-models/rigged-cell-shaded-grand-piano-e9f77ab00f204dbeb046f17c553ec071\nPerformance recordings supplied by project owner.";
        EditorSceneManager.MarkSceneDirty(root.scene);EditorSceneManager.SaveScene(root.scene);AssetDatabase.SaveAssets();
        File.WriteAllText("C:/Users/isabe/Documents/Codex/2026-11-18/unity-midterm-improvements/work/grand-piano-result.txt","MODEL PASS keys="+keys.Count+" bounds="+bounds+"\n"+report);
    }
    static Material Mat(string name,Color color,float metal){var existing=AssetDatabase.LoadAssetAtPath<Material>(Folder+name+".mat");if(existing)return existing;var material=new Material(Shader.Find("Universal Render Pipeline/Lit")){name=name};material.color=color;material.SetFloat("_Metallic",metal);material.SetFloat("_Smoothness",.75f);AssetDatabase.CreateAsset(material,Folder+name+".mat");return material;}
}
