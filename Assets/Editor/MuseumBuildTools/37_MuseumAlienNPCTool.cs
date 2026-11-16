using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

// Places other alien visitors around the museum (different models from the player's alien, all from Poly Pizza).
// Animated ones (Quaternius, CC0) idle and play a reaction when you come close; the others float and bob.
public static class MuseumAlienNPCTool
{
    struct NPC { public string name,model,near,idle,react; public Vector3 offset; public float size,pitch; public bool floats; public string[] lines; }

    static readonly NPC[] Npcs={
        new NPC{name="NPC_PhoneScholar",model="AlienNPC_Humanoid",near="Exhibit_01_iPhone",offset=new Vector3(-1.9f,0,-1.4f),size=1.5f,pitch=1.0f,idle="Idle",react="Wave",lines=new[]{
            "They stared at this rectangle seven hours a day. Our scholars believe it was staring back.",
            "Every human carried one. None of them could explain how it worked.",
            "I pressed the glass. It asked me for a \"passcode\". Rude."}},
        new NPC{name="NPC_WristCritic",model="AlienNPC_Creature",near="Exhibit_03_Watch",offset=new Vector3(1.9f,0,-1.3f),size=1.0f,pitch=1.5f,idle="Idle",react="Yes",lines=new[]{
            "The wrist one told them to stand up. Records show they did not stand up.",
            "It counted their steps. They had so few steps left. Sad little counter."}},
        new NPC{name="NPC_TwoHeads",model="AlienNPC_CosmicDweller",near="Exhibit_04_SpaceSuit",offset=new Vector3(-2.0f,0,-1.2f),size=1.4f,pitch=0.7f,floats=true,lines=new[]{
            "I tried the suit on. There is no room for the second head.",
            "A whole planet's worth of air, packed into a backpack. Adorable."}},
        new NPC{name="NPC_SaucerParker",model="AlienNPC_LittleGuy",near="Exhibit_06_Lander",offset=new Vector3(1.8f,0,-1.4f),size=1.0f,pitch=1.8f,floats=true,lines=new[]{
            "Seven minutes of terror. I feel that every time I park my saucer.",
            "They threw a parachute at another planet and hoped. Respect."}},
        new NPC{name="NPC_RoverFan",model="AlienNPC_Humanoid",near="Exhibit_07_Perseverance",offset=new Vector3(2.9f,0,-1.6f),size=1.45f,pitch=0.85f,idle="Idle",react="Yes",lines=new[]{
            "It drove slower than my grandmother, and she is a moss.",
            "We found it still working. Nobody had told it the humans were gone."}},
        new NPC{name="NPC_RecordDancer",model="AlienNPC_Creature",near="Exhibit_00_RecordPlayer",offset=new Vector3(1.8f,0,1.0f),size=1.0f,pitch=1.3f,idle="Dance",react="Yes",lines=new[]{
            "Music on a plate! And they never ate it!",
            "Put on record 1. Trust me. I have been dancing for three hundred years."}},
        new NPC{name="NPC_DotWatcher",model="AlienNPC_Classic",near="11_Balcony",offset=new Vector3(-0.8f,0.46f,1.8f),size=1.1f,pitch=0.9f,floats=false,lines=new[]{
            "Every human who ever lived was on that little dot.",
            "I come here to look at it. It is very small. It is very quiet now."}},
    };

