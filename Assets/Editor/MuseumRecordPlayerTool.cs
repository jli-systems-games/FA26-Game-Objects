using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 1) Makes the three interior fins transparent (glass panel; collider, foot and light line kept).
// 2) Puts a record player on the lobby desk: press E, choose one of three records, music keeps playing after closing.
public static class MuseumRecordPlayerTool
{
    [MenuItem("Tools/Museum/Glass Fins + Lobby Record Player")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MidtermMuseum_BeforeRecordPlayer.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        var glass=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Glass_Ice.asset");
        var shape=GameObject.Find("07_ShapeRefinement");
        if(shape!=null && glass!=null)
            foreach(Transform fin in shape.transform){
                var panel=fin.Find("Panel");if(panel==null)continue;
                var r=panel.GetComponent<Renderer>();Undo.RecordObject(r,"glass fin");r.sharedMaterial=glass;
                r.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;
            }

        var desk=GameObject.Find("LobbyInformationPlinth");
        var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/RecordPlayer.glb");
        if(desk!=null && asset!=null && GameObject.Find("Exhibit_00_RecordPlayer")==null){
            var top=desk.GetComponent<Renderer>().bounds;
            var root=new GameObject("Exhibit_00_RecordPlayer");Undo.RegisterCreatedObjectUndo(root,"record player");
            root.transform.SetParent(desk.transform.parent,false);
            root.transform.position=new Vector3(top.center.x,top.max.y,top.center.z);
            root.transform.rotation=desk.transform.rotation*Quaternion.Euler(0,180,0);
            var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,root.transform);g.name="RecordPlayer";
            g.transform.localRotation=Quaternion.identity;
            var b=Bounds(g);float max=Mathf.Max(b.size.x,b.size.y,b.size.z);if(max>0.0001f)g.transform.localScale*=0.65f/max;
            b=Bounds(g);g.transform.position+=new Vector3(top.center.x-b.center.x,top.max.y-b.min.y,top.center.z-b.center.z);

            var src=root.AddComponent<AudioSource>();
            src.playOnAwake=true;src.loop=true;src.volume=0.45f;src.spatialBlend=0.35f;src.minDistance=6;src.maxDistance=40;
            var c1=Clip("StarfieldRomance.mp3");var c2=Clip("StellerDreams.ogg");var c3=Clip("FloatingInSpace.wav");
            src.clip=c1; // lobby music from the start

            var e=root.AddComponent<MuseumExhibit>();
            e.musicSource=src;e.interactRange=2.6f;
            e.title="Record Player: Sounds of Earth";
            e.meta="Lobby  ·  Choose a record  ·  Voyager Golden Records launched 1977";
            e.body="For most of the 20th century, music lived on vinyl: a groove cut into a disc, read by a needle. "+
                   "Listening meant owning a physical object and a machine to play it. Today the same songs travel invisibly to the phone in your pocket.\n\n"+
                   "In 1977 NASA sent two gold-plated records into space on Voyager 1 and 2, carrying greetings, sounds and music from Earth for anyone who might find them.";
            e.themeLink="This museum starts where Voyager's record ends: technology carrying human life outward. Pick a record — it keeps playing while you explore.";
            e.credit="Model: \"Record Player\" by Ginsta, CC BY 3.0, via Poly Pizza. Music (CC0, OpenGameArt): Starfield Romance – Yoiyami; Steller Dreams – Synth-thetic; Floating in Space – Umplix.";
            e.modes=new[]{
                new MuseumExhibit.Mode{label="Starfield Romance",caption="Record 1 — 'Starfield Romance' by Yoiyami (CC0). Soft piano for the lobby. Press 1 again to stop.",music=c1,screenColor=Color.white},
                new MuseumExhibit.Mode{label="Steller Dreams",caption="Record 2 — 'Steller Dreams' by Synth-thetic (CC0). Slow synths, like drifting in orbit. Press 2 again to stop.",music=c2,screenColor=Color.white},
                new MuseumExhibit.Mode{label="Floating in Space",caption="Record 3 — 'Floating in Space' by Umplix (CC0). Pure ambience. Press 3 again to stop.",music=c3,screenColor=Color.white},
            };
            var ring=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ring.name="FocusRing";Object.DestroyImmediate(ring.GetComponent<Collider>());
            ring.transform.SetParent(root.transform,false);ring.transform.localScale=new Vector3(1.0f,0.003f,1.0f);ring.transform.localPosition=new Vector3(0,0.003f,0);
            ring.GetComponent<Renderer>().sharedMaterial=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumGenerated/Exhibit_FocusGlow.mat");
            ring.SetActive(false);e.focusHighlight=ring;
        } else if(desk==null||asset==null) Debug.LogError("Need LobbyInformationPlinth and Assets/MuseumModels/RecordPlayer.glb");

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Fins are glass; record player placed on the lobby desk.");
    }
    static AudioClip Clip(string f)=>AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/MuseumAudio/"+f);
    static Bounds Bounds(GameObject g){var rs=g.GetComponentsInChildren<Renderer>();var b=rs.Length>0?rs[0].bounds:new Bounds(g.transform.position,Vector3.zero);foreach(var r in rs)b.Encapsulate(r.bounds);return b;}
}
