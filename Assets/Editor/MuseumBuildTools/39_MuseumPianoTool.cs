using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public static class MuseumPianoTool
{
    [MenuItem("Tools/Museum/Add Classical Piano")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying || scene.path!="Assets/Scenes/MidtermMuseum.unity")return;
        if(GameObject.Find("Exhibit_ClassicalPiano"))return;
        var root=new GameObject("Exhibit_ClassicalPiano");Undo.RegisterCreatedObjectUndo(root,"Add classical piano");
        root.transform.position=new Vector3(4.8f,0,-11.5f);root.transform.rotation=Quaternion.Euler(0,180,0);
        var black=Mat("Piano_Obsidian",new Color(.025f,.03f,.045f));
        var white=Mat("Piano_Ivory",new Color(.91f,.89f,.81f));
        var brass=Mat("Piano_Brass",new Color(.58f,.39f,.12f));
        Box(root,"Cabinet",new Vector3(0,.95f,.25f),new Vector3(1.65f,1.1f,.5f),black);
        Box(root,"KeyboardBed",new Vector3(0,.79f,-.18f),new Vector3(1.65f,.12f,.5f),black);
        Box(root,"Lid",new Vector3(0,1.52f,.25f),new Vector3(1.76f,.08f,.6f),black);
        for(int i=0;i<35;i++){
            float x=-.73f+i*.043f;
            Box(root,"WhiteKey_"+i,new Vector3(x,.872f,-.24f),new Vector3(.04f,.035f,.32f),white,false);
            if(i<34 && i%7!=2 && i%7!=6)Box(root,"BlackKey_"+i,new Vector3(x+.0215f,.905f,-.15f),new Vector3(.024f,.045f,.18f),black,false);
        }
        for(int i=-1;i<=1;i+=2){
            Box(root,"Leg",new Vector3(i*.72f,.35f,.12f),new Vector3(.14f,.7f,.38f),black);
            Box(root,"BenchLeg",new Vector3(i*.5f,.24f,-.95f),new Vector3(.08f,.48f,.3f),black);
        }
        Box(root,"Bench",new Vector3(0,.5f,-.95f),new Vector3(1.2f,.12f,.38f),black);
        for(int i=-1;i<=1;i++)Box(root,"Pedal",new Vector3(i*.12f,.13f,-.12f),new Vector3(.07f,.035f,.27f),brass);
        Box(root,"ScreenFrame",new Vector3(0,2.35f,.39f),new Vector3(2.3f,1.35f,.08f),black);
        var panel=GameObject.CreatePrimitive(PrimitiveType.Quad);panel.name="PerformanceScreen";panel.transform.SetParent(root.transform,false);
        panel.transform.localPosition=new Vector3(0,2.35f,.34f);panel.transform.localScale=new Vector3(2.2f,1.2375f,1);
        var screenMat=new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        AssetDatabase.CreateAsset(screenMat,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/Piano_Screen.mat"));
        panel.GetComponent<Renderer>().sharedMaterial=screenMat;UnityEngine.Object.DestroyImmediate(panel.GetComponent<Collider>());
        var label=new GameObject("PerformanceStatus",typeof(TextMeshPro));label.transform.SetParent(root.transform,false);
        label.transform.localPosition=new Vector3(0,1.62f,-.04f);label.transform.localRotation=Quaternion.identity;
        var text=label.GetComponent<TextMeshPro>();text.fontSize=1.8f;text.alignment=TextAlignmentOptions.Center;
        text.rectTransform.sizeDelta=new Vector2(2.6f,.3f);text.color=Color.cyan;
        var ex=root.AddComponent<MuseumExhibit>();ex.title="Piano: Sounds of Earth";ex.interactRange=3.3f;ex.keepModeOnOpen=true;
        ex.body="An acoustic piano turns touch into vibration: keys drive hammers against strings.\n\nChoose a performance with 1–3. Press the same selection again, or 4, to stop. Close the panel to watch the screen. Moving away stops playback.\n\nRecordings must be supplied locally before playback is available.";
        ex.meta="Lobby · Classical listening station";ex.themeLink="Long before humans sent their voices into space, these instruments carried their emotions across a room.";
        ex.credit="Piano built from Unity primitives. Performance recordings supplied by the project owner. Mahler reference: Leonard Bernstein, Vienna Philharmonic, 1973.";
        ex.modes=new[]{
            Mode("Rachmaninoff · I", "Piano Concerto No. 2 — I. Moderato. Anna Fedorova."),
            Mode("Rachmaninoff · II", "Piano Concerto No. 2 — II. Adagio sostenuto. User-selected performance."),
            Mode("Mahler · IV", "Symphony No. 5 — IV. Adagietto. Leonard Bernstein, Vienna Philharmonic, 1973."),
            Mode("Stop", "Stop the current performance.")};
        var piano=root.AddComponent<MuseumPiano>();piano.performanceScreen=panel.GetComponent<Renderer>();piano.screenLabel=text;
        piano.recordings=new[]{
            Recording("Rachmaninoff — Moderato","rEGOihjqO9w"),Recording("Rachmaninoff — Adagio sostenuto","JRDH5csoWi8"),Recording("Mahler — Adagietto (Bernstein)","Bj6KLv7kv2Q")};
        root.transform.Find("ScreenFrame").gameObject.SetActive(false);panel.SetActive(false);label.SetActive(false);
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
        Debug.Log("Classical piano and performance screen created; awaiting local recordings.");
    }
    static MuseumExhibit.Mode Mode(string title,string caption)=>new(){label=title,caption=caption+" Local media required if the screen reports missing media.",spinSpeed=0};
    static MuseumPiano.Recording Recording(string title,string id)=>new(){title=title,referenceUrl="https://www.youtube.com/watch?v="+id};
    static Material Mat(string name,Color color){var m=new Material(Shader.Find("Universal Render Pipeline/Lit")){name=name};m.color=color;m.SetFloat("_Smoothness",.7f);AssetDatabase.CreateAsset(m,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/"+name+".mat"));return m;}
    static void Box(GameObject parent,string name,Vector3 position,Vector3 size,Material mat,bool collision=true){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent.transform,false);g.transform.localPosition=position;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=mat;if(!collision)UnityEngine.Object.DestroyImmediate(g.GetComponent<Collider>());}
}