    [MenuItem("Tools/Museum/Add Alien Visitor NPCs")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/SceneBackups/MidtermMuseum_BeforeNPCs.unity"));
        System.IO.Directory.CreateDirectory("Assets/MuseumGenerated/NPC");
        var arch=GameObject.Find("SpaceMuseum_Architecture");
        var group=arch.transform.Find("12_AlienVisitors");
        if(group==null){group=new GameObject("12_AlienVisitors").transform;group.SetParent(arch.transform,false);Undo.RegisterCreatedObjectUndo(group.gameObject,"npcs");}
        int n=0;
        foreach(var d in Npcs){
            if(group.Find(d.name)!=null)continue;
            var anchor=GameObject.Find(d.near);var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/MuseumModels/"+d.model+".glb");
            if(anchor==null||asset==null){Debug.LogWarning("Skipped "+d.name+" (missing "+(anchor==null?d.near:d.model)+")");continue;}
            var root=new GameObject(d.name);Undo.RegisterCreatedObjectUndo(root,d.name);root.transform.SetParent(group,false);
            Vector3 p;
            if(d.near=="11_Balcony")p=anchor.transform.TransformPoint(d.offset);           // sitting on the balcony bench
            else{var a=anchor.transform.position;p=new Vector3(a.x+d.offset.x,0,a.z+d.offset.z);} // standing on the museum floor
            root.transform.position=p;
            // Face the exhibit.
            var look=anchor.transform.position-p;look.y=0;if(look.sqrMagnitude>0.01f)root.transform.rotation=Quaternion.LookRotation(look);
            if(d.near=="11_Balcony")root.transform.rotation=anchor.transform.rotation; // looking out to Earth

            var g=(GameObject)PrefabUtility.InstantiatePrefab(asset,root.transform);g.transform.localPosition=Vector3.zero;g.transform.localRotation=Quaternion.identity;
            var b=Bounds(g);if(b.size.y>0.001f)g.transform.localScale*=d.size/b.size.y;
            b=Bounds(g);g.transform.position+=new Vector3(p.x-b.center.x,p.y-b.min.y,p.z-b.center.z);

            var npc=root.AddComponent<MuseumAlienNPC>();npc.lines=d.lines;npc.voicePitch=d.pitch;npc.floats=d.floats;npc.bubbleHeight=d.size+0.4f;
            var col=root.AddComponent<CapsuleCollider>();col.height=d.size;col.center=new Vector3(0,d.size/2,0);col.radius=0.3f;
            if(!string.IsNullOrEmpty(d.idle))npc.animator=SetupAnimator(g,d.model,d.idle,d.react);
            n++;
        }
        EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log(n+" alien visitors added. Walk up to them to hear what they think.");
    }

    // Idle (looping) <-> React (plays once on the "Talk" trigger, then back to Idle).
    public static Animator SetupAnimator(GameObject g,string model,string idle,string react)
    {
        var clips=new Dictionary<string,AnimationClip>();
        foreach(var o in AssetDatabase.LoadAllAssetsAtPath("Assets/MuseumModels/"+model+".glb"))
            if(o is AnimationClip c)foreach(var want in new[]{idle,react})
                if(c.name==want||c.name.EndsWith("|"+want)||c.name.EndsWith("_"+want)){
                    string path="Assets/MuseumGenerated/NPC/"+model+"_"+want+".anim";
                    var copy=AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                    if(copy==null){copy=Object.Instantiate(c);var st=AnimationUtility.GetAnimationClipSettings(copy);st.loopTime=want==idle;AnimationUtility.SetAnimationClipSettings(copy,st);AssetDatabase.CreateAsset(copy,path);}
                    clips[want]=copy;}
        if(!clips.ContainsKey(idle)){Debug.LogWarning(model+": no '"+idle+"' clip");return null;}
        string cp="Assets/MuseumGenerated/NPC/"+model+"_"+idle+"_"+react+".controller";
        var ctrl=AssetDatabase.LoadAssetAtPath<AnimatorController>(cp);
        if(ctrl==null){
            ctrl=AnimatorController.CreateAnimatorControllerAtPath(cp);ctrl.AddParameter("Talk",AnimatorControllerParameterType.Trigger);
            var sm=ctrl.layers[0].stateMachine;var si=sm.AddState("Idle");si.motion=clips[idle];sm.defaultState=si;
            if(clips.TryGetValue(react,out var rc)){
                var sr=sm.AddState("React");sr.motion=rc;
                var t1=si.AddTransition(sr);t1.AddCondition(AnimatorConditionMode.If,0,"Talk");t1.hasExitTime=false;t1.duration=0.15f;
                var t2=sr.AddTransition(si);t2.hasExitTime=true;t2.exitTime=0.95f;t2.duration=0.2f;
            }
        }
        var anim=g.GetComponent<Animator>();if(anim==null)anim=g.AddComponent<Animator>();
        anim.runtimeAnimatorController=ctrl;anim.applyRootMotion=false;
        return anim;
    }
    public static Bounds Bounds(GameObject g){var b=new Bounds();bool f=true;foreach(var r in g.GetComponentsInChildren<Renderer>()){if(f){b=r.bounds;f=false;}else b.Encapsulate(r.bounds);}return b;}
}
