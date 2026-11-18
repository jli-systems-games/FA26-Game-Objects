using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// Explicit request files allow validation in the already-open editor, preserving its scene.
[InitializeOnLoad]
public static class MuseumPianoValidation
{
    static readonly string Folder="C:/Users/isabe/Documents/Codex/2026-11-18/unity-midterm-improvements/work";
    static int phase=-1;
    static double due;
    static int loungePhase=-1;static double loungeDue;static Vector3 standPosition;
    static MuseumPianoValidation(){EditorApplication.update+=Tick;Directory.CreateDirectory(Folder);File.WriteAllText(Path.Combine(Folder,"piano-ready.txt"),"v4");}
    static void Tick()
    {
        var label=GameObject.Find("Exhibit_ClassicalPiano/PerformanceStatus");
        if(label && Quaternion.Angle(label.transform.localRotation,Quaternion.identity)>0.01f){
            Undo.RecordObject(label.transform,"Correct piano text facing");label.transform.localRotation=Quaternion.identity;
            if(!EditorApplication.isPlaying)UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(label.scene);
        }
        if(EditorApplication.isPlaying && SessionState.GetBool("PianoValidation",false))RunValidation();
        if(EditorApplication.isPlaying && SessionState.GetBool("LoungeValidation",false))RunLoungeValidation();
        var request=Path.Combine(Folder,"piano-request.txt");
        if(EditorApplication.isCompiling || !File.Exists(request))return;
        string command;
        try{command=File.ReadAllText(request).Trim();File.Delete(request);}catch(IOException){return;}
        try{
            if(command=="cleanfilter"){
                var noise=GameObject.Find("ColorNoise");if(noise){Undo.RecordObject(noise,"Remove distracting screen noise");noise.SetActive(false);if(!EditorApplication.isPlaying)EditorSceneManagerSave(noise);}
                File.WriteAllText(Path.Combine(Folder,"filter-result.txt"),"ColorNoise disabled; pixel rendering and lighting unchanged. Playing="+EditorApplication.isPlaying);
            }
            if(command=="chaise"){MuseumChaiseTool.Build();MuseumChaiseTool.Preview();}
            if(command=="chaisepreview")MuseumChaiseTool.Preview();
            if(command=="lounge")MuseumViewingLoungeTool.Build();
            if(command=="loungetest"){SessionState.SetBool("LoungeValidation",true);loungePhase=-1;EditorApplication.isPlaying=true;}
            if(command=="rugwalk"){
                var visitor=UnityEngine.Object.FindFirstObjectByType<MuseumVisitorController>();var controller=visitor.GetComponent<CharacterController>();var record=GameObject.Find("Exhibit_00_RecordPlayer");
                visitor.enabled=false;controller.enabled=false;visitor.transform.position=record.transform.position+new Vector3(1f,.08f,2.1f);controller.enabled=true;Physics.SyncTransforms();
                Vector3 start=visitor.transform.position;for(int i=0;i<60;i++)controller.Move(new Vector3(0,-.015f,-.03f));
                float distance=start.z-visitor.transform.position.z;bool onRug=false;
                foreach(var hit in Physics.RaycastAll(visitor.transform.position+Vector3.up*.4f,Vector3.down,1f,~0,QueryTriggerInteraction.Ignore))if(hit.collider.transform.IsChildOf(record.transform))onRug=true;
                File.WriteAllText(Path.Combine(Folder,"rug-walk.txt"),"Travel="+distance+" start="+start+" end="+visitor.transform.position+" OnRecordRug="+onRug);
                LoungeCheck(distance>1.5f && onRug,"Character walks from floor onto record-player rug");visitor.enabled=true;
            }
            if(command=="loungeinspect"){
                var record=GameObject.Find("Exhibit_00_RecordPlayer");var piano=GameObject.Find("Exhibit_ClassicalPiano");var dancer=GameObject.Find("NPC_RecordDancer");var report=new System.Text.StringBuilder();
                report.AppendLine("Record="+record.transform.position+" piano="+piano.transform.position+" dancer="+dancer.transform.position);
                foreach(var collider in UnityEngine.Object.FindObjectsByType<Collider>(FindObjectsSortMode.None)){
                    if(Vector3.Distance(collider.bounds.ClosestPoint(record.transform.position),record.transform.position)>4)continue;
                    var path=collider.name;for(var parent=collider.transform.parent;parent;parent=parent.parent)path=parent.name+"/"+path;
                    report.AppendLine(path+" "+collider.GetType().Name+" trigger="+collider.isTrigger+" bounds="+collider.bounds);
                }
                File.WriteAllText(Path.Combine(Folder,"lounge-inspect.txt"),report.ToString());
            }
            if(command=="grand")MuseumGrandPianoTool.Build();
            if(command=="orient"){
                var root=GameObject.Find("Exhibit_ClassicalPiano");var model=root.transform.Find("GrandPianoModel");model.localRotation=Quaternion.Euler(-90,0,0);
                var renderers=model.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
                model.position+=new Vector3(root.transform.position.x-bounds.center.x,root.transform.position.y-bounds.min.y,root.transform.position.z-bounds.center.z);
                EditorSceneManagerSave(root);
            }
            if(command=="preview"){
                var root=GameObject.Find("Exhibit_ClassicalPiano");var cameraObject=new GameObject("PianoPreviewCamera");var camera=cameraObject.AddComponent<Camera>();
                var transforms=root.transform.Find("GrandPianoModel").GetComponentsInChildren<Transform>();var layers=new int[transforms.Length];for(int i=0;i<transforms.Length;i++){layers[i]=transforms[i].gameObject.layer;transforms[i].gameObject.layer=31;}
                camera.cullingMask=1<<31;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.12f,.14f,.18f);
                camera.transform.position=root.transform.position+new Vector3(2.6f,2.3f,3f);camera.transform.LookAt(root.transform.position+Vector3.up*.7f);camera.fieldOfView=45;camera.farClipPlane=100;
                var rt=new RenderTexture(1280,720,24);camera.targetTexture=rt;camera.Render();var previous=RenderTexture.active;RenderTexture.active=rt;var image=new Texture2D(1280,720,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,1280,720),0,0);image.Apply();File.WriteAllBytes(Path.Combine(Folder,"grand-preview.png"),image.EncodeToPNG());RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(image);UnityEngine.Object.DestroyImmediate(cameraObject);rt.Release();UnityEngine.Object.DestroyImmediate(rt);
                for(int i=0;i<transforms.Length;i++)transforms[i].gameObject.layer=layers[i];
            }
            if(command=="build"){
                if(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)throw new Exception("Unsaved scene: build deferred to protect current edits.");
                MuseumPianoTool.Build();
                File.WriteAllText(Path.Combine(Folder,"piano-result.txt"),GameObject.Find("Exhibit_ClassicalPiano")?"BUILD PASS":"BUILD FAILED");
            }
            if(command=="play")EditorApplication.isPlaying=true;
            if(command=="test"){SessionState.SetBool("PianoValidation",true);phase=-1;EditorApplication.isPlaying=true;}
            if(command=="media"){
                var wrong=Camera.main.GetComponent<MuseumComfortView>();if(wrong)UnityEngine.Object.DestroyImmediate(wrong);
                var stray=Camera.main.GetComponent<UnityEngine.UI.RawImage>();if(stray)UnityEngine.Object.DestroyImmediate(stray);
                var strayRenderer=Camera.main.GetComponent<CanvasRenderer>();if(strayRenderer)UnityEngine.Object.DestroyImmediate(strayRenderer);
                Camera.main.rect=new Rect(0,0,1,1);
                var pixel=GameObject.Find("PixelRender");
                if(pixel && !pixel.GetComponent<MuseumComfortView>())pixel.AddComponent<MuseumComfortView>();
                if(pixel && !GameObject.Find("PixelLetterbox")){
                    var bars=new GameObject("PixelLetterbox",typeof(RectTransform),typeof(UnityEngine.UI.Image));bars.transform.SetParent(pixel.transform.parent,false);bars.transform.SetSiblingIndex(pixel.transform.GetSiblingIndex());
                    var rect=(RectTransform)bars.transform;rect.anchorMin=Vector2.zero;rect.anchorMax=Vector2.one;rect.offsetMin=rect.offsetMax=Vector2.zero;
                    var image=bars.GetComponent<UnityEngine.UI.Image>();image.color=Color.black;image.raycastTarget=false;
                }
                var p=UnityEngine.Object.FindFirstObjectByType<MuseumPiano>();
                string[] names={"Rachmaninoff_I","Rachmaninoff_II","Mahler_Adagietto"};
                for(int i=0;i<3;i++)p.recordings[i].video=AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>("Assets/MuseumMedia/"+names[i]+".mp4");
                p.recordings[0].startSeconds=5;p.recordings[0].endSeconds=697;
                var ex=p.GetComponent<MuseumExhibit>();ex.body=ex.body.Replace("Recordings must be supplied locally before playback is available.","Three local concert recordings are available. The Fedorova recording plays only the first movement (00:05–11:37).");ex.modes[1].caption="Piano Concerto No. 2 — II. Adagio sostenuto. Khatia Buniatishvili.";EditorUtility.SetDirty(ex);
                EditorUtility.SetDirty(p);UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(p.gameObject.scene);UnityEditor.SceneManagement.EditorSceneManager.SaveScene(p.gameObject.scene);
                File.WriteAllText(Path.Combine(Folder,"piano-result.txt"),"MEDIA "+string.Join(",",Array.ConvertAll(p.recordings,r=>r.video?r.video.name+" audio="+r.video.audioTrackCount+" seconds="+r.video.length:"MISSING")));
            }
            if(command=="stop")EditorApplication.isPlaying=false;
            if(command=="inspect"){
                var p=UnityEngine.Object.FindFirstObjectByType<MuseumPiano>();var visitor=UnityEngine.Object.FindFirstObjectByType<MuseumVisitorController>();
                File.WriteAllText(Path.Combine(Folder,"piano-result.txt"),"Playing="+EditorApplication.isPlaying+" Piano="+(p!=null)+" Camera aspect="+Camera.main.aspect+" rect="+Camera.main.rect+" FOV="+Camera.main.fieldOfView+" Visitor="+visitor.transform.position+" Status="+(p?p.Status:"missing"));
            }
        }catch(Exception error){File.WriteAllText(Path.Combine(Folder,"piano-result.txt"),error.ToString());Debug.LogException(error);}
    }
    static void Check(bool condition,string message){File.AppendAllText(Path.Combine(Folder,"piano-tests.txt"),(condition?"PASS ":"FAIL ")+message+"\n");}
    static void LoungeCheck(bool pass,string message){File.AppendAllText(Path.Combine(Folder,"lounge-tests.txt"),(pass?"PASS ":"FAIL ")+message+"\n");}
    static void RunLoungeValidation()
    {
        if(EditorApplication.timeSinceStartup<loungeDue)return;
        var sofa=UnityEngine.Object.FindFirstObjectByType<MuseumViewingSofa>();var visitor=UnityEngine.Object.FindFirstObjectByType<MuseumVisitorController>();if(!sofa || !visitor)return;
        var body=visitor.GetComponent<CharacterController>();var npc=GameObject.Find("NPC_RecordDancer").GetComponent<MuseumAlienNPC>();
        if(loungePhase<0){loungePhase=0;loungeDue=EditorApplication.timeSinceStartup+2;return;}
        try{
            if(loungePhase==0){
                File.WriteAllText(Path.Combine(Folder,"lounge-tests.txt"),"Unity "+Application.unityVersion+"\n");
                UnityEngine.Object.FindFirstObjectByType<MuseumMenu>().StartGame();body.enabled=false;visitor.transform.position=sofa.transform.position+sofa.transform.forward*1.3f+Vector3.up*.08f;body.enabled=true;visitor.SetLocked(false);standPosition=visitor.transform.position;
                var piano=UnityEngine.Object.FindFirstObjectByType<MuseumPiano>();piano.GetComponent<MuseumExhibit>().SetInspecting(true);piano.Select(1);piano.GetComponent<MuseumExhibit>().SetInspecting(false);
            }
            if(loungePhase==1){standPosition=visitor.transform.position;sofa.Recline();LoungeCheck(sofa.IsReclining && !body.enabled && !visitor.enabled,"Sofa reclines player and locks movement");}
            if(loungePhase==2){LoungeCheck(Vector3.Dot(visitor.worldCamera.transform.forward,(sofa.screen.position-sofa.headRest.position).normalized)>.98f,"Reclining view faces video screen");
                var menu=UnityEngine.Object.FindFirstObjectByType<MuseumMenu>();menu.TogglePause();
                LoungeCheck(menu.MenuOpen && sofa.IsReclining && Time.timeScale==0,"Tab pause opens while reclining");
                menu.TogglePause();LoungeCheck(!menu.MenuOpen && sofa.IsReclining && visitor.inputLocked && !visitor.enabled && Time.timeScale==1,"Resume preserves reclining controls");
                LoungeCheck(Mathf.Abs(Vector3.Dot(visitor.worldCamera.transform.right,Vector3.up))<.001f,"Reclining camera has no sideways tilt");
                var camera=visitor.worldCamera;camera.Render();var rt=camera.targetTexture;var previous=RenderTexture.active;RenderTexture.active=rt;var image=new Texture2D(rt.width,rt.height,TextureFormat.RGB24,false);image.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);image.Apply();File.WriteAllBytes(Path.Combine(Folder,"chaise-view.png"),image.EncodeToPNG());RenderTexture.active=previous;UnityEngine.Object.DestroyImmediate(image);
                LoungeCheck(Physics.OverlapSphere(sofa.headRest.position,.08f).Length==0,"Reclining camera is clear of colliders");
                sofa.StandUp();LoungeCheck(!sofa.IsReclining && body.enabled && visitor.enabled && Vector3.Distance(visitor.transform.position,standPosition)<.05f,"Getting up restores standing position and controls");}
            if(loungePhase==3){body.enabled=false;visitor.transform.position=npc.transform.position+Vector3.forward*2.2f+Vector3.up*.08f;body.enabled=true;visitor.inputLocked=false;visitor.SetLocked(false);visitor.enabled=false;visitor.worldCamera.transform.position=visitor.transform.position+Vector3.up*1.6f;visitor.worldCamera.transform.LookAt(npc.transform.position+Vector3.up*.7f);}
            if(loungePhase==4){LoungeCheck(npc.CanTalk,"Record dancer can be selected at 2.2 metres");LoungeCheck(!npc.IsTalking,"Passing nearby does not automatically trigger dialogue");npc.Talk();}
            if(loungePhase==5){
                LoungeCheck(npc.IsTalking,"Explicit talk starts record dancer dialogue");visitor.enabled=true;
                var record=GameObject.Find("Exhibit_00_RecordPlayer");LoungeCheck(!record.GetComponent<BoxCollider>().enabled,"Oversized record-player box is disabled");LoungeCheck(record.GetComponentsInChildren<MeshCollider>().Length>0,"Record furniture uses surface mesh collision");
                var report=new System.Text.StringBuilder();foreach(var renderer in record.GetComponentsInChildren<Renderer>())report.AppendLine(renderer.name+" "+renderer.bounds);File.WriteAllText(Path.Combine(Folder,"rug-meshes.txt"),report.ToString());
                SessionState.SetBool("LoungeValidation",false);File.AppendAllText(Path.Combine(Folder,"lounge-tests.txt"),"DONE\n");return;
            }
            loungePhase++;loungeDue=EditorApplication.timeSinceStartup+1;
        }catch(Exception error){SessionState.SetBool("LoungeValidation",false);File.AppendAllText(Path.Combine(Folder,"lounge-tests.txt"),error+"\n");}
    }
    static void EditorSceneManagerSave(GameObject root){UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(root.scene);UnityEditor.SceneManagement.EditorSceneManager.SaveScene(root.scene);}
    static void RunValidation()
    {
        if(EditorApplication.timeSinceStartup<due)return;
        var piano=UnityEngine.Object.FindFirstObjectByType<MuseumPiano>();
        if(!piano)return;
        var visitor=UnityEngine.Object.FindFirstObjectByType<MuseumVisitorController>();
        var interactor=visitor.GetComponent<MuseumExhibitInteractor>();
        var exhibit=piano.GetComponent<MuseumExhibit>();
        var video=piano.GetComponent<UnityEngine.Video.VideoPlayer>();
        if(phase<=6)exhibit.SetInspecting(true);
        if(phase==-1){phase=0;due=EditorApplication.timeSinceStartup+3;return;}
        try{
            if(phase==0){
                Check(!piano.performanceScreen.gameObject.activeSelf,"TV hidden before playback");
                File.WriteAllText(Path.Combine(Folder,"piano-tests.txt"),"Unity "+Application.unityVersion+"\n");
                UnityEngine.Object.FindFirstObjectByType<MuseumMenu>().StartGame();
                var body=visitor.GetComponent<CharacterController>();body.enabled=false;visitor.transform.position=piano.transform.position-piano.transform.forward*2+Vector3.up*.1f;body.enabled=true;
                typeof(MuseumExhibitInteractor).GetMethod("Open",System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic).Invoke(interactor,new object[]{exhibit});
                Check(interactor.IsReading && visitor.inputLocked,"Open piano panel locks movement");
                piano.Select(0);
            }
            if(phase>=1 && phase<=3){
                var keys=piano.GetComponentInChildren<MuseumPianoKeys>();
                Check(keys && keys.PressedKeyCount>0,"Keys move during performance "+phase);
                Check(piano.performanceScreen.gameObject.activeSelf,"TV visible during performance "+phase);
                Check(video.clip==piano.recordings[phase-1].video && video.isPlaying && video.frame>0,"Recording "+phase+" decodes and advances; frame="+video.frame+" time="+video.time+" status="+piano.Status);
                Check(video.clip && video.clip.audioTrackCount>0,"Recording "+phase+" contains audio");
                Check(piano.GetComponent<AudioSource>().volume>.5f,"Recording "+phase+" fades in");
                if(phase<3)piano.Select(phase);
                else piano.Select(2);
            }
            if(phase==4){Check(!video.isPlaying,"Same selection stops playback");Check(!piano.performanceScreen.gameObject.activeSelf,"TV hides after stopping");Check(piano.GetComponentInChildren<MuseumPianoKeys>().PressedKeyCount==0,"Keys return to rest");piano.Select(1);}
            if(phase==5){piano.Select(3);}
            if(phase==6){Check(!video.isPlaying,"Stop button stops playback");piano.Select(2);interactor.Close();visitor.SetLocked(false);}
            if(phase==7){
                Check(video.isPlaying,"Closing panel keeps screen playback running");
                var pixel=GameObject.Find("PixelRender").GetComponent<RectTransform>();
                Check(Mathf.Abs(pixel.rect.width/pixel.rect.height-16f/9)<.005f,"Pixel image preserves 16:9: "+pixel.rect.size);
                ScreenCapture.CaptureScreenshot(Path.Combine(Folder,"piano-live.png"));
                var body=visitor.GetComponent<CharacterController>();body.enabled=false;visitor.transform.position+=Vector3.right*8;body.enabled=true;
            }
            if(phase==8){Check(!video.isPlaying,"Leaving piano range stops playback");piano.StopPlayback();SessionState.SetBool("PianoValidation",false);phase=-1;File.AppendAllText(Path.Combine(Folder,"piano-tests.txt"),"DONE\n");return;}
            phase++;due=EditorApplication.timeSinceStartup+5;EditorApplication.QueuePlayerLoopUpdate();
        }catch(Exception error){File.AppendAllText(Path.Combine(Folder,"piano-tests.txt"),"ERROR "+error+"\n");SessionState.SetBool("PianoValidation",false);}
    }
}
