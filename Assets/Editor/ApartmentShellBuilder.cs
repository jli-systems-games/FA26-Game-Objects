using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentShellBuilder
{
    const float S=.01f, H=2.7f, FloorDepth=.08f, SillHeight=.9f, GlassTop=2.2f, GlassDepth=.04f, FrameThickness=.05f, Tol=.001f;
    readonly struct R { public readonly string id; public readonly float minX,maxX,minY,maxY; public R(string id,float minX,float maxX,float minY,float maxY){this.id=id;this.minX=minX;this.maxX=maxX;this.minY=minY;this.maxY=maxY;} }

    static readonly Vector2[] FloorPoints={new(0,0),new(823,0),new(823,507),new(552,507),new(552,565),new(312,565),new(312,502),new(255,502),new(255,354),new(160,354),new(160,413),new(106,413),new(106,364),new(0,364)};
    static readonly R[] Exterior={
        new("TOP_LEFT_SOLID",0,62,0,32),new("LEFT_MAIN",0,19,0,364),new("TOP_PIER_1",190,258,0,63),new("TOP_PIER_2",386,454,0,32),new("TOP_PIER_3",558,650,0,64),new("TOP_RIGHT_CORNER",791,823,0,32),
        new("RIGHT_SOLID_1",791,823,104,173),new("RIGHT_SOLID_2",791,823,301,369),new("LEFT_LOWER_WALL",0,116,354,364),new("ENTRY_NOTCH_VERTICAL",106,116,354,413),new("ENTRY_NOTCH_BOTTOM",106,160,402,413),
        new("SERVICE_WEST",245,255,330,502),new("SERVICE_SHOULDER",245,331,482,502),new("SERVICE_STEP_VERTICAL",312,331,482,565),new("SERVICE_BOTTOM_MAIN",312,478,545,565),new("SERVICE_BOTTOM_RETURN",478,542,555,565),new("ALCOVE_LEFT_RETURN",542,552,479,565),new("ALCOVE_BOTTOM",552,823,497,507)};
    static readonly R[] Interior={
        new("UPPER_LEFT_TOP",245,327,253,263),new("UPPER_LEFT_RIGHT",317,327,253,354),new("BATH_TOP_LEFT",245,316,344,354),new("BATH_TOP_RIGHT_STUB",327,334,344,354),new("WD_TOP",462,559,252,263),
        new("WD_RIGHT",539,559,252,359),new("CENTER_BRIDGE",412,461,344,354),new("CENTER_CORE_VERTICAL",468,478,344,565),new("CENTER_TOP_RIGHT",468,539,344,359),new("ALCOVE_ENTRY_STUB",542,552,359,376)};
    static readonly R[] Windows={
        new("TOP_WINDOW_1",62,190,0,32),new("TOP_WINDOW_2",258,386,0,32),new("TOP_WINDOW_3",454,558,0,32),new("TOP_WINDOW_4",650,791,0,32),new("RIGHT_WINDOW_1",791,823,32,104),new("RIGHT_WINDOW_2",791,823,173,301),new("RIGHT_WINDOW_3",791,823,369,497)};

    [MenuItem("Tools/Apartment/Rebuild Snap-to-Grid V2 Shell")]
    public static void Build(){
        var old=GameObject.Find("Apartment"); if(old) Object.DestroyImmediate(old);
        var root=new GameObject("Apartment"); var settings=root.AddComponent<ApartmentShellSettings>(); settings.unityUnitsPerPlanPixel=S; settings.wallHeight=H;
        var floor=Child(root,"Floor"); var ext=Child(root,"ExteriorWalls"); var interior=Child(root,"InteriorWalls"); var windows=Child(root,"Windows"); var debug=Child(root,"DebugMarkers");
        var fm=Mat("Apartment_Floor",new Color(.72f,.69f,.62f,1),false); var em=Mat("Apartment_ExteriorWall",new Color(.82f,.82f,.8f,1),false); var im=Mat("Apartment_InteriorWall",new Color(.92f,.91f,.88f,1),false); var gm=Mat("Apartment_Glass",new Color(.25f,.65f,.85f,.28f),true); var dm=Mat("Apartment_ValidationError",Color.red,false);
        CreateFloor(floor.transform,fm); foreach(var r in Exterior) CreateWallFromBounds(r,ext.transform,em); foreach(var r in Interior) CreateWallFromBounds(r,interior.transform,im); foreach(var r in Windows) CreateWindowAssembly(r,windows.transform,em,gm);
        int errors=Validate(root.transform,debug.transform,dm); Static(root); Selection.activeGameObject=root; EditorSceneManager.MarkSceneDirty(root.scene); SceneView.lastActiveSceneView?.FrameSelected();
        if(errors==0) Debug.Log("[Apartment Validation] PASS — Snap-to-Grid V2 footprints and required joints are exact."); else Debug.LogError($"[Apartment Validation] FAILED with {errors} error(s). See DebugMarkers.");
    }

    [MenuItem("Tools/Apartment/Update Windows Only")]
    public static void UpdateWindowsOnly(){
        var root=GameObject.Find("Apartment");if(!root){Debug.LogError("[Apartment Windows] Apartment root not found; no geometry was changed.");return;}
        var windows=root.transform.Find("Windows");if(!windows){Debug.LogError("[Apartment Windows] Windows parent not found; no geometry was changed.");return;}
        for(int i=windows.childCount-1;i>=0;i--)Object.DestroyImmediate(windows.GetChild(i).gameObject);
        var wall=AssetDatabase.LoadAssetAtPath<Material>("Assets/ApartmentShell/Materials/Apartment_ExteriorWall.mat")??Mat("Apartment_ExteriorWall",new Color(.82f,.82f,.8f,1),false);
        var glass=AssetDatabase.LoadAssetAtPath<Material>("Assets/ApartmentShell/Materials/Apartment_Glass.mat")??Mat("Apartment_Glass",new Color(.25f,.65f,.85f,.28f),true);
        foreach(var r in Windows)CreateWindowAssembly(r,windows,wall,glass);
        Static(windows.gameObject);int errors=ValidateWindows(windows);EditorSceneManager.MarkSceneDirty(root.scene);Selection.activeTransform=windows;SceneView.lastActiveSceneView?.FrameSelected();
        if(errors==0)Debug.Log("[Apartment Windows] PASS — 7 complete assemblies; sill, glass, header, and frame footprints are valid.");else Debug.LogError($"[Apartment Windows] FAILED with {errors} error(s).");
    }

    static GameObject Child(GameObject p,string n){var g=new GameObject(n);g.transform.SetParent(p.transform,false);return g;}
    static void CreateWallFromBounds(R r,Transform p,Material m){
        float cx=(r.minX+r.maxX)/2, cy=(r.minY+r.maxY)/2, sx=r.maxX-r.minX, sy=r.maxY-r.minY;
        var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=r.id;g.transform.SetParent(p,false);g.transform.localPosition=new Vector3(cx*S,H/2,-cy*S);g.transform.localScale=new Vector3(sx*S,H,sy*S);g.GetComponent<Renderer>().sharedMaterial=m;
    }
    static void CreateWindowAssembly(R r,Transform p,Material wall,Material glass){
        float cx=(r.minX+r.maxX)/2*S,cz=-(r.minY+r.maxY)/2*S,sx=(r.maxX-r.minX)*S,sz=(r.maxY-r.minY)*S;bool top=sx>=sz;
        var a=Child(p.gameObject,r.id);CreateBox("LowerWall",a.transform,new Vector3(cx,SillHeight/2,cz),new Vector3(sx,SillHeight,sz),wall,true);CreateBox("UpperWall",a.transform,new Vector3(cx,(GlassTop+H)/2,cz),new Vector3(sx,H-GlassTop,sz),wall,true);
        float width=top?sx:sz;float inner=Mathf.Max(.01f,width-FrameThickness*2);Vector3 glassSize=top?new Vector3(inner,GlassTop-SillHeight-FrameThickness*2,GlassDepth):new Vector3(GlassDepth,GlassTop-SillHeight-FrameThickness*2,inner);
        CreateBox("Glass",a.transform,new Vector3(cx,(SillHeight+GlassTop)/2,cz),glassSize,glass,false);
        var f=Child(a,"Frame");float mid=(SillHeight+GlassTop)/2,fh=GlassTop-SillHeight;
        if(top){CreateBox("Left",f.transform,new Vector3(cx-width/2+FrameThickness/2,mid,cz),new Vector3(FrameThickness,fh,sz),wall,false);CreateBox("Right",f.transform,new Vector3(cx+width/2-FrameThickness/2,mid,cz),new Vector3(FrameThickness,fh,sz),wall,false);CreateBox("Bottom",f.transform,new Vector3(cx,SillHeight+FrameThickness/2,cz),new Vector3(inner,FrameThickness,sz),wall,false);CreateBox("Top",f.transform,new Vector3(cx,GlassTop-FrameThickness/2,cz),new Vector3(inner,FrameThickness,sz),wall,false);}
        else{CreateBox("Left",f.transform,new Vector3(cx,mid,cz+width/2-FrameThickness/2),new Vector3(sx,fh,FrameThickness),wall,false);CreateBox("Right",f.transform,new Vector3(cx,mid,cz-width/2+FrameThickness/2),new Vector3(sx,fh,FrameThickness),wall,false);CreateBox("Bottom",f.transform,new Vector3(cx,SillHeight+FrameThickness/2,cz),new Vector3(sx,FrameThickness,inner),wall,false);CreateBox("Top",f.transform,new Vector3(cx,GlassTop-FrameThickness/2,cz),new Vector3(sx,FrameThickness,inner),wall,false);}
    }
    static GameObject CreateBox(string n,Transform p,Vector3 pos,Vector3 scale,Material m,bool collider){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;}
    static int ValidateWindows(Transform p){int e=0;foreach(var r in Windows){var a=p.Find(r.id);if(!a){Debug.LogError("[Apartment Windows] Missing "+r.id);e++;continue;}string[] required={"LowerWall","Glass","UpperWall","Frame"};foreach(string n in required)if(!a.Find(n)){Debug.LogError($"[Apartment Windows] {r.id} missing {n}");e++;}foreach(var renderer in a.GetComponentsInChildren<Renderer>()){var b=renderer.bounds;if(b.min.x<r.minX*S-Tol||b.max.x>r.maxX*S+Tol||b.min.z<-r.maxY*S-Tol||b.max.z>-r.minY*S+Tol){Debug.LogError($"[Apartment Windows] FOOTPRINT ERROR: {r.id}/{renderer.name}");e++;}}}return e;}
    static void CreateFloor(Transform p,Material m){
        var pts=new List<Vector2>(FloorPoints);var tri=Triangulate(pts);int n=pts.Count;var v=new Vector3[n*2];for(int i=0;i<n;i++){v[i]=new Vector3(pts[i].x*S,0,-pts[i].y*S);v[i+n]=new Vector3(pts[i].x*S,-FloorDepth,-pts[i].y*S);}
        var ix=new List<int>();for(int i=0;i<tri.Count;i+=3){ix.Add(tri[i]);ix.Add(tri[i+1]);ix.Add(tri[i+2]);ix.Add(tri[i+2]+n);ix.Add(tri[i+1]+n);ix.Add(tri[i]+n);}for(int i=0;i<n;i++){int j=(i+1)%n;ix.Add(i);ix.Add(i+n);ix.Add(j+n);ix.Add(i);ix.Add(j+n);ix.Add(j);}
        var mesh=new Mesh{name="ApartmentFloorMesh_V2",vertices=v,triangles=ix.ToArray()};mesh.RecalculateNormals();mesh.RecalculateBounds();var g=new GameObject("FloorPolygon");g.transform.SetParent(p,false);g.AddComponent<MeshFilter>().sharedMesh=mesh;g.AddComponent<MeshRenderer>().sharedMaterial=m;g.AddComponent<MeshCollider>().sharedMesh=mesh;
    }

    static int Validate(Transform root,Transform debug,Material marker){
        int e=0;foreach(var r in Exterior)e+=CheckFootprint(r,root.Find("ExteriorWalls/"+r.id),debug,marker);foreach(var r in Interior)e+=CheckFootprint(r,root.Find("InteriorWalls/"+r.id),debug,marker);
        var top=new[]{Get(Exterior,"TOP_LEFT_SOLID"),Get(Windows,"TOP_WINDOW_1"),Get(Exterior,"TOP_PIER_1"),Get(Windows,"TOP_WINDOW_2"),Get(Exterior,"TOP_PIER_2"),Get(Windows,"TOP_WINDOW_3"),Get(Exterior,"TOP_PIER_3"),Get(Windows,"TOP_WINDOW_4"),Get(Exterior,"TOP_RIGHT_CORNER")};e+=CheckXChain("TOP FACADE",top,debug,marker);
        var right=new[]{Get(Exterior,"TOP_RIGHT_CORNER"),Get(Windows,"RIGHT_WINDOW_1"),Get(Exterior,"RIGHT_SOLID_1"),Get(Windows,"RIGHT_WINDOW_2"),Get(Exterior,"RIGHT_SOLID_2"),Get(Windows,"RIGHT_WINDOW_3"),Get(Exterior,"ALCOVE_BOTTOM")};e+=CheckYChain("RIGHT FACADE",right,debug,marker);
        e+=SharedX("SERVICE_BOTTOM_MAIN","SERVICE_BOTTOM_RETURN",478,debug,marker);e+=SharedX("SERVICE_BOTTOM_RETURN","ALCOVE_LEFT_RETURN",542,debug,marker);e+=SharedX("ALCOVE_LEFT_RETURN","ALCOVE_BOTTOM",552,debug,marker);
        float[] service={245,255,312,331,468,478,542,552};foreach(float x in service)if(!HasBoundary(x))e+=Error("MISSING REQUIRED SERVICE BOUNDARY x="+x,Point(x,550),debug,marker);return e;
    }
    static int CheckFootprint(R r,Transform t,Transform d,Material m){if(!t)return Error("MISSING WALL "+r.id,Point(r.minX,r.minY),d,m);var p=new Vector3((r.minX+r.maxX)/2*S,H/2,-(r.minY+r.maxY)/2*S);var s=new Vector3((r.maxX-r.minX)*S,H,(r.maxY-r.minY)*S);return (t.localPosition-p).sqrMagnitude>1e-10f||(t.localScale-s).sqrMagnitude>1e-10f?Error("FOOTPRINT ERROR for "+r.id,p,d,m):0;}
    static int CheckXChain(string label,R[] c,Transform d,Material m){int e=0;for(int i=0;i<c.Length-1;i++)if(Mathf.Abs(c[i].maxX*S-c[i+1].minX*S)>Tol)e+=Error($"GAP ERROR in {label} between {c[i].id} and {c[i+1].id}",Point(c[i].maxX,0),d,m);return e;}
    static int CheckYChain(string label,R[] c,Transform d,Material m){int e=0;for(int i=0;i<c.Length-1;i++)if(Mathf.Abs(c[i].maxY*S-c[i+1].minY*S)>Tol)e+=Error($"GAP ERROR in {label} between {c[i].id} and {c[i+1].id}",Point(823,c[i].maxY),d,m);return e;}
    static int SharedX(string a,string b,float x,Transform d,Material m){var l=Get(Exterior,a);var r=Get(Exterior,b);return Mathf.Abs(l.maxX*S-r.minX*S)>Tol||l.maxX*S!=x*S||r.minX*S!=x*S?Error($"GAP ERROR between {a} and {b}",Point(x,Mathf.Max(l.minY,r.minY)),d,m):0;}
    static R Get(R[] a,string id){foreach(var r in a)if(r.id==id)return r;throw new System.InvalidOperationException("Missing rectangle "+id);}
    static bool HasBoundary(float x){foreach(var r in Exterior)if(r.minX==x||r.maxX==x)return true;foreach(var r in Interior)if(r.minX==x||r.maxX==x)return true;return false;}
    static int Error(string s,Vector3 p,Transform d,Material m){Debug.LogError("[Apartment Validation] "+s);var g=GameObject.CreatePrimitive(PrimitiveType.Sphere);g.name="ERROR_"+s.Replace(' ','_');g.transform.SetParent(d,false);g.transform.localPosition=p;g.transform.localScale=Vector3.one*.12f;g.GetComponent<Renderer>().sharedMaterial=m;Object.DestroyImmediate(g.GetComponent<Collider>());return 1;}
    static Vector3 Point(float x,float y)=>new(x*S,.1f,-y*S);

    static List<int> Triangulate(List<Vector2> p){var r=new List<int>();var v=new List<int>();for(int i=0;i<p.Count;i++)v.Add(i);int guard=p.Count*p.Count;while(v.Count>2&&guard-->0){bool clipped=false;for(int i=0;i<v.Count;i++){int a=v[(i+v.Count-1)%v.Count],b=v[i],c=v[(i+1)%v.Count];if(Cross(p[a],p[b],p[c])<=0)continue;bool inside=false;for(int k=0;k<v.Count;k++){int q=v[k];if(q==a||q==b||q==c)continue;if(Inside(p[q],p[a],p[b],p[c])){inside=true;break;}}if(inside)continue;r.Add(a);r.Add(b);r.Add(c);v.RemoveAt(i);clipped=true;break;}if(!clipped)break;}return r;}
    static float Cross(Vector2 a,Vector2 b,Vector2 c)=>(b.x-a.x)*(c.y-a.y)-(b.y-a.y)*(c.x-a.x);
    static bool Inside(Vector2 p,Vector2 a,Vector2 b,Vector2 c)=>Cross(a,b,p)>=0&&Cross(b,c,p)>=0&&Cross(c,a,p)>=0;
    static Material Mat(string n,Color c,bool transparent){const string f="Assets/ApartmentShell/Materials";if(!AssetDatabase.IsValidFolder("Assets/ApartmentShell"))AssetDatabase.CreateFolder("Assets","ApartmentShell");if(!AssetDatabase.IsValidFolder(f))AssetDatabase.CreateFolder("Assets/ApartmentShell","Materials");string path=$"{f}/{n}.mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard")){name=n};AssetDatabase.CreateAsset(m,path);}m.color=c;if(transparent){m.SetFloat("_Surface",1);m.SetFloat("_Blend",0);m.SetFloat("_SrcBlend",(float)UnityEngine.Rendering.BlendMode.SrcAlpha);m.SetFloat("_DstBlend",(float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);m.SetFloat("_ZWrite",0);m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");m.renderQueue=3000;}EditorUtility.SetDirty(m);return m;}
    static void Static(GameObject g){g.isStatic=true;foreach(Transform c in g.transform)Static(c.gameObject);}
}
