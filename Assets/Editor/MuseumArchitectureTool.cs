using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class MuseumArchitectureTool
{
    const string Folder="Assets/MuseumGenerated";
    static Transform root;
    static Material metal, silver, cyan, purple, amber;
    static Vector3[] points={new Vector3(-4,0,-18),new Vector3(-12,0,-8),new Vector3(-12,0,10),new Vector3(-4,0,18),new Vector3(10,0,14),new Vector3(12,0,-6)};
    static Color C(string hex) { Color c; ColorUtility.TryParseHtmlString(hex,out c); return c; }
    static void Save(Object o,string name) { AssetDatabase.CreateAsset(o,AssetDatabase.GenerateUniqueAssetPath(Folder+"/"+name+".asset")); }
    static Material Lit(string name,Color color,float metallic,float smoothness,Color emission)
    {
        var m=new Material(Shader.Find("Universal Render Pipeline/Lit")){name=name};
        m.SetColor("_BaseColor",color); m.SetFloat("_Metallic",metallic); m.SetFloat("_Smoothness",smoothness);
        if(emission.maxColorComponent>0){m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",emission);}
        Save(m,name); return m;
    }
    static GameObject Box(string name,Vector3 position,Vector3 scale,Material mat,Transform parent,bool collision=true)
    {
        var go=GameObject.CreatePrimitive(PrimitiveType.Cube); go.name=name;
        go.transform.SetParent(parent,false);go.transform.localPosition=position;go.transform.localScale=scale;
        go.GetComponent<Renderer>().sharedMaterial=mat;
        if(!collision) Object.DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }
    static GameObject Beam(string name,Vector3 a,Vector3 b,float width,Material mat,Transform parent,bool collision=false)
    {
        var go=Box(name,(a+b)*0.5f,new Vector3(width,width,(b-a).magnitude),mat,parent,collision);
        go.transform.localRotation=Quaternion.LookRotation((b-a).normalized,Vector3.up);return go;
    }
    static Transform Group(string name){var g=new GameObject(name);g.transform.SetParent(root,false);return g.transform;}
    static Material Glass(string name,Color tint)
    {
        tint.a=0.1f;var m=Lit(name,tint,0,0.96f,Color.black);
        m.SetFloat("_Surface",1);m.SetFloat("_Blend",0);m.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);
        m.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);m.SetFloat("_ZWrite",0);
        m.SetFloat("_Cull",(float)CullMode.Off);m.SetFloat("_BlendModePreserveSpecular",1);
        m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.SetOverrideTag("RenderType","Transparent");m.renderQueue=3000;
        m.SetShaderPassEnabled("ShadowCaster",false);EditorUtility.SetDirty(m);return m;
    }
    [MenuItem("Tools/Museum/Build Space Museum Architecture")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(scene.path!="Assets/Scenes/MidtermMuseum.unity" || EditorApplication.isPlaying) return;
        GameObject museum=null;foreach(var g in scene.GetRootGameObjects())if(g.name=="Museum")museum=g;
        if(museum==null || museum.transform.Find("MuseumFloor_Polygon")==null){Debug.LogError("Polygon floor required.");return;}
        if(museum.transform.Find("SpaceMuseum_Architecture")!=null){Selection.activeGameObject=museum.transform.Find("SpaceMuseum_Architecture").gameObject;return;}
        // Snapshot the saved scene before generating additional architecture.
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath(Folder+"/MidtermMuseum_BeforeArchitecture.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();Undo.SetCurrentGroupName("Build space museum architecture");
        var architecture=new GameObject("SpaceMuseum_Architecture");root=architecture.transform;root.SetParent(museum.transform,false);
        Undo.RegisterCreatedObjectUndo(architecture,"Build architecture");
        metal=Lit("Frame_Obsidian",C("#151B30"),0.72f,0.78f,Color.black);
        silver=Lit("Frame_Titanium",C("#93A5B4"),0.82f,0.8f,Color.black);
        cyan=Lit("Light_Cyan",C("#41E9EB"),0.15f,0.7f,C("#41E9EB")*2.2f);
        purple=Lit("Light_Violet",C("#AB73FF"),0.15f,0.7f,C("#AB73FF")*2.2f);
        amber=Lit("Light_Amber",C("#FFBB70"),0.15f,0.7f,C("#FFBB70")*1.8f);
        var gradient=new Texture2D(256,256,TextureFormat.RGBA32,false){name="Gradient_CyanVioletCoral",wrapMode=TextureWrapMode.Clamp};
        var normal=new Texture2D(128,128,TextureFormat.RGBA32,true,true){name="Normal_FineRibs",wrapMode=TextureWrapMode.Repeat};
        for(int y=0;y<256;y++)for(int x=0;x<256;x++){
            float t=(x+y)/510f;gradient.SetPixel(x,y,t<0.55f?Color.Lerp(C("#1C7887"),C("#59458A"),t/0.55f):Color.Lerp(C("#59458A"),C("#A15F77"),(t-0.55f)/0.45f));}
        gradient.Apply();Save(gradient,gradient.name);
        for(int y=0;y<128;y++)for(int x=0;x<128;x++){
            var n=new Vector3(Mathf.Cos(x*Mathf.PI/8)*0.15f,0,1).normalized;
            normal.SetPixel(x,y,new Color(n.x*0.5f+0.5f,n.y*0.5f+0.5f,n.z*0.5f+0.5f,n.x*0.5f+0.5f));}
        normal.Apply();Save(normal,normal.name);
        var panel=Lit("Panel_Gradient_Ribbed",Color.white,0.48f,0.76f,Color.black);
        panel.SetTexture("_BaseMap",gradient);panel.SetTexture("_BumpMap",normal);panel.SetFloat("_BumpScale",0.3f);panel.EnableKeyword("_NORMALMAP");EditorUtility.SetDirty(panel);
        var glass=new[]{Glass("Glass_Ice",C("#95EDF5")),Glass("Glass_Lilac",C("#C3A2F2")),Glass("Glass_Rose",C("#F1B6C8"))};
        var walls=Group("01_GlassAndFrames");var rails=Group("02_PerimeterLights");
        float[] heights={5.8f,6.5f,7.4f,7f,5.8f,4.2f};
        for(int i=0;i<6;i++){
            Vector3 a=points[i],b=points[(i+1)%6];float h=heights[i];Material light=i<2?cyan:i<4?purple:amber;
            Beam("BaseFrame_"+i,a+Vector3.up*0.12f,b+Vector3.up*0.12f,0.22f,metal,walls,true);
            Beam("EdgeLight_"+i,a+Vector3.up*0.27f,b+Vector3.up*0.27f,0.045f,light,rails);
            Beam("Crown_"+i,a+Vector3.up*h,b+Vector3.up*h,0.23f,silver,walls);
            Beam("CrownGlow_"+i,a+Vector3.up*(h-0.16f),b+Vector3.up*(h-0.16f),0.045f,light,rails);
            Beam("CornerColumn_"+i,a,a+Vector3.up*Mathf.Max(h,heights[(i+5)%6]),0.25f,metal,walls,true);
            float length=Vector3.Distance(a,b);Vector3 direction=(b-a).normalized;
            System.Action<Vector3,Vector3,string> pane=(s,e,n)=>{
                var go=Box(n,(s+e)*0.5f+Vector3.up*(h/2),new Vector3(Vector3.Distance(s,e),h,0.09f),glass[i%3],walls);
                go.transform.localRotation=Quaternion.LookRotation(Vector3.Cross(e-s,Vector3.up).normalized,Vector3.up);
            };
            if(i==5){var mid=(a+b)/2;pane(a,mid-direction*2.2f,"EntranceGlass_A");pane(mid+direction*2.2f,b,"EntranceGlass_B");
                Beam("DoorPost_A",mid-direction*2.2f,mid-direction*2.2f+Vector3.up*h,0.2f,silver,walls,true);
                Beam("DoorPost_B",mid+direction*2.2f,mid+direction*2.2f+Vector3.up*h,0.2f,silver,walls,true);
            } else {pane(a,b,"GlassWall_"+i);int count=Mathf.FloorToInt(length/5);
                for(int j=1;j<=count;j++){var pos=Vector3.Lerp(a,b,(float)j/(count+1));Beam("Mullion_"+i+"_"+j,pos,pos+Vector3.up*h,0.085f,silver,walls);}}
        }
        var roof=Group("03_FloatingRoofRibs");
        for(int i=0;i<4;i++){
            float z=-6+i*5;float h=6.4f+i*0.22f;
            var a=new Vector3(-10.6f,h,z-1.5f);var b=new Vector3(10.2f,h+0.65f,z+1.5f);
            Beam("SlantedRoofRib_"+i,a,b,0.27f,metal,roof);
            Beam("RoofRibbon_"+i,a-Vector3.up*0.18f,b-Vector3.up*0.18f,0.065f,i%2==0?cyan:purple,roof);
        }
        var entrance=Group("04_EntryBridgeAndLobby");Vector3 doorway=(points[5]+points[0])/2;
        Vector3 outward=new Vector3(0.6f,0,-0.8f),side=new Vector3(0.8f,0,0.6f);
        var bridge=Box("ArrivalBridge",doorway+outward*4-Vector3.up*0.25f,new Vector3(4.4f,0.5f,8.5f),panel,entrance);
        bridge.transform.localRotation=Quaternion.LookRotation(outward);
        for(int sign=-1;sign<=1;sign+=2){Vector3 start=doorway+side*sign*2.15f;Vector3 end=start+outward*8;
            Beam("BridgeEdge_"+sign,start+Vector3.up*0.08f,end+Vector3.up*0.08f,0.08f,cyan,entrance);
            Beam("BridgeHandrail_"+sign,start+Vector3.up*1.1f,end+Vector3.up*1.1f,0.07f,silver,entrance);
            var guard=Box("BridgeSafetyGlass_"+sign,(start+end)/2+Vector3.up*0.55f,new Vector3(0.08f,1.1f,8),glass[0],entrance);
            guard.transform.localRotation=Quaternion.LookRotation(outward);}
        // A solid end rail stops accidental falls at the arrival end.
        var endBarrier=Box("ArrivalEndGuard",doorway+outward*8.1f+Vector3.up*0.65f,new Vector3(4.4f,1.3f,0.1f),glass[0],entrance);
        endBarrier.transform.localRotation=Quaternion.LookRotation(outward);
        var lobby=Box("LobbyInformationPlinth",doorway-outward*2+side*3+Vector3.up*0.6f,new Vector3(2,1.2f,0.8f),panel,entrance);
        lobby.transform.localRotation=Quaternion.LookRotation(outward);
        var exhibits=Group("05_ExhibitionPlinths");
        Vector3[] places={new Vector3(-7.5f,0,-5),new Vector3(-7.5f,0,3),new Vector3(-5,0,11),new Vector3(5,0,10),new Vector3(7,0,3),new Vector3(6,0,-4)};
        for(int i=0;i<places.Length;i++){
            var p=places[i];var glow=i<2?cyan:i<4?purple:amber;
            Box("ExhibitBase_"+(i+1),p+Vector3.up*0.48f,new Vector3(2.5f,0.96f,1.5f),metal,exhibits);
            Box("GradientFascia_"+(i+1),p+new Vector3(0,0.6f,-0.76f),new Vector3(2.3f,0.45f,0.035f),panel,exhibits,false);
            Box("LightRim_"+(i+1),p+Vector3.up*0.98f,new Vector3(2.6f,0.045f,1.6f),glow,exhibits,false);
            Box("DisplaySurface_"+(i+1),p+Vector3.up*1.02f,new Vector3(2.5f,0.04f,1.5f),silver,exhibits);
            // Actual raised ribs on the pedestal fascia complement the normal texture.
            for(int j=0;j<9;j++)Box("RaisedRib_"+i+"_"+j,p+new Vector3(-1+j*0.25f,0.55f,-0.8f),new Vector3(0.025f,0.6f,0.065f),silver,exhibits,false);
        }
        var center=GameObject.CreatePrimitive(PrimitiveType.Cylinder);center.name="CentralFeaturePodium";center.transform.SetParent(exhibits,false);
        center.transform.localPosition=new Vector3(0,0.18f,3);center.transform.localScale=new Vector3(4.5f,0.18f,4.5f);center.GetComponent<Renderer>().sharedMaterial=panel;
        var floor= museum.transform.Find("MuseumFloor_Polygon").GetComponent<Renderer>();Undo.RecordObject(floor,"Assign gradient floor material");
        var original=AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumFloor.mat");
        if(original!=null){var floorMat=new Material(original){name="Floor_GlassGradient"};floorMat.SetTexture("_BaseMap",gradient);floorMat.SetColor("_BaseColor",new Color(0.65f,0.85f,1,0.34f));Save(floorMat,floorMat.name);floor.sharedMaterial=floorMat;}
        var lighting=Group("06_AccentLights");
        for(int i=0;i<3;i++){
            var g=new GameObject("ZoneLight_"+i);g.transform.SetParent(lighting,false);g.transform.localPosition=new Vector3(i==0?-6:i==1?6:0,4,i==2?12:0);
            var l=g.AddComponent<Light>();l.type=LightType.Point;l.color=i==0?C("#79E9F2"):i==1?C("#B393F7"):C("#FFD3A1");l.intensity=2.2f;l.range=12;l.shadows=LightShadows.None;
        }
        var probeGO=new GameObject("InteriorReflectionProbe");probeGO.transform.SetParent(lighting,false);probeGO.transform.localPosition=new Vector3(0,3,0);
        var probe=probeGO.AddComponent<ReflectionProbe>();probe.mode=ReflectionProbeMode.Baked;probe.size=new Vector3(27,12,40);probe.resolution=128;probe.boxProjection=true;
        // Save an entrance view while retaining the render-texture setup.
        foreach(var g in scene.GetRootGameObjects())if(g.name=="Main Camera"){
            Undo.RecordObject(g.transform,"Frame architecture from entrance");g.transform.position=doorway+outward*6.7f+Vector3.up*1.8f;
            g.transform.rotation=Quaternion.LookRotation(new Vector3(0,2.7f,4)-g.transform.position);}
        Selection.activeGameObject=architecture;
        if(SceneView.lastActiveSceneView!=null){SceneView.lastActiveSceneView.LookAt(new Vector3(0,1,0),Quaternion.Euler(34,-28,0),34,false);}
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Space museum architecture saved: tinted glass, asymmetric crown, entry bridge, gradient ribbed panels, six empty display plinths and reflection probe. Exhibits and player still to add.");
    }
}
