using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// The atmosphere pass came out far too dark: the new Nebula 2 sky is very dark, so the ambient light it gives is weak,
// ACES tonemapping darkens mid-tones, and fog + vignette darkened it further.
// This keeps the gallery mood but brings the museum back to a readable brightness, then re-bakes.
public static class MuseumBrightenTool
{
    [MenuItem("Tools/Museum/Brighten Atmosphere + Rebake")]
    public static void Brighten()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeBrighten.unity"));

        // Ambient: fixed soft colours instead of the dark sky (sky stays as the background).
        RenderSettings.ambientMode=AmbientMode.Trilight;
        RenderSettings.ambientSkyColor=new Color(0.55f,0.6f,0.8f);
        RenderSettings.ambientEquatorColor=new Color(0.45f,0.4f,0.6f);
        RenderSettings.ambientGroundColor=new Color(0.25f,0.22f,0.3f);
        RenderSettings.ambientIntensity=1f;
        // Lighter, thinner fog.
        RenderSettings.fogDensity=0.005f;RenderSettings.fogColor=new Color(0.35f,0.3f,0.5f);

        var sun=GameObject.Find("Directional Light")?.GetComponent<Light>();
        if(sun!=null){Undo.RecordObject(sun,"sun");sun.intensity=1.3f;sun.color=new Color(0.9f,0.92f,1f);}
        var atmo=GameObject.Find("10_Atmosphere");
        if(atmo!=null)foreach(var l in atmo.GetComponentsInChildren<Light>()){
            Undo.RecordObject(l,"light");
            if(l.type==LightType.Spot){l.intensity=14;l.range=9;l.spotAngle=55;l.shadows=LightShadows.None;} // also clears the shadow-atlas warning
            else{l.intensity=3.5f;l.range=9;}
        }
        // Keep realtime shadows only on the two hero exhibits.
        foreach(var n in new[]{"Spot_Exhibit_07_Perseverance","Spot_Exhibit_04_SpaceSuit"}){var g=GameObject.Find(n);if(g!=null)g.GetComponent<Light>().shadows=LightShadows.Soft;}

        var prof=AssetDatabase.LoadAssetAtPath<VolumeProfile>("Assets/MuseumGenerated/MuseumVolumeProfile.asset");
        if(prof!=null){
            if(prof.TryGet(out Tonemapping tm))tm.mode.Override(TonemappingMode.Neutral);
            if(prof.TryGet(out ColorAdjustments ca)){ca.postExposure.Override(0.9f);ca.contrast.Override(6f);ca.saturation.Override(10f);}
            if(prof.TryGet(out Vignette vg))vg.intensity.Override(0.15f);
            if(prof.TryGet(out Bloom bl)){bl.threshold.Override(1.0f);bl.intensity.Override(0.5f);}
            EditorUtility.SetDirty(prof);AssetDatabase.SaveAssets();
        }
        // The focus rings were glowing white under bloom: tone their glow down.
        var ring=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");
        if(ring!=null){ring.SetColor("_EmissionColor",new Color(1f,0.5f,0.78f)*0.8f);ring.SetColor("_BaseColor",new Color(1f,0.6f,0.82f));EditorUtility.SetDirty(ring);}

        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Brightened: trilight ambient, stronger gallery lights, neutral tonemapping, +0.9 exposure, thinner fog. Re-baking now.");
        Lightmapping.BakeAsync();
    }
}
