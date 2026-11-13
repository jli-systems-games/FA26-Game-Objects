using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

// Lossless (no visual change) ways to make the WebGL download smaller:
//  - Brotli compression instead of Gzip (with decompression fallback so itch.io still works)
//  - remove the "Made with Unity" splash screen (allowed on Unity 6 Personal) - also starts faster
//  - medium managed code stripping + strip engine code (smaller .wasm, no effect on visuals)
//  - music imported as Vorbis at good quality and loaded in the background
public static class MuseumWebGLSizeTool
{
    [MenuItem("Tools/Museum/WebGL Size Settings (no quality loss)")]
    public static void Apply()
    {
        PlayerSettings.WebGL.compressionFormat=WebGLCompressionFormat.Brotli;
        PlayerSettings.WebGL.decompressionFallback=true;
        PlayerSettings.SplashScreen.show=false;
        PlayerSettings.stripEngineCode=true;
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.WebGL,ManagedStrippingLevel.Medium);
        PlayerSettings.productName="Human Museum";

        int n=0;
        foreach(var guid in AssetDatabase.FindAssets("t:AudioClip",new[]{"Assets/MuseumAudio"})){
            var path=AssetDatabase.GUIDToAssetPath(guid);
            if(AssetImporter.GetAtPath(path) is AudioImporter ai){
                var s=ai.defaultSampleSettings;s.compressionFormat=AudioCompressionFormat.Vorbis;s.quality=0.6f;s.loadType=AudioClipLoadType.CompressedInMemory;
                ai.defaultSampleSettings=s;ai.loadInBackground=true;ai.SaveAndReimport();n++;
            }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("WebGL size settings applied (Brotli, no splash, stripping, "+n+" music files as Vorbis 60%). Also set Build Profiles > Code Optimization = Disk Size, then rebuild.");
    }
}
