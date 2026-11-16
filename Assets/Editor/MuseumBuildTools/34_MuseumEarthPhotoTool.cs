using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Adds the "Blue Marble" photo (Apollo 17, 7 Dec 1972, NASA, public domain) to the balcony exhibit panel.
public static class MuseumEarthPhotoTool
{
    [MenuItem("Tools/Museum/Earth Photo In Balcony Panel")]
    public static void Apply()
    {
        const string path="Assets/MuseumSky/Earth_Apollo17_BlueMarble.jpg";
        if(AssetImporter.GetAtPath(path) is TextureImporter ti){ti.maxTextureSize=1024;ti.textureCompression=TextureImporterCompression.CompressedHQ;ti.mipmapEnabled=false;ti.SaveAndReimport();}
        var ex=GameObject.Find("Exhibit_10_WhereEarthWas")?.GetComponent<MuseumExhibit>();
        if(ex==null){Debug.LogError("Exhibit_10_WhereEarthWas not found (run the balcony tool first).");return;}
        Undo.RecordObject(ex,"photo");ex.image=AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if(!ex.credit.Contains("Apollo 17"))ex.credit+=" Photo: \"The Blue Marble\", Apollo 17 crew, NASA, 7 December 1972 (public domain).";
        EditorSceneManager.MarkSceneDirty(ex.gameObject.scene);EditorSceneManager.SaveScene(ex.gameObject.scene);
        Debug.Log("Blue Marble photo added to the Where Earth Was panel.");
    }
}
