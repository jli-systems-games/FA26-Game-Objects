using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[InitializeOnLoad]
public static class MuseumPlayerSetupTool
{
    static MuseumPlayerSetupTool(){if(SessionState.GetBool("MuseumVisitorTest",false)){running=true;EditorApplication.update+=Tick;}}
    [MenuItem("Tools/Museum/Add Switchable Visitor")]
    public static void Setup()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying || scene.path!="Assets/Scenes/MidtermMuseum.unity")return;
        if(Object.FindFirstObjectByType<MuseumVisitorController>()!=null)return;
        var camera=Camera.main;
        if(camera==null){Debug.LogError("Main Camera not found");return;}
        var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Starter Assets/Runtime/ThirdPersonController/Prefabs/PlayerArmature.prefab");
        if(prefab==null){Debug.LogError("Starter Assets PlayerArmature missing");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforePlayer.unity"));
        Undo.IncrementCurrentGroup();int group=Undo.GetCurrentGroup();Undo.SetCurrentGroupName("Add museum visitor");
        var player=(GameObject)PrefabUtility.InstantiatePrefab(prefab);
        PrefabUtility.UnpackPrefabInstance(player,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
        player.name="MuseumVisitor";Undo.RegisterCreatedObjectUndo(player,"Create visitor");
        foreach(var component in player.GetComponents<MonoBehaviour>())Object.DestroyImmediate(component);
        foreach(var c in player.GetComponentsInChildren<Camera>())Object.DestroyImmediate(c);
        foreach(var listener in player.GetComponentsInChildren<AudioListener>())Object.DestroyImmediate(listener);
        player.tag="Player";player.transform.position=new Vector3(7.6f,0.08f,-16.8f);
        Vector3 forward=new Vector3(-0.6f,0,0.8f);player.transform.rotation=Quaternion.LookRotation(forward);
        var body=player.GetComponent<CharacterController>();body.height=1.8f;body.radius=0.28f;body.center=new Vector3(0,0.93f,0);body.stepOffset=0.32f;body.skinWidth=0.035f;
        var visitor=player.AddComponent<MuseumVisitorController>();visitor.worldCamera=camera;visitor.firstPerson=true;
        foreach(var behaviour in camera.GetComponents<MonoBehaviour>()){
            if(behaviour.GetType().Namespace!=null && behaviour.GetType().Namespace.StartsWith("Unity.Cinemachine")){
                Undo.RecordObject(behaviour,"Use visitor camera controller");behaviour.enabled=false;}}
        Undo.RecordObject(camera,"Configure visitor camera");camera.nearClipPlane=0.07f;
        Undo.RecordObject(camera.transform,"Place visitor camera");camera.transform.SetPositionAndRotation(player.transform.position+Vector3.up*1.65f,Quaternion.LookRotation(forward));
        // Give the sample character a custom silver and lavender material.
        var mat=new Material(Shader.Find("Universal Render Pipeline/Lit")){name="Visitor_PearlTitanium"};
        mat.SetColor("_BaseColor",new Color(0.58f,0.65f,0.85f,1));mat.SetFloat("_Metallic",0.55f);mat.SetFloat("_Smoothness",0.72f);
        AssetDatabase.CreateAsset(mat,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/Visitor_PearlTitanium.mat"));
        foreach(var r in player.GetComponentsInChildren<Renderer>()){
            var materials=r.sharedMaterials;for(int i=0;i<materials.Length;i++)materials[i]=mat;r.sharedMaterials=materials;}
        var canvas=Object.FindFirstObjectByType<Canvas>();
        if(canvas!=null){
            var panel=new GameObject("VisitorControls",typeof(RectTransform),typeof(CanvasRenderer),typeof(Image));
            Undo.RegisterCreatedObjectUndo(panel,"Create controls HUD");panel.transform.SetParent(canvas.transform,false);
            var rect=panel.GetComponent<RectTransform>();rect.anchorMin=new Vector2(0,0);rect.anchorMax=new Vector2(1,0);rect.pivot=new Vector2(0.5f,0);
            rect.anchoredPosition=new Vector2(0,0);rect.sizeDelta=new Vector2(0,92);
            var image=panel.GetComponent<Image>();image.color=new Color(0.015f,0.025f,0.055f,0.88f);image.raycastTarget=false;
            var textGO=new GameObject("ControlsText",typeof(RectTransform),typeof(CanvasRenderer),typeof(TextMeshProUGUI));textGO.transform.SetParent(panel.transform,false);
            var tr=textGO.GetComponent<RectTransform>();tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=new Vector2(22,8);tr.offsetMax=new Vector2(-22,-8);
            var text=textGO.GetComponent<TextMeshProUGUI>();text.font=TMP_Settings.defaultFontAsset;text.fontSize=22;text.color=new Color(0.8f,0.95f,1,1);text.alignment=TextAlignmentOptions.MidlineLeft;text.raycastTarget=false;
            text.text="CLICK TO EXPLORE  |  FIRST PERSON\nWASD / ARROWS  MOVE     MOUSE  LOOK     SHIFT  RUN     V  VIEW     ESC  CURSOR";visitor.controlsText=text;
            foreach(var raw in canvas.GetComponentsInChildren<RawImage>(true)){Undo.RecordObject(raw,"Keep view overlay click-through");raw.raycastTarget=false;}
        }
        Undo.CollapseUndoOperations(group);Selection.activeGameObject=player;AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Museum visitor saved: first person default, V toggles third person, WASD move, Shift run, click to capture mouse, Esc release. Pixel render target preserved.");
    }
    static bool running;static double started;static Vector3 initial;static bool switched;static MuseumVisitorController live;
    [MenuItem("Tools/Museum/Test Visitor Movement And View")]
    public static void Test(){if(running||EditorApplication.isPlaying)return;running=true;SessionState.SetBool("MuseumVisitorTest",true);started=0;switched=false;live=null;EditorApplication.update+=Tick;EditorApplication.isPlaying=true;}
    static void Tick()
    {
        if(!EditorApplication.isPlaying)return;
        if(live==null){live=Object.FindFirstObjectByType<MuseumVisitorController>();if(live==null)return;started=EditorApplication.timeSinceStartup;initial=live.transform.position;}
        double elapsed=EditorApplication.timeSinceStartup-started;
        live.testMovement=elapsed>0.5 && elapsed<1.5?Vector2.up:Vector2.zero;
        if(elapsed>2 && !switched){live.ToggleView();switched=true;}
        if(elapsed<3)return;
        float distance=Vector3.Distance(initial,live.transform.position);
        bool passed=distance>2 && live.IsGrounded && !live.firstPerson && live.worldCamera.targetTexture!=null;
        string report="passed="+passed+"\ndistance="+distance+"\ngrounded="+live.IsGrounded+"\nthirdPerson="+!live.firstPerson+"\npixelTarget="+(live.worldCamera.targetTexture!=null)+"\nposition="+live.transform.position;
        System.IO.File.WriteAllText("/private/tmp/museum-visitor-test.txt",report);
        live.testMovement=Vector2.zero;EditorApplication.update-=Tick;running=false;SessionState.SetBool("MuseumVisitorTest",false);EditorApplication.isPlaying=false;Debug.Log("Visitor smoke test: "+report);
    }
}
