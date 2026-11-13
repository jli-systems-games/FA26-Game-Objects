using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Atmosphere pass + the required BAKED LIGHTING.
//  1) Gallery lights: a spot light above every exhibit (cool / violet / warm in turn) + low coloured wash lights along the walls.
//  2) Post-processing on the Main Camera only (so UI text stays sharp): bloom on the light strips, ACES tonemapping,
//     a little contrast and a soft vignette. Subtle violet fog for depth.
//  3) Baking setup: architecture marked Static (Contribute GI), lightmap UVs generated for the custom meshes,
//     light probes for moving things (visitor, swaying iPhone), reflection probe re-baked, then the bake starts.
// Everything is added; nothing is deleted. Bake progress shows at the bottom-right of Unity.
public static class MuseumAtmosphereTool
{
    [MenuItem("Tools/Museum/Atmosphere Lights + Post FX + Bake Lighting")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var arch=GameObject.Find("SpaceMuseum_Architecture");if(arch==null){Debug.LogError("SpaceMuseum_Architecture not found");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeAtmosphere.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        // ----- 1. Lights -----
        var old=arch.transform.Find("10_Atmosphere");if(old!=null)Undo.DestroyObjectImmediate(old.gameObject); // rerun = rebuild this group only
        var root=new GameObject("10_Atmosphere").transform;Undo.RegisterCreatedObjectUndo(root.gameObject,"atmo");root.SetParent(arch.transform,false);
        Color[] tints={new Color(0.55f,0.9f,1f),new Color(0.75f,0.6f,1f),new Color(1f,0.82f,0.6f)};
        int i=0;
        foreach(var ex in Object.FindObjectsByType<MuseumExhibit>(FindObjectsSortMode.InstanceID)){
            var g=new GameObject("Spot_"+ex.name);g.transform.SetParent(root,false);
            g.transform.position=ex.transform.position+Vector3.up*4.6f;g.transform.rotation=Quaternion.LookRotation(Vector3.down,Vector3.forward);
            var l=g.AddComponent<Light>();l.type=LightType.Spot;l.spotAngle=42;l.innerSpotAngle=20;l.range=7;l.intensity=9;
            l.color=tints[i++%3];l.lightmapBakeType=LightmapBakeType.Mixed;l.shadows=LightShadows.Soft;
        }
        // Wall wash: low lights near the perimeter, colours following the walls (cyan -> violet -> amber).
        Vector3[] wash={new Vector3(-10.5f,0.6f,-6),new Vector3(-10.5f,0.6f,4),new Vector3(-6,0.6f,15),new Vector3(4,0.6f,14),new Vector3(10,0.6f,6),new Vector3(10.5f,0.6f,-4),new Vector3(0,0.6f,-15)};
        Color[] washC={new Color(0.3f,0.9f,1f),new Color(0.3f,0.9f,1f),new Color(0.6f,0.4f,1f),new Color(0.8f,0.4f,1f),new Color(1f,0.5f,0.75f),new Color(1f,0.7f,0.4f),new Color(0.4f,0.8f,1f)};
        for(int k=0;k<wash.Length;k++){
            var g=new GameObject("Wash_"+k);g.transform.SetParent(root,false);g.transform.localPosition=wash[k];
            var l=g.AddComponent<Light>();l.type=LightType.Point;l.range=7;l.intensity=2.2f;l.color=washC[k];l.lightmapBakeType=LightmapBakeType.Baked;l.shadows=LightShadows.None;
        }
        var sun=GameObject.Find("Directional Light")?.GetComponent<Light>();
        if(sun!=null){Undo.RecordObject(sun,"sun");sun.lightmapBakeType=LightmapBakeType.Mixed;sun.intensity=0.6f;sun.color=new Color(0.75f,0.8f,1f);}

        // Light probes over the walkable floor (for the visitor and moving exhibits).
        var probes=new GameObject("LightProbes").AddComponent<LightProbeGroup>();probes.transform.SetParent(root,false);
        var pts=new List<Vector3>();
        for(float x=-11;x<=11;x+=3.5f)for(float z=-17;z<=17;z+=3.5f)foreach(var y in new[]{0.4f,2.2f,4.5f})pts.Add(new Vector3(x,y,z));
        probes.probePositions=pts.ToArray();

        // ----- 2. Post FX (Main Camera only) + fog -----
        var cam=Camera.main;
        if(cam!=null){var data=cam.GetUniversalAdditionalCameraData();Undo.RecordObject(data,"pp");data.renderPostProcessing=true;}
        var disp=GameObject.Find("DisplayCamera")?.GetComponent<Camera>();
        if(disp!=null){var d=disp.GetUniversalAdditionalCameraData();Undo.RecordObject(d,"pp");d.renderPostProcessing=false;} // keep UI crisp
        var vol=Object.FindFirstObjectByType<Volume>();
        if(vol!=null){
            string path="Assets/MuseumGenerated/MuseumVolumeProfile.asset";
            var prof=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if(prof==null){prof=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(prof,path);}
            Bloom bloom=Get<Bloom>(prof);bloom.threshold.Override(0.85f);bloom.intensity.Override(0.9f);bloom.scatter.Override(0.72f);bloom.tint.Override(new Color(0.8f,0.85f,1f));
            Tonemapping tm=Get<Tonemapping>(prof);tm.mode.Override(TonemappingMode.ACES);
            ColorAdjustments ca=Get<ColorAdjustments>(prof);ca.postExposure.Override(0.35f);ca.contrast.Override(12f);ca.saturation.Override(8f);
            Vignette vg=Get<Vignette>(prof);vg.intensity.Override(0.28f);vg.smoothness.Override(0.5f);vg.color.Override(new Color(0.08f,0.02f,0.15f));
            EditorUtility.SetDirty(prof);AssetDatabase.SaveAssets();
            Undo.RecordObject(vol,"profile");vol.sharedProfile=prof;vol.isGlobal=true;
        }
        RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=0.012f;RenderSettings.fogColor=new Color(0.12f,0.08f,0.22f);

        // ----- 3. Static flags + lightmap UVs -----
        var flags=StaticEditorFlags.ContributeGI|StaticEditorFlags.OccluderStatic|StaticEditorFlags.OccludeeStatic|StaticEditorFlags.ReflectionProbeStatic|StaticEditorFlags.BatchingStatic;
        int count=0;
        foreach(var r in arch.GetComponentsInChildren<MeshRenderer>(true)){
            var t=r.transform;
            if(t.GetComponentInParent<MuseumExhibit>(true)!=null)continue;      // exhibits move/animate -> probes
            if(t.GetComponentInParent<MuseumAutoDoor>(true)!=null)continue;      // sliding doors
            if(t.GetComponentInParent<MuseumSlidingDoor>(true)!=null&&t.name.StartsWith("Hatch"))continue;
            if(PrefabUtility.IsPartOfPrefabInstance(r))continue;                 // downloaded models: no lightmap UVs
            var mf=r.GetComponent<MeshFilter>();if(mf==null||mf.sharedMesh==null)continue;
            var m=mf.sharedMesh;
            if(m.uv2==null||m.uv2.Length==0){
                string mp=AssetDatabase.GetAssetPath(m);
                if(mp.EndsWith(".asset")){Unwrapping.GenerateSecondaryUVSet(m);EditorUtility.SetDirty(m);}else continue; // built-in meshes already have UV2
            }
            if(r.sharedMaterial!=null && r.sharedMaterial.renderQueue>=3000)continue; // transparent glass: skip
            GameObjectUtility.SetStaticEditorFlags(r.gameObject,flags);count++;
        }
        var floor=GameObject.Find("MuseumFloor_Polygon");
        if(floor!=null){var mf=floor.GetComponent<MeshFilter>();if(mf!=null&&mf.sharedMesh!=null&&(mf.sharedMesh.uv2==null||mf.sharedMesh.uv2.Length==0)){Unwrapping.GenerateSecondaryUVSet(mf.sharedMesh);EditorUtility.SetDirty(mf.sharedMesh);}
            GameObjectUtility.SetStaticEditorFlags(floor,flags);count++;}
        AssetDatabase.SaveAssets();

        // ----- Lighting settings + bake -----
        string lp="Assets/MuseumGenerated/MuseumLightingSettings.lighting";
        var ls=AssetDatabase.LoadAssetAtPath<LightingSettings>(lp);
        if(ls==null){ls=new LightingSettings{name="MuseumLightingSettings"};AssetDatabase.CreateAsset(ls,lp);}
        ls.bakedGI=true;ls.realtimeGI=false;ls.mixedBakeMode=MixedLightingMode.IndirectOnly;ls.lightmapper=LightingSettings.Lightmapper.ProgressiveGPU;
        ls.lightmapResolution=8;ls.lightmapMaxSize=1024;ls.directionalityMode=LightmapsMode.NonDirectional;ls.lightmapCompression=LightmapCompression.NormalQuality;
        ls.directSampleCount=16;ls.indirectSampleCount=128;ls.maxBounces=2;ls.filteringMode=LightingSettings.FilterMode.Auto;
        EditorUtility.SetDirty(ls);Lightmapping.lightingSettings=ls;

        var probe=Object.FindFirstObjectByType<ReflectionProbe>();if(probe!=null){Undo.RecordObject(probe,"probe");probe.mode=ReflectionProbeMode.Baked;}

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Atmosphere added ("+count+" static objects). Baking lighting now — watch the progress bar at the bottom right. Save the scene when it finishes.");
        Lightmapping.BakeAsync();
    }

    static T Get<T>(VolumeProfile p) where T:VolumeComponent{if(!p.TryGet(out T c)){c=p.Add<T>(true);}c.active=true;
        if(!AssetDatabase.IsSubAsset(c)){c.name=typeof(T).Name;AssetDatabase.AddObjectToAsset(c,p);}return c;}
}
