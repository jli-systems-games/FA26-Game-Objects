using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Second pass on the building silhouette. Adds, never deletes:
//  - a faceted glass "crystal" roof rising to an off-centre peak (replaces the 4 heavy black beams, which are hidden)
//  - silver ribs from the peak down to every corner
//  - a slanted entrance canopy over the arrival bridge
//  - three free-standing angled fins that separate the lobby and the exhibition zones (with colliders)
public static class MuseumShapeTool
{
    const string Folder="Assets/MuseumGenerated";
    // Same outline and crown heights as MuseumArchitectureTool.
    static readonly Vector3[] P={new Vector3(-4,0,-18),new Vector3(-12,0,-8),new Vector3(-12,0,10),new Vector3(-4,0,18),new Vector3(10,0,14),new Vector3(12,0,-6)};
    static readonly float[] H={5.8f,6.5f,7.4f,7f,5.8f,4.2f};
    static readonly Vector3 Peak=new Vector3(-3,11.5f,5);

    static Material M(string n)=>AssetDatabase.LoadAssetAtPath<Material>(Folder+"/"+n+".asset");

    static GameObject Box(string name,Transform parent,Vector3 pos,Vector3 scale,Quaternion rot,Material m,bool collider)
    {
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent,false);
        g.transform.localPosition=pos;g.transform.localRotation=rot;g.transform.localScale=scale;
        g.GetComponent<Renderer>().sharedMaterial=m;if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
    }
    static void Beam(string name,Transform parent,Vector3 a,Vector3 b,float w,Material m)
        =>Box(name,parent,(a+b)/2,new Vector3(w,w,(b-a).magnitude),Quaternion.LookRotation(b-a),m,false);

    [MenuItem("Tools/Museum/Refine Building Shape")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        var arch=GameObject.Find("SpaceMuseum_Architecture");
        if(arch==null){Debug.LogError("SpaceMuseum_Architecture not found");return;}
        if(arch.transform.Find("07_ShapeRefinement")!=null){Debug.Log("Already refined.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath(Folder+"/MidtermMuseum_BeforeShape.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();
        var metal=M("Frame_Obsidian");var silver=M("Frame_Titanium");var cyan=M("Light_Cyan");var violet=M("Light_Violet");
        var amber=M("Light_Amber");var panel=M("Panel_Gradient_Ribbed");var glass=M("Glass_Lilac");

        var root=new GameObject("07_ShapeRefinement").transform;root.SetParent(arch.transform,false);
        Undo.RegisterCreatedObjectUndo(root.gameObject,"shape");

        // Hide the old floating beams (kept in the scene).
        var ribs=arch.transform.Find("03_FloatingRoofRibs");
        if(ribs!=null){Undo.RecordObject(ribs.gameObject,"hide");ribs.gameObject.SetActive(false);}

        // ----- Faceted crystal roof -----
        var roofGO=new GameObject("CrystalRoof",typeof(MeshFilter),typeof(MeshRenderer));roofGO.transform.SetParent(root,false);
        var verts=new System.Collections.Generic.List<Vector3>();var tris=new System.Collections.Generic.List<int>();
        System.Action<Vector3,Vector3,Vector3> Tri=(a,b,c)=>{int i=verts.Count;verts.Add(a);verts.Add(b);verts.Add(c);tris.Add(i);tris.Add(i+1);tris.Add(i+2);};
        for(int i=0;i<6;i++){
            Vector3 a=P[i]+Vector3.up*H[i],b=P[(i+1)%6]+Vector3.up*H[i];
            // Split each face at a lifted midpoint so the roof reads as folded crystal, not a tent.
            Vector3 mid=Vector3.Lerp(a,b,0.5f);Vector3 fold=Vector3.Lerp(mid,Peak,0.45f)+Vector3.up*(i%2==0?0.9f:-0.4f);
            Tri(a,fold,b);Tri(a,Peak,fold);Tri(fold,Peak,b);
            // Close the step between neighbouring crown heights at each corner.
            Vector3 c=P[(i+1)%6];Vector3 lo=c+Vector3.up*Mathf.Min(H[i],H[(i+1)%6]),hi=c+Vector3.up*Mathf.Max(H[i],H[(i+1)%6]);
            Tri(lo,Peak,hi);
            Beam("FoldRib_"+i,root,fold,Peak,0.06f,silver);Beam("FoldEdgeA_"+i,root,a,fold,0.05f,silver);Beam("FoldEdgeB_"+i,root,fold,b,0.05f,silver);
            Beam("PeakRib_"+i,root,c+Vector3.up*Mathf.Max(H[i],H[(i+1)%6]),Peak,0.12f,i%2==0?metal:silver);
            if(i%2==0)Beam("PeakGlow_"+i,root,c+Vector3.up*(Mathf.Max(H[i],H[(i+1)%6])-0.12f),Peak-Vector3.up*0.12f,0.04f,i<2?cyan:i<4?violet:amber);
        }
        var mesh=new Mesh{name="CrystalRoofMesh"};mesh.SetVertices(verts);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();mesh.RecalculateBounds();
        AssetDatabase.CreateAsset(mesh,AssetDatabase.GenerateUniqueAssetPath(Folder+"/CrystalRoofMesh.asset"));
        roofGO.GetComponent<MeshFilter>().sharedMesh=mesh;roofGO.GetComponent<MeshRenderer>().sharedMaterial=glass;
        roofGO.GetComponent<MeshRenderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;

        // ----- Entrance canopy (cantilevered, tilted up toward the bridge) -----
        Vector3 door=(P[5]+P[0])/2,outward=new Vector3(0.6f,0,-0.8f);
        var rot=Quaternion.LookRotation(outward)*Quaternion.Euler(-9,0,0);
        Box("EntranceCanopy",root,door+outward*2.6f+Vector3.up*4.4f,new Vector3(5.6f,0.16f,6.2f),rot,panel,false);
        Box("EntranceCanopyGlow",root,door+outward*2.6f+Vector3.up*4.3f,new Vector3(5.7f,0.04f,6.3f),rot,cyan,false);
        Beam("CanopyStrut_A",root,door+new Vector3(0.8f,0,0.6f)*2.2f+Vector3.up*4.2f,door+outward*5.4f+new Vector3(0.8f,0,0.6f)*2.2f+Vector3.up*4.9f,0.1f,silver);
        Beam("CanopyStrut_B",root,door-new Vector3(0.8f,0,0.6f)*2.2f+Vector3.up*4.2f,door+outward*5.4f-new Vector3(0.8f,0,0.6f)*2.2f+Vector3.up*4.9f,0.1f,silver);

        // ----- Angled fins: lobby screen + zone dividers (solid, walkable around) -----
        Fin(root,"Fin_LobbyScreen",new Vector3(-0.5f,0,-7.6f),6.0f,3.8f,-14,panel,cyan);
        Fin(root,"Fin_Zone_SpaceBody",new Vector3(0.2f,0,13.7f),3.2f,3.4f,72,panel,violet);
        Fin(root,"Fin_Zone_Mars",new Vector3(-10.4f,0,-1.0f),2.6f,3.2f,18,panel,amber);

        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("Building shape refined: crystal roof, entrance canopy, 3 guiding fins. Old roof beams hidden, nothing deleted.");
    }

    // A tall panel leaning slightly, with a light line on top and a metal foot.
    static void Fin(Transform root,string name,Vector3 pos,float length,float height,float yaw,Material face,Material glow)
    {
        var g=new GameObject(name).transform;g.SetParent(root,false);g.localPosition=pos;g.localRotation=Quaternion.Euler(0,yaw,0);
        Box("Panel",g,new Vector3(0,height/2,0),new Vector3(length,height,0.22f),Quaternion.Euler(0,0,4),face,true);
        Box("TopLight",g,new Vector3(-height/2*Mathf.Sin(4*Mathf.Deg2Rad),height+0.03f,0),new Vector3(length,0.05f,0.26f),Quaternion.Euler(0,0,4),glow,false);
        Box("Foot",g,new Vector3(0,0.06f,0),new Vector3(length+0.2f,0.12f,0.4f),Quaternion.identity,M("Frame_Obsidian"),false);
    }
}
