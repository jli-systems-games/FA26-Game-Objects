using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// Tech + slightly pixel typography (both fonts SIL Open Font License, files in Assets/MuseumFonts):
//  - Pixelify Sans: titles, buttons, prompts, short labels
//  - Space Mono: body text (monospace, techy but easy to read)
// Creates TextMeshPro font assets and applies them to every TMP text in the scene.
public static class MuseumFontTool
{
    const string Dir="Assets/MuseumFonts/";
    // Objects whose text is a heading/label -> pixel font. Everything else -> mono body font.
    static readonly string[] HeadingNames={"Title","Subtitle","Paused","Label","PromptText","HatchPrompt","Text_Prompt","Label_Title","Hint"};

    [MenuItem("Tools/Museum/Apply Tech Pixel Fonts")]
    public static void Apply()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying){Debug.LogError("Exit Play Mode first.");return;}
        var pixel=FontAsset("PixelifySans");var mono=FontAsset("SpaceMono-Regular");
        if(pixel==null||mono==null){Debug.LogError("Font files missing in "+Dir);return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeFonts.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        int n=0;
        foreach(var t in Object.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include,FindObjectsSortMode.None)){
            if(!t.gameObject.scene.IsValid())continue;
            bool heading=System.Array.Exists(HeadingNames,h=>t.name==h)||(t.transform.parent!=null && t.transform.parent.name.EndsWith("Button"));
            Undo.RecordObject(t,"font");t.font=heading?pixel:mono;
            // The prompt panel's text sits under HatchPromptUI / ExhibitPrompt: treat as heading.
            if(t.transform.parent!=null&&(t.transform.parent.name=="HatchPromptUI"||t.transform.parent.name=="ExhibitPrompt"))t.font=pixel;
            n++;
        }
        // New TMP text created later uses the mono font by default.
        var settings=TMP_Settings.instance;if(settings!=null){var so=new SerializedObject(settings);var p=so.FindProperty("m_defaultFontAsset");if(p!=null){p.objectReferenceValue=mono;so.ApplyModifiedProperties();}}
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Fonts applied to "+n+" texts (Pixelify Sans headings, Space Mono body).");
    }

    // Dynamic TMP font asset saved next to the .ttf (glyphs are added as needed, also in WebGL).
    static TMP_FontAsset FontAsset(string file)
    {
        string path=Dir+file+" SDF.asset";
        var fa=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);if(fa!=null)return fa;
        var font=AssetDatabase.LoadAssetAtPath<Font>(Dir+file+".ttf");if(font==null)return null;
        fa=TMP_FontAsset.CreateFontAsset(font,90,9,UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA,1024,1024,AtlasPopulationMode.Dynamic,true);
        fa.name=file+" SDF";
        AssetDatabase.CreateAsset(fa,path);
        fa.atlasTextures[0].name=file+" Atlas";AssetDatabase.AddObjectToAsset(fa.atlasTextures[0],fa);
        fa.material.name=file+" Material";AssetDatabase.AddObjectToAsset(fa.material,fa);
        AssetDatabase.SaveAssets();return fa;
    }
}
