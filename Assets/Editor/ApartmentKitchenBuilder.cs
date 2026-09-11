using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentKitchenBuilder
{
    const float S=.01f, CabinetHeight=.85f, CounterHeight=.9f, CounterThickness=.05f;

    [MenuItem("Tools/Apartment/Update Kitchen Only")]
    public static void BuildKitchenOnly()
    {
        var apartment=GameObject.Find("Apartment");
        if(!apartment){Debug.LogError("[Apartment Kitchen] Apartment root not found; nothing changed.");return;}
        var old=apartment.transform.Find("Kitchen");if(old)Object.DestroyImmediate(old.gameObject);
        var kitchen=Group("Kitchen",apartment.transform);var bases=Group("BaseCabinets",kitchen.transform);var counters=Group("Countertops",kitchen.transform);var stove=Group("Stove",kitchen.transform);var sink=Group("Sink",kitchen.transform);var dishwasher=Group("Dishwasher",kitchen.transform);var uppers=Group("UpperCabinets",kitchen.transform);

        var cabinet=MaterialAsset("Kitchen_Cabinet",new Color(.72f,.70f,.66f,1));var counter=MaterialAsset("Kitchen_Countertop",new Color(.16f,.17f,.18f,1));var metal=MaterialAsset("Kitchen_Metal",new Color(.48f,.52f,.55f,1));var dark=MaterialAsset("Kitchen_Cooktop",new Color(.035f,.04f,.045f,1));var backsplash=MaterialAsset("Kitchen_Backsplash",new Color(.78f,.80f,.80f,1));

        BoxPlan("PrepCabinet_01",bases.transform,24,79,154,167,0,CabinetHeight,cabinet);
        BoxPlan("PrepCabinet_02",bases.transform,24,79,167,180,0,CabinetHeight,cabinet);
        BoxPlan("SinkCabinet",bases.transform,24,79,222,286,0,CabinetHeight,cabinet);
        BoxPlan("LowerCabinet_01",bases.transform,24,79,286,317,0,CabinetHeight,cabinet);
        BoxPlan("LowerCabinet_02",bases.transform,24,79,317,348,0,CabinetHeight,cabinet);

        BoxPlan("Countertop_TopEnd",counters.transform,24,79,92,96,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Prep",counters.transform,24,79,154,180,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Dishwasher",counters.transform,24,79,180,222,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Sink_Back",counters.transform,24,34,222,286,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Sink_Front",counters.transform,69,79,222,286,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Sink_Top",counters.transform,34,69,222,232,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Sink_Bottom",counters.transform,34,69,276,286,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Countertop_Lower",counters.transform,24,79,286,348,CounterHeight-CounterThickness,CounterHeight,counter);
        BoxPlan("Backsplash",counters.transform,24,25,92,348,CounterHeight,1.42f,backsplash);

        BoxPlan("StoveBody",stove.transform,24,79,96,154,0,CabinetHeight,cabinet);
        BoxPlan("Cooktop",stove.transform,24,79,96,154,CounterHeight-.025f,CounterHeight,dark);
        Burner("Burner_01",stove.transform,.39f,-1.10f,dark);Burner("Burner_02",stove.transform,.64f,-1.10f,dark);Burner("Burner_03",stove.transform,.39f,-1.40f,dark);Burner("Burner_04",stove.transform,.64f,-1.40f,dark);

        BoxPlan("DishwasherBody",dishwasher.transform,24,79,180,222,0,CabinetHeight,metal);
        BoxPlan("DishwasherHandle",dishwasher.transform,77,79,184,218,.70f,.73f,dark);

        BoxPlan("SinkBasin",sink.transform,34,69,232,276,.80f,.885f,metal);
        var faucet=Group("Faucet",sink.transform);Cylinder("Stem",faucet.transform,new Vector3(.31f,1.02f,-2.54f),.025f,.24f,metal);Box("Spout",faucet.transform,new Vector3(.405f,1.13f,-2.54f),new Vector3(.19f,.035f,.035f),metal);Box("Drop",faucet.transform,new Vector3(.49f,1.08f,-2.54f),new Vector3(.035f,.13f,.035f),metal);

        BuildAdaptiveUpperCabinets(kitchen.transform,cabinet,metal);

        MarkStatic(kitchen);int errors=Validate(kitchen.transform);EditorSceneManager.MarkSceneDirty(apartment.scene);Selection.activeGameObject=kitchen;SceneView.lastActiveSceneView?.FrameSelected();
        if(errors==0)Debug.Log("[Apartment Kitchen] PASS — kitchen fits x=19–82, y=88–354 and contains integrated counter, stove, sink, dishwasher, and cabinets.");else Debug.LogError($"[Apartment Kitchen] FAILED with {errors} error(s).");
    }

    [MenuItem("Tools/Apartment/Update Upper Cabinets From Current Kitchen")]
    public static void UpdateUpperCabinetsOnly()
    {
        var kitchen=GameObject.Find("Apartment/Kitchen");if(!kitchen){Debug.LogError("[Upper Cabinets] Kitchen not found; nothing changed.");return;}
        var cabinet=AssetDatabase.LoadAssetAtPath<Material>("Assets/ApartmentShell/Materials/Kitchen_Cabinet.mat")??MaterialAsset("Kitchen_Cabinet",new Color(.72f,.70f,.66f,1));
        var metal=AssetDatabase.LoadAssetAtPath<Material>("Assets/ApartmentShell/Materials/Kitchen_Metal.mat")??MaterialAsset("Kitchen_Metal",new Color(.48f,.52f,.55f,1));
        int errors=BuildAdaptiveUpperCabinets(kitchen.transform,cabinet,metal);MarkStatic(kitchen.transform.Find("UpperCabinets").gameObject);EditorSceneManager.MarkSceneDirty(kitchen.scene);Selection.activeTransform=kitchen.transform.Find("UpperCabinets");SceneView.lastActiveSceneView?.FrameSelected();
        if(errors==0)Debug.Log("[Upper Cabinets] PASS — adaptive run matches the current lower kitchen geometry.");else Debug.LogError($"[Upper Cabinets] FAILED with {errors} error(s).");
    }

    static int BuildAdaptiveUpperCabinets(Transform kitchen,Material cabinet,Material metal)
    {
        var counters=kitchen.Find("Countertops");var cooktop=kitchen.Find("Stove/Cooktop");var basin=kitchen.Find("Sink/SinkBasin");
        if(!counters||!cooktop||!basin){Debug.LogError("[Upper Cabinets] Current countertops, cooktop, or sink basin are missing.");return 1;}
        Bounds lower=RendererBounds(counters);lower.Encapsulate(cooktop.GetComponent<Renderer>().bounds);lower.Encapsulate(basin.GetComponent<Renderer>().bounds);
        float lowerDepth=lower.size.x,upperDepth=lowerDepth*.5f,backX=lower.min.x,frontX=backX+upperDepth,zMin=lower.min.z,zMax=lower.max.z,run=zMax-zMin;
        int modules=run<2.2f?3:run>3.2f?5:4;var parent=kitchen.Find("UpperCabinets");if(!parent)parent=Group("UpperCabinets",kitchen).transform;for(int i=parent.childCount-1;i>=0;i--)Object.DestroyImmediate(parent.GetChild(i).gameObject);
        float moduleLength=run/modules;
        for(int i=0;i<modules;i++)
        {
            float a=zMax-i*moduleLength,b=zMax-(i+1)*moduleLength,zc=(a+b)*.5f;var module=Group($"Module_{i+1:00}",parent);
            Box("Body",module.transform,new Vector3(backX+upperDepth*.5f,1.815f,zc),new Vector3(upperDepth,.73f,moduleLength),cabinet);
            int doors=moduleLength>.5f?2:1;float doorLength=(moduleLength-.03f)/doors;
            for(int d=0;d<doors;d++)
            {
                float dz=b+.015f+doorLength*(d+.5f);Box($"Door_{d+1:00}",module.transform,new Vector3(frontX-.008f,1.815f,dz),new Vector3(.016f,.67f,doorLength-.012f),cabinet);
                Box($"Handle_{d+1:00}",module.transform,new Vector3(frontX+.012f,1.80f,dz),new Vector3(.025f,.22f,.018f),metal);
            }
        }
        int errors=0;Bounds upper=RendererBounds(parent,"Body");float ratio=upper.size.x/lowerDepth;if(Mathf.Abs(upper.min.z-zMin)>.002f||Mathf.Abs(upper.max.z-zMax)>.002f){Debug.LogError("[Upper Cabinets] Run does not align with measured lower work area.");errors++;}if(ratio<.45f||ratio>.55f){Debug.LogError("[Upper Cabinets] Depth ratio is outside 45–55%.");errors++;}if(parent.childCount!=modules){Debug.LogError("[Upper Cabinets] Module count mismatch.");errors++;}
        Debug.Log($"[Upper Cabinets] Measured lower run={run:F3}, depth={lowerDepth:F3}; built {modules} modules, upper depth={upperDepth:F3} ({ratio:P0}).");return errors;
    }

    static Bounds RendererBounds(Transform root,string requiredName=null)
    {
        Bounds b=new Bounds();bool found=false;foreach(var r in root.GetComponentsInChildren<Renderer>()){if(requiredName!=null&&r.name!=requiredName)continue;if(!found){b=r.bounds;found=true;}else b.Encapsulate(r.bounds);}if(!found)throw new System.InvalidOperationException("No matching renderers under "+root.name);return b;
    }

    static int Validate(Transform kitchen)
    {
        int e=0;string[] required={"BaseCabinets","Countertops","Stove/StoveBody","Stove/Cooktop","Stove/Burner_01","Stove/Burner_02","Stove/Burner_03","Stove/Burner_04","Sink/SinkBasin","Sink/Faucet","Dishwasher/DishwasherBody","UpperCabinets"};
        foreach(string path in required)if(!kitchen.Find(path)){Debug.LogError("[Apartment Kitchen] Missing "+path);e++;}
        foreach(var r in kitchen.GetComponentsInChildren<Renderer>()){var b=r.bounds;if(b.min.x<.19f-.001f||b.max.x>.82f+.001f||b.min.z<-3.54f-.001f||b.max.z>-.88f+.001f||b.min.y<-.001f){Debug.LogError("[Apartment Kitchen] ZONE/FLOOR ERROR: "+r.name);e++;}}
        if(kitchen.parent.Find("ExteriorWalls/LEFT_MAIN")==null){Debug.LogError("[Apartment Kitchen] LEFT_MAIN reference wall missing.");e++;}return e;
    }

    static GameObject Group(string name,Transform parent){var g=new GameObject(name);g.transform.SetParent(parent,false);return g;}
    static GameObject BoxPlan(string name,Transform parent,float minX,float maxX,float minY,float maxY,float bottom,float top,Material m)=>Box(name,parent,new Vector3((minX+maxX)*.5f*S,(bottom+top)*.5f,-(minY+maxY)*.5f*S),new Vector3((maxX-minX)*S,top-bottom,(maxY-minY)*S),m);
    static GameObject Box(string name,Transform parent,Vector3 position,Vector3 scale,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(parent,false);g.transform.localPosition=position;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static void Burner(string name,Transform parent,float x,float z,Material m){Cylinder(name,parent,new Vector3(x,.912f,z),.075f,.015f,m);}
    static GameObject Cylinder(string name,Transform parent,Vector3 position,float radius,float height,Material m){var g=GameObject.CreatePrimitive(PrimitiveType.Cylinder);g.name=name;g.transform.SetParent(parent,false);g.transform.localPosition=position;g.transform.localScale=new Vector3(radius*2,height*.5f,radius*2);g.GetComponent<Renderer>().sharedMaterial=m;return g;}
    static Material MaterialAsset(string name,Color color){const string folder="Assets/ApartmentShell/Materials";string path=$"{folder}/{name}.mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Standard")){name=name};AssetDatabase.CreateAsset(m,path);}m.color=color;EditorUtility.SetDirty(m);return m;}
    static void MarkStatic(GameObject g){g.isStatic=true;foreach(Transform child in g.transform)MarkStatic(child.gameObject);}
}
