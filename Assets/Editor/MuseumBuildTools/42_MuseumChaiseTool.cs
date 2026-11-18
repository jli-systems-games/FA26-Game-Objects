using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MuseumChaiseTool
{
    const string Work="C:/Users/isabe/Documents/Codex/2026-11-18/unity-midterm-improvements/work/";
    public static void Build()
    {
        if(EditorApplication.isPlaying)throw new System.InvalidOperationException("Exit Play Mode before replacing sofa.");
        var root=GameObject.Find("PianoViewingSofa");if(!root)throw new System.InvalidOperationException("Viewing sofa not found.");
        if(root.transform.Find("DownloadedChaise"))return;
        var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/ViewingChaise/ViewingChaise.glb");if(!asset)throw new System.InvalidOperationException("Chaise is still importing.");
        var model=(GameObject)PrefabUtility.InstantiatePrefab(asset,root.transform);model.name="DownloadedChaise";Undo.RegisterCreatedObjectUndo(model,"Replace viewing sofa");
        model.transform.localPosition=Vector3.zero;model.transform.localRotation=Quaternion.Euler(0,90,0);model.transform.localScale=Vector3.one*2.2f;
        var renderers=model.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
        model.transform.position+=Vector3.up*(root.transform.position.y-bounds.min.y);
        foreach(var filter in model.GetComponentsInChildren<MeshFilter>()){var collider=Undo.AddComponent<MeshCollider>(filter.gameObject);collider.sharedMesh=filter.sharedMesh;}
        var backup=new GameObject("BlockSofa_Backup");backup.transform.SetParent(root.transform,false);Undo.RegisterCreatedObjectUndo(backup,"Keep block sofa backup");
        var originals=new List<Transform>();foreach(Transform child in root.transform)if(child!=model.transform && child!=backup.transform && child.name!="RecliningView" && child.name!="RecliningBody")originals.Add(child);
        foreach(var child in originals)Undo.SetTransformParent(child,backup.transform,"Keep original sofa");backup.SetActive(false);
        var sofa=root.GetComponent<MuseumViewingSofa>();Undo.RecordObject(sofa.headRest,"Adjust chaise view");Undo.RecordObject(sofa.bodyRest,"Adjust chaise pose");
        sofa.headRest.localPosition=new Vector3(-.7f,.97f,.05f);sofa.bodyRest.localPosition=new Vector3(.7f,.68f,0);sofa.bodyRest.localRotation=Quaternion.Euler(0,0,90);
        EditorSceneManager.MarkSceneDirty(root.scene);EditorSceneManager.SaveScene(root.scene);AssetDatabase.SaveAssets();
        File.WriteAllText(Work+"chaise-result.txt","BUILD PASS renderers="+renderers.Length+" materials="+renderers[0].sharedMaterials.Length+" bounds="+bounds);
    }
    public static void Preview()
    {
        var root=GameObject.Find("PianoViewingSofa");var transforms=root.transform.Find("DownloadedChaise").GetComponentsInChildren<Transform>();var layers=new int[transforms.Length];
        var cameraObject=new GameObject("ChaisePreviewCamera");var camera=cameraObject.AddComponent<Camera>();var rt=new RenderTexture(1280,720,24);
        try{
            for(int i=0;i<transforms.Length;i++){layers[i]=transforms[i].gameObject.layer;transforms[i].gameObject.layer=31;}
            camera.cullingMask=1<<31;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.12f,.14f,.18f);
            camera.transform.position=root.transform.TransformPoint(new Vector3(2.7f,2.1f,3f));camera.transform.LookAt(root.transform.position+Vector3.up*.65f);camera.fieldOfView=42;camera.targetTexture=rt;camera.Render();
            var previous=RenderTexture.active;RenderTexture.active=rt;var image=new Texture2D(1280,720,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1280,720),0,0);image.Apply();File.WriteAllBytes(Work+"chaise-preview.png",image.EncodeToPNG());RenderTexture.active=previous;Object.DestroyImmediate(image);
        }finally{for(int i=0;i<transforms.Length;i++)transforms[i].gameObject.layer=layers[i];Object.DestroyImmediate(cameraObject);rt.Release();Object.DestroyImmediate(rt);}
    }
}
