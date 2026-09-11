using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentFurnitureBuilder
{
    [MenuItem("Tools/Apartment/Build Bedroom and Study Furniture")]
    public static void Build()
    {
        var apartment=GameObject.Find("Apartment");
        var topWindow=GameObject.Find("Apartment/Windows/TOP_WINDOW_4/LowerWall");
        var rightWall=GameObject.Find("Apartment/ExteriorWalls/RIGHT_SOLID_2");
        var bottomWall=GameObject.Find("Apartment/ExteriorWalls/ALCOVE_BOTTOM");
        var alcoveReturn=GameObject.Find("Apartment/ExteriorWalls/ALCOVE_LEFT_RETURN");
        var upperDoor=GameObject.Find("Apartment/Doors/RIGHT_CLOSET_DOOR_UPPER/DoorLeaf");
        var lowerDoor=GameObject.Find("Apartment/Doors/RIGHT_CLOSET_DOOR_LOWER/DoorLeaf");
        if(!apartment||!topWindow||!rightWall||!bottomWall||!alcoveReturn||!upperDoor||!lowerDoor){Debug.LogError("[Furniture] Current room or door references are missing; nothing changed.");return;}

        var old=apartment.transform.Find("Furniture");if(old)Object.DestroyImmediate(old.gameObject);
        var root=Group("Furniture",apartment.transform);
        var wood=Mat("Furniture_Wood",new Color(.38f,.24f,.15f,1));var lightWood=Mat("Furniture_LightWood",new Color(.62f,.47f,.31f,1));
        var fabric=Mat("Furniture_Fabric",new Color(.73f,.75f,.72f,1));var blanket=Mat("Furniture_Blanket",new Color(.26f,.42f,.52f,1));
        var dark=Mat("Furniture_Dark",new Color(.12f,.14f,.15f,1));var metal=Mat("Furniture_Metal",new Color(.31f,.34f,.36f,1));var lamp=Mat("Furniture_LampShade",new Color(.82f,.75f,.59f,1));

        Bounds tw=topWindow.GetComponent<Renderer>().bounds,rw=rightWall.GetComponent<Renderer>().bounds,bw=bottomWall.GetComponent<Renderer>().bounds;
        float deskWidth=Mathf.Min(1.45f,tw.size.x-.18f),deskX=tw.center.x,deskZ=tw.min.z-.34f;
        BuildDesk(root.transform,new Vector3(deskX,0,deskZ),deskWidth,.60f,wood,metal,dark);
        BuildChair(root.transform,new Vector3(deskX,0,deskZ-.64f),lightWood,fabric,metal);

        float rightFace=rw.min.x,bottomFace=bw.max.z;
        float doorExtent=Mathf.Max(upperDoor.GetComponent<Renderer>().bounds.max.x,lowerDoor.GetComponent<Renderer>().bounds.max.x);
        float bedMinX=Mathf.Max(doorExtent+.08f,alcoveReturn.GetComponent<Renderer>().bounds.max.x+.15f);
        float bedWidth=Mathf.Min(1.35f,rightFace-bedMinX-.10f),bedLength=1.95f;
        float bedX=bedMinX+bedWidth*.5f,bedZ=bottomFace+.15f+bedLength*.5f;
        BuildBed(root.transform,new Vector3(bedX,0,bedZ),bedWidth,bedLength,wood,fabric,blanket);

        float nightWidth=.36f,nightX=Mathf.Min(rightFace-.07f-nightWidth*.5f,bedX+bedWidth*.5f+.08f+nightWidth*.5f);
        BuildNightstand(root.transform,new Vector3(nightX,0,bottomFace+.47f),nightWidth,.34f,.52f,lightWood,metal);
        float lampX=bedX-bedWidth*.5f-.08f-.14f,lampZ=bedZ+bedLength*.30f;
        BuildLamp(root.transform,new Vector3(lampX,0,lampZ),metal,lamp);

        MarkStatic(root);int errors=Validate(root.transform,apartment.transform);
        EditorSceneManager.MarkSceneDirty(apartment.scene);Selection.activeGameObject=root;SceneView.lastActiveSceneView?.FrameSelected();
        foreach(string n in new[]{"Desk","Chair","Bed","FloorLamp","Nightstand"}){Bounds b=Combined(root.transform.Find(n));Debug.Log($"[Furniture] {n} bounds center={b.center:F3} size={b.size:F3} min={b.min:F3} max={b.max:F3}");}
        if(errors==0)Debug.Log("[Furniture] PASS — study and sleeping furniture fit current walls and open-door clearances.");else Debug.LogError($"[Furniture] FAILED with {errors} validation error(s).");
    }

    static void BuildDesk(Transform p,Vector3 c,float width,float depth,Material wood,Material metal,Material dark)
    {
        var g=Group("Desk",p);Box("Top",g.transform,new Vector3(c.x,.75f,c.z),new Vector3(width,.07f,depth),wood);
        float x=width*.43f,z=depth*.39f;Box("Leg_01",g.transform,new Vector3(c.x-x,.37f,c.z-z),new Vector3(.06f,.74f,.06f),metal);Box("Leg_02",g.transform,new Vector3(c.x+x,.37f,c.z-z),new Vector3(.06f,.74f,.06f),metal);Box("Leg_03",g.transform,new Vector3(c.x-x,.37f,c.z+z),new Vector3(.06f,.74f,.06f),metal);Box("Leg_04",g.transform,new Vector3(c.x+x,.37f,c.z+z),new Vector3(.06f,.74f,.06f),metal);
        Box("DrawerBlock",g.transform,new Vector3(c.x+width*.34f,.62f,c.z),new Vector3(width*.24f,.18f,depth*.82f),wood);
        Box("BackPanel",g.transform,new Vector3(c.x,.57f,c.z+depth*.45f),new Vector3(width,.28f,.035f),light(wood));
        Box("Monitor",g.transform,new Vector3(c.x,.98f,c.z+.04f),new Vector3(.52f,.32f,.035f),dark);Box("MonitorStand",g.transform,new Vector3(c.x,.81f,c.z+.04f),new Vector3(.08f,.15f,.08f),metal);
    }
    static void BuildChair(Transform p,Vector3 c,Material wood,Material fabric,Material metal)
    {
        var g=Group("Chair",p);Box("Seat",g.transform,new Vector3(c.x,.46f,c.z),new Vector3(.46f,.09f,.44f),fabric);
        Box("Back",g.transform,new Vector3(c.x,.75f,c.z-.205f),new Vector3(.46f,.55f,.07f),fabric);
        float x=.18f,z=.17f;Box("Leg_01",g.transform,new Vector3(c.x-x,.22f,c.z-z),new Vector3(.045f,.44f,.045f),metal);Box("Leg_02",g.transform,new Vector3(c.x+x,.22f,c.z-z),new Vector3(.045f,.44f,.045f),metal);Box("Leg_03",g.transform,new Vector3(c.x-x,.22f,c.z+z),new Vector3(.045f,.44f,.045f),metal);Box("Leg_04",g.transform,new Vector3(c.x+x,.22f,c.z+z),new Vector3(.045f,.44f,.045f),metal);
        Box("SupportBar",g.transform,new Vector3(c.x,.23f,c.z),new Vector3(.36f,.035f,.035f),wood);
    }
    static void BuildBed(Transform p,Vector3 c,float width,float length,Material wood,Material fabric,Material cover)
    {
        var g=Group("Bed",p);Box("Frame",g.transform,new Vector3(c.x,.22f,c.z),new Vector3(width,.20f,length),wood);
        Box("Mattress",g.transform,new Vector3(c.x,.39f,c.z),new Vector3(width-.08f,.22f,length-.10f),fabric);
        Box("Blanket",g.transform,new Vector3(c.x,.515f,c.z-length*.12f),new Vector3(width-.12f,.035f,length*.62f),cover);
        Box("Pillow_01",g.transform,new Vector3(c.x-width*.24f,.525f,c.z+length*.36f),new Vector3(width*.38f,.13f,.34f),fabric);
        Box("Pillow_02",g.transform,new Vector3(c.x+width*.24f,.525f,c.z+length*.36f),new Vector3(width*.38f,.13f,.34f),fabric);
        Box("Headboard",g.transform,new Vector3(c.x,.55f,c.z+length*.5f-.035f),new Vector3(width+.04f,.82f,.07f),wood);
        float x=width*.43f,z=length*.43f;Box("Leg_01",g.transform,new Vector3(c.x-x,.09f,c.z-z),new Vector3(.07f,.18f,.07f),wood);Box("Leg_02",g.transform,new Vector3(c.x+x,.09f,c.z-z),new Vector3(.07f,.18f,.07f),wood);Box("Leg_03",g.transform,new Vector3(c.x-x,.09f,c.z+z),new Vector3(.07f,.18f,.07f),wood);Box("Leg_04",g.transform,new Vector3(c.x+x,.09f,c.z+z),new Vector3(.07f,.18f,.07f),wood);
    }
    static void BuildNightstand(Transform p,Vector3 c,float width,float depth,float height,Material wood,Material metal)
    {
        var g=Group("Nightstand",p);Box("Body",g.transform,new Vector3(c.x,height*.5f,c.z),new Vector3(width,height,depth),wood);
        Box("Drawer_01",g.transform,new Vector3(c.x,c.y+.37f,c.z-depth*.505f),new Vector3(width-.04f,.16f,.025f),light(wood));
        Box("Drawer_02",g.transform,new Vector3(c.x,c.y+.17f,c.z-depth*.505f),new Vector3(width-.04f,.16f,.025f),light(wood));
        Box("Handle_01",g.transform,new Vector3(c.x,.37f,c.z-depth*.53f),new Vector3(.12f,.025f,.025f),metal);Box("Handle_02",g.transform,new Vector3(c.x,.17f,c.z-depth*.53f),new Vector3(.12f,.025f,.025f),metal);
        Box("Top",g.transform,new Vector3(c.x,height+.025f,c.z),new Vector3(width+.04f,.05f,depth+.04f),wood);
    }
    static void BuildLamp(Transform p,Vector3 c,Material metal,Material shade)
    {
        var g=Group("FloorLamp",p);Cylinder("Base",g.transform,new Vector3(c.x,.035f,c.z),.14f,.07f,metal);
        Cylinder("Pole",g.transform,new Vector3(c.x,.82f,c.z),.018f,1.55f,metal);
        Cylinder("Shade",g.transform,new Vector3(c.x,1.62f,c.z),.17f,.24f,shade);
        Sphere("Bulb",g.transform,new Vector3(c.x,1.54f,c.z),new Vector3(.10f,.13f,.10f),shade);
    }
    static int Validate(Transform furniture,Transform apartment)
    {
        int e=0;string[] names={"Desk","Chair","Bed","FloorLamp","Nightstand"};foreach(string n in names)if(!furniture.Find(n)){Debug.LogError("[Furniture] Missing "+n);e++;}
        var walls=new System.Collections.Generic.List<Renderer>();foreach(string p in new[]{"ExteriorWalls","InteriorWalls"}){var t=apartment.Find(p);if(t)walls.AddRange(t.GetComponentsInChildren<Renderer>());}
        var doors=apartment.Find("Doors");var doorRenderers=doors?doors.GetComponentsInChildren<Renderer>():new Renderer[0];
        foreach(string n in names){var rs=furniture.Find(n).GetComponentsInChildren<Renderer>();foreach(var a in rs){foreach(var b in walls)if(Significant(a.bounds,b.bounds)){Debug.LogError($"[Furniture] {n}/{a.name} intersects wall {b.name}.");e++;}foreach(var b in doorRenderers)if(b.name=="DoorLeaf"&&Significant(a.bounds,b.bounds)){Debug.LogError($"[Furniture] {n}/{a.name} blocks open door under {b.transform.parent.name}.");e++;}}}
        return e;
    }
    static bool Significant(Bounds a,Bounds b){if(!a.Intersects(b))return false;Vector3 s=Vector3.Min(a.max,b.max)-Vector3.Max(a.min,b.min);return s.x>.005f&&s.y>.005f&&s.z>.005f;}
    static Bounds Combined(Transform t){var rs=t.GetComponentsInChildren<Renderer>();Bounds b=rs[0].bounds;for(int i=1;i<rs.Length;i++)b.Encapsulate(rs[i].bounds);return b;}
    static Material light(Material source){return source;}
    static GameObject Group(string n,Transform p){var g=new GameObject(n);g.transform.SetParent(p,false);return g;}
    static GameObject Box(string n,Transform p,Vector3 pos,Vector3 scale,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static GameObject Cylinder(string n,Transform p,Vector3 pos,float radius,float height,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=n;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=new Vector3(radius*2,height*.5f,radius*2);g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static GameObject Sphere(string n,Transform p,Vector3 pos,Vector3 scale,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Sphere);g.name=n;g.transform.SetParent(p,false);g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static Material Mat(string n,Color c){string p=$"Assets/ApartmentShell/Materials/{n}.mat";var m=AssetDatabase.LoadAssetAtPath<Material>(p);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard")){name=n};AssetDatabase.CreateAsset(m,p);}m.color=c;EditorUtility.SetDirty(m);return m;}
    static void MarkStatic(GameObject g){g.isStatic=true;foreach(Transform c in g.transform)MarkStatic(c.gameObject);}
}
