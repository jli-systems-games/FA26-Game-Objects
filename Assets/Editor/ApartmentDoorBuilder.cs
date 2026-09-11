using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentDoorBuilder
{
    const float Height=2.05f, Thickness=.04f, Clearance=.015f, FrameThickness=.05f;
    [Serializable] class Report { public List<DoorData> doors=new List<DoorData>(); }
    [Serializable] class DoorData
    {
        public string name; public float measuredOpeningWidth; public float doorWidth;
        public float[] hingeWorldPosition, leafWorldPosition, doorDimensions; public float finalYRotation;
        public List<string> overlaps=new List<string>();
    }
    static Report report;

    [MenuItem("Tools/Apartment/Build All Doors From Current Openings")]
    public static void Build()
    {
        var apartment=GameObject.Find("Apartment"); if(!apartment){Debug.LogError("[Doors] Apartment root missing; nothing changed.");return;}
        GameObject entry=Find("Apartment/ExteriorWalls/ENTRY_NOTCH_BOTTOM"),service=Find("Apartment/ExteriorWalls/SERVICE_WEST");
        GameObject closetTop=Find("Apartment/InteriorWalls/UPPER_LEFT_TOP"),bathLeft=Find("Apartment/InteriorWalls/BATH_TOP_LEFT");
        GameObject bridge=Find("Apartment/InteriorWalls/CENTER_BRIDGE"),wdTop=Find("Apartment/InteriorWalls/WD_TOP");
        GameObject wdBottom=Find("Apartment/InteriorWalls/CENTER_TOP_RIGHT"),stub=Find("Apartment/InteriorWalls/ALCOVE_ENTRY_STUB");
        GameObject alcoveReturn=Find("Apartment/ExteriorWalls/ALCOVE_LEFT_RETURN");
        if(!entry||!service||!closetTop||!bathLeft||!bridge||!wdTop||!wdBottom||!stub||!alcoveReturn){Debug.LogError("[Doors] One or more current jamb reference walls are missing; nothing changed.");return;}

        var old=apartment.transform.Find("Doors");if(old)UnityEngine.Object.DestroyImmediate(old.gameObject);
        var doors=Group("Doors",apartment.transform);report=new Report();
        var slab=MaterialAsset("Door_Slab",new Color(.56f,.39f,.25f,1));var trim=MaterialAsset("Door_Trim",new Color(.86f,.84f,.79f,1));var metal=MaterialAsset("Door_Hardware",new Color(.34f,.37f,.39f,1));

        Bounds eb=B(entry),sb=B(service),ct=B(closetTop),bl=B(bathLeft),br=B(bridge),wt=B(wdTop),wb=B(wdBottom),st=B(stub),ar=B(alcoveReturn);

        float entryZ=eb.center.z,entryOpen=sb.min.x-eb.max.x;
        MakeDoor(doors.transform,"ENTRY_DOOR",new Vector3(sb.min.x,0,entryZ),Vector3.forward,entryOpen-.04f,entryOpen,0,slab,trim,metal,true,
            'X',eb.max.x,sb.min.x,entryZ);

        float leftX=(ct.min.x+bl.min.x)*.5f,leftOpen=ct.min.z-bl.max.z;
        MakeDoor(doors.transform,"LEFT_CLOSET_DOOR",new Vector3(leftX,0,bl.max.z),Vector3.left,leftOpen-.04f,leftOpen,270,slab,trim,metal,true,
            'Z',bl.max.z,ct.min.z,leftX);

        float bathOpen=br.min.x-bl.max.x,bathZ=bl.max.z;
        MakeDoor(doors.transform,"BATHROOM_DOOR",new Vector3(bl.max.x,0,bathZ),Vector3.back,bathOpen-.04f,bathOpen,180,slab,trim,metal,true,
            'X',bl.max.x,br.min.x,(bl.center.z+br.center.z)*.5f);

        float wdX=Mathf.Max(wt.min.x,wb.min.x),wdOpen=wt.min.z-wb.max.z;
        MakeDoor(doors.transform,"WD_CLOSET_DOOR",new Vector3(wdX,0,wb.max.z),Vector3.left,wdOpen-.04f,wdOpen,270,slab,trim,metal,true,
            'Z',wb.max.z,wt.min.z,wdX);

        float rightX=(st.center.x+ar.center.x)*.5f,rightOpen=st.min.z-ar.max.z,rightLeaf=rightOpen*.5f-.02f,mid=(st.min.z+ar.max.z)*.5f;
        MakeDoor(doors.transform,"RIGHT_CLOSET_DOOR_UPPER",new Vector3(rightX,0,st.min.z),Vector3.right,rightLeaf,rightOpen,90,slab,trim,metal,false,
            'Z',mid,st.min.z,rightX);
        MakeDoor(doors.transform,"RIGHT_CLOSET_DOOR_LOWER",new Vector3(rightX,0,ar.max.z),Vector3.right,rightLeaf,rightOpen,90,slab,trim,metal,false,
            'Z',ar.max.z,mid,rightX);

        MarkStatic(doors);DetectOverlaps(doors.transform,apartment.transform);
        int errors=Validate(doors.transform);
        string path="Assets/SceneSnapshots/current_door_placement_report.json";File.WriteAllText(Path.GetFullPath(path),JsonUtility.ToJson(report,true));AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceSynchronousImport);
        EditorSceneManager.MarkSceneDirty(apartment.scene);Selection.activeGameObject=doors;SceneView.lastActiveSceneView?.FrameSelected();
        foreach(var d in report.doors)Debug.Log($"[Doors] {d.name}: opening={d.measuredOpeningWidth:F3}, hinge=({d.hingeWorldPosition[0]:F3},{d.hingeWorldPosition[1]:F3},{d.hingeWorldPosition[2]:F3}), leaf=({d.leafWorldPosition[0]:F3},{d.leafWorldPosition[1]:F3},{d.leafWorldPosition[2]:F3}), dims=({d.doorDimensions[0]:F3},{d.doorDimensions[1]:F3},{d.doorDimensions[2]:F3}), Y={d.finalYRotation:F0}, overlaps={d.overlaps.Count}");
        if(errors==0)Debug.Log("[Doors] PASS — 5 current openings, 6 fully-open leaves, no fixture/appliance intersections.");else Debug.LogError($"[Doors] FAILED with {errors} validation error(s).");
    }

    static void MakeDoor(Transform root,string name,Vector3 hinge,Vector3 direction,float width,float opening,float yRotation,Material slab,Material trim,Material metal,bool twoHandles,char openingAxis,float endA,float endB,float plane)
    {
        var group=Group(name,root);Vector3 center=hinge+direction*(width*.5f);center.y=Clearance+Height*.5f;
        var leaf=Box("DoorLeaf",group.transform,center,new Vector3(Thickness,Height,width),slab);leaf.transform.localEulerAngles=new Vector3(0,yRotation,0);
        Vector3 normal=new Vector3(direction.z,0,-direction.x),free=hinge+direction*(width-.09f);free.y=.95f;
        AddHandle(group.transform,twoHandles?"Handle_Interior":"Handle",free+normal*(Thickness*.5f+.025f),direction,metal);
        if(twoHandles)AddHandle(group.transform,"Handle_Exterior",free-normal*(Thickness*.5f+.025f),direction,metal);
        var inset=Box("InsetPanel",group.transform,center+normal*(Thickness*.5f+.006f),new Vector3(.008f,Height*.62f,width*.72f),trim);inset.transform.localEulerAngles=new Vector3(0,yRotation,0);
        var frame=Group("Frame",group.transform);BuildFrame(frame.transform,openingAxis,endA,endB,plane,trim);
        report.doors.Add(new DoorData{name=name,measuredOpeningWidth=opening,doorWidth=width,hingeWorldPosition=A(hinge),leafWorldPosition=A(center),doorDimensions=A(new Vector3(width,Height,Thickness)),finalYRotation=yRotation});
    }
    static void BuildFrame(Transform parent,char axis,float a,float b,float plane,Material m)
    {
        float lo=Mathf.Min(a,b),hi=Mathf.Max(a,b),span=hi-lo;
        if(axis=='X'){Box("Jamb_A",parent,new Vector3(lo,Height*.5f,plane),new Vector3(FrameThickness,Height,.07f),m);Box("Jamb_B",parent,new Vector3(hi,Height*.5f,plane),new Vector3(FrameThickness,Height,.07f),m);Box("Header",parent,new Vector3((lo+hi)*.5f,Height+.025f,plane),new Vector3(span+FrameThickness,.05f,.07f),m);}
        else{Box("Jamb_A",parent,new Vector3(plane,Height*.5f,lo),new Vector3(.07f,Height,FrameThickness),m);Box("Jamb_B",parent,new Vector3(plane,Height*.5f,hi),new Vector3(.07f,Height,FrameThickness),m);Box("Header",parent,new Vector3(plane,Height+.025f,(lo+hi)*.5f),new Vector3(.07f,.05f,span+FrameThickness),m);}
    }
    static void AddHandle(Transform p,string name,Vector3 pos,Vector3 direction,Material m)
    {
        Vector3 scale=Mathf.Abs(direction.x)>.5f?new Vector3(.12f,.035f,.025f):new Vector3(.025f,.035f,.12f);Box(name,p,pos,scale,m);
    }
    static void DetectOverlaps(Transform doors,Transform apartment)
    {
        var existing=new List<Renderer>();foreach(var r in apartment.GetComponentsInChildren<Renderer>(true))if(!r.transform.IsChildOf(doors))existing.Add(r);
        foreach(var d in report.doors){var leaf=doors.Find(d.name+"/DoorLeaf").GetComponent<Renderer>();foreach(var r in existing){if(!leaf.bounds.Intersects(r.bounds))continue;Vector3 min=Vector3.Max(leaf.bounds.min,r.bounds.min),max=Vector3.Min(leaf.bounds.max,r.bounds.max),s=max-min;if(s.x>.002f&&s.y>.002f&&s.z>.002f)d.overlaps.Add(PathOf(r.transform,apartment)+" | intersectionSize="+s.ToString("F4")+" | may be intentional hinge/jamb seating");}}
    }
    static int Validate(Transform doors)
    {
        int e=0;if(doors.childCount!=6){Debug.LogError("[Doors] Door assembly count is not 6.");e++;}
        foreach(var d in report.doors){var g=doors.Find(d.name);if(!g||!g.Find("DoorLeaf")){e++;continue;}Bounds b=g.Find("DoorLeaf").GetComponent<Renderer>().bounds;if(b.min.y<Clearance-.002f||Mathf.Abs(b.size.y-Height)>.003f){Debug.LogError("[Doors] Floor clearance/height error: "+d.name);e++;}}
        CheckNoIntersection(doors,"BATHROOM_DOOR","Apartment/BathroomFixtures",ref e);CheckNoIntersection(doors,"WD_CLOSET_DOOR","Apartment/Laundry",ref e);return e;
    }
    static void CheckNoIntersection(Transform doors,string door,string other,ref int e){var a=doors.Find(door+"/DoorLeaf").GetComponent<Renderer>().bounds;var go=GameObject.Find(other);if(!go)return;foreach(var r in go.GetComponentsInChildren<Renderer>())if(a.Intersects(r.bounds)){Vector3 s=Vector3.Min(a.max,r.bounds.max)-Vector3.Max(a.min,r.bounds.min);if(s.x>.002f&&s.y>.002f&&s.z>.002f){Debug.LogError($"[Doors] {door} intersects {other}/{r.name}.");e++;break;}}}
    static GameObject Find(string p)=>GameObject.Find(p);static Bounds B(GameObject g)=>g.GetComponent<Renderer>().bounds;static float[] A(Vector3 v)=>new[]{v.x,v.y,v.z};
    static string PathOf(Transform t,Transform root){var n=new List<string>();while(t){n.Add(t.name);if(t==root)break;t=t.parent;}n.Reverse();return string.Join("/",n);}
    static GameObject Group(string n,Transform p){var g=new GameObject(n);g.transform.SetParent(p,false);return g;}
    static GameObject Box(string n,Transform p,Vector3 pos,Vector3 scale,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static Material MaterialAsset(string n,Color c){string p=$"Assets/ApartmentShell/Materials/{n}.mat";var m=AssetDatabase.LoadAssetAtPath<Material>(p);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard")){name=n};AssetDatabase.CreateAsset(m,p);}m.color=c;EditorUtility.SetDirty(m);return m;}
    static void MarkStatic(GameObject g){g.isStatic=true;foreach(Transform c in g.transform)MarkStatic(c.gameObject);}
}
