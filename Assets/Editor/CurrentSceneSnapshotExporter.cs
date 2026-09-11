using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CurrentSceneSnapshotExporter
{
    const string OutputAssetPath = "Assets/SceneSnapshots/current_scene_snapshot.json";

    [Serializable] class Snapshot
    {
        public string sceneSnapshotVersion;
        public string unitySceneName;
        public string unitySceneAssetPath;
        public string generatedAt;
        public CoordinateInfo sceneCoordinateInfo;
        public List<ObjectRecord> objects = new List<ObjectRecord>();
        public List<AssemblyRecord> assemblies = new List<AssemblyRecord>();
        public SpatialRelationships importantSpatialRelationships;
        public List<OverlapRecord> overlaps = new List<OverlapRecord>();
    }
    [Serializable] class CoordinateInfo { public AxisInfo unityAxes; }
    [Serializable] class AxisInfo { public string x; public string y; public string z; }
    [Serializable] class Vec3 { public float x, y, z; public Vec3(Vector3 v) { x=v.x; y=v.y; z=v.z; } }
    [Serializable] class TransformRecord { public Vec3 position, rotationEuler, scale; }
    [Serializable] class BoundsRecord
    {
        public Vec3 center, size, min, max;
        public BoundsRecord(Bounds b) { center=new Vec3(b.center); size=new Vec3(b.size); min=new Vec3(b.min); max=new Vec3(b.max); }
    }
    [Serializable] class Dimensions { public float worldWidthX, worldHeightY, worldDepthZ; }
    [Serializable] class MaterialRecord { public string name, assetPath; public float[] color; }
    [Serializable] class ObjectRecord
    {
        public string path, name, parentPath, meshType, mesh, collider;
        public bool active, activeSelf, activeInHierarchy, isStatic, hasRenderer;
        public TransformRecord worldTransform, localTransform;
        public BoundsRecord rendererBounds;
        public Dimensions derivedDimensions;
        public List<MaterialRecord> materials = new List<MaterialRecord>();
    }
    [Serializable] class AssemblyRecord { public string path; public int objectCount, rendererCount; public BoundsRecord combinedBounds; }
    [Serializable] class NamedBounds { public string name, path, note; public BoundsRecord bounds; }
    [Serializable] class SpatialSection { public List<NamedBounds> measurements = new List<NamedBounds>(); public string note; }
    [Serializable] class SpatialRelationships { public SpatialSection kitchen, bathroom, laundry, openAreas; }
    [Serializable] class OverlapRecord { public string objectA, objectB; public BoundsRecord intersectionBounds; public string note; }

    [MenuItem("Tools/Apartment/Export Current Scene Snapshot (Read Only)")]
    public static void Export()
    {
        var root = GameObject.Find("Apartment");
        if (!root) { Debug.LogError("[Scene Snapshot] Apartment root not found; no file written."); return; }
        var scene = root.scene;
        var snapshot = new Snapshot {
            sceneSnapshotVersion = "CURRENT_MANUALLY_CORRECTED_SCENE",
            unitySceneName = scene.name,
            unitySceneAssetPath = scene.path,
            generatedAt = DateTime.UtcNow.ToString("o"),
            sceneCoordinateInfo = new CoordinateInfo { unityAxes = new AxisInfo { x="right/left", y="up/down", z="forward/back" } }
        };

        var transforms = root.GetComponentsInChildren<Transform>(true);
        foreach (var t in transforms) snapshot.objects.Add(DescribeObject(t, root.transform));

        string[] assemblyPaths = {
            "Apartment", "Apartment/Floor", "Apartment/ExteriorWalls", "Apartment/InteriorWalls", "Apartment/Windows",
            "Apartment/Kitchen", "Apartment/Kitchen/BaseCabinets", "Apartment/Kitchen/Countertops", "Apartment/Kitchen/Stove",
            "Apartment/Kitchen/Sink", "Apartment/Kitchen/UpperCabinets", "Apartment/BathroomFixtures",
            "Apartment/BathroomFixtures/Bathtub", "Apartment/BathroomFixtures/Toilet", "Apartment/BathroomFixtures/BathroomSink",
            "Apartment/Laundry", "Apartment/Laundry/Washer", "Apartment/Laundry/Dryer"
        };
        foreach (string path in assemblyPaths) { var go=GameObject.Find(path); if (go) snapshot.assemblies.Add(DescribeAssembly(go.transform, root.transform)); }

        snapshot.importantSpatialRelationships = BuildRelationships(root.transform);
        snapshot.overlaps = FindOverlaps(root.transform);

        string absolute = Path.GetFullPath(OutputAssetPath);
        Directory.CreateDirectory(Path.GetDirectoryName(absolute));
        File.WriteAllText(absolute, JsonUtility.ToJson(snapshot, true));
        AssetDatabase.ImportAsset(OutputAssetPath, ImportAssetOptions.ForceSynchronousImport);
        Debug.Log($"[Scene Snapshot] Exported {snapshot.objects.Count} objects, {snapshot.assemblies.Count} assemblies, and {snapshot.overlaps.Count} neutral overlaps to {absolute}");
    }

    static ObjectRecord DescribeObject(Transform t, Transform apartment)
    {
        var renderers=t.GetComponents<Renderer>(); bool hasBounds=renderers.Length>0; Bounds b=default;
        if (hasBounds) { b=renderers[0].bounds; for(int i=1;i<renderers.Length;i++) b.Encapsulate(renderers[i].bounds); }
        var mf=t.GetComponent<MeshFilter>(); var collider=t.GetComponent<Collider>();
        var record=new ObjectRecord {
            path=PathOf(t,apartment), name=t.name, parentPath=t.parent?PathOf(t.parent,apartment):"",
            active=t.gameObject.activeInHierarchy, activeSelf=t.gameObject.activeSelf, activeInHierarchy=t.gameObject.activeInHierarchy, isStatic=t.gameObject.isStatic, hasRenderer=hasBounds,
            meshType=InferMeshType(mf), mesh=mf&&mf.sharedMesh?mf.sharedMesh.name:null, collider=collider?collider.GetType().Name:null,
            worldTransform=new TransformRecord { position=new Vec3(t.position), rotationEuler=new Vec3(t.eulerAngles), scale=new Vec3(t.lossyScale) },
            localTransform=new TransformRecord { position=new Vec3(t.localPosition), rotationEuler=new Vec3(t.localEulerAngles), scale=new Vec3(t.localScale) },
            rendererBounds=hasBounds?new BoundsRecord(b):null,
            derivedDimensions=hasBounds?new Dimensions { worldWidthX=b.size.x, worldHeightY=b.size.y, worldDepthZ=b.size.z }:null
        };
        foreach(var m in renderers.SelectMany(r=>r.sharedMaterials).Where(m=>m).Distinct())
        {
            Color c=m.HasProperty("_BaseColor")?m.GetColor("_BaseColor"):m.HasProperty("_Color")?m.color:Color.white;
            record.materials.Add(new MaterialRecord { name=m.name, assetPath=AssetDatabase.GetAssetPath(m), color=new[]{c.r,c.g,c.b,c.a} });
        }
        return record;
    }

    static AssemblyRecord DescribeAssembly(Transform t, Transform apartment)
    {
        var rs=t.GetComponentsInChildren<Renderer>(true); Bounds b=default; bool found=false;
        foreach(var r in rs) { if(!found){b=r.bounds;found=true;} else b.Encapsulate(r.bounds); }
        return new AssemblyRecord { path=PathOf(t,apartment), objectCount=t.GetComponentsInChildren<Transform>(true).Length, rendererCount=rs.Length, combinedBounds=found?new BoundsRecord(b):null };
    }

    static SpatialRelationships BuildRelationships(Transform root)
    {
        var result=new SpatialRelationships {
            kitchen=new SpatialSection(), bathroom=new SpatialSection(), laundry=new SpatialSection(),
            openAreas=new SpatialSection { note="Open floor zones were not inferred; use the exported floor and object bounds for downstream clearance analysis." }
        };
        AddBounds(result.kitchen,"kitchenTotal","Apartment/Kitchen");
        AddBounds(result.kitchen,"countertopTotal","Apartment/Kitchen/Countertops");
        AddBounds(result.kitchen,"stove","Apartment/Kitchen/Stove");
        AddBounds(result.kitchen,"sink","Apartment/Kitchen/Sink");
        AddBounds(result.kitchen,"upperCabinets","Apartment/Kitchen/UpperCabinets");
        AddBounds(result.bathroom,"bathtub","Apartment/BathroomFixtures/Bathtub");
        AddBounds(result.bathroom,"toilet","Apartment/BathroomFixtures/Toilet");
        AddBounds(result.bathroom,"bathroomSink","Apartment/BathroomFixtures/BathroomSink");
        AddBounds(result.laundry,"washer","Apartment/Laundry/Washer");
        AddBounds(result.laundry,"dryer","Apartment/Laundry/Dryer");

        Bounds bath; if(TryUsableBounds(new[]{"Apartment/ExteriorWalls/SERVICE_WEST","Apartment/InteriorWalls/CENTER_CORE_VERTICAL","Apartment/ExteriorWalls/SERVICE_BOTTOM_MAIN","Apartment/InteriorWalls/CENTER_BRIDGE"},out bath))
            result.bathroom.measurements.Insert(0,new NamedBounds{name="approximateUsableBounds",path="derived from current boundary wall faces",note="Axis-aligned enclosure estimate from current wall renderer bounds.",bounds=new BoundsRecord(bath)});
        Bounds laundry; if(TryLaundryBounds(out laundry))
            result.laundry.measurements.Insert(0,new NamedBounds{name="closetApproximateBounds",path="derived from WD_TOP, WD_RIGHT, and CENTER_TOP_RIGHT",note="Axis-aligned niche volume from current wall faces; height capped at current wall height.",bounds=new BoundsRecord(laundry)});
        return result;
    }

    static void AddBounds(SpatialSection section,string name,string path)
    {
        var go=GameObject.Find(path); if(!go)return; Bounds b; if(!CombinedBounds(go.transform,out b))return;
        section.measurements.Add(new NamedBounds{name=name,path=path,bounds=new BoundsRecord(b)});
    }
    static bool TryUsableBounds(string[] p,out Bounds b)
    {
        b=default; GameObject west=GameObject.Find(p[0]),east=GameObject.Find(p[1]),south=GameObject.Find(p[2]),north=GameObject.Find(p[3]);
        if(!west||!east||!south||!north)return false;
        var wb=west.GetComponent<Renderer>().bounds;var eb=east.GetComponent<Renderer>().bounds;var sb=south.GetComponent<Renderer>().bounds;var nb=north.GetComponent<Renderer>().bounds;
        float xmin=wb.max.x,xmax=eb.min.x,zmin=sb.max.z,zmax=nb.min.z;
        b=new Bounds(new Vector3((xmin+xmax)/2,1.35f,(zmin+zmax)/2),new Vector3(xmax-xmin,2.7f,zmax-zmin));return true;
    }
    static bool TryLaundryBounds(out Bounds b)
    {
        b=default;GameObject top=GameObject.Find("Apartment/InteriorWalls/WD_TOP");GameObject right=GameObject.Find("Apartment/InteriorWalls/WD_RIGHT");GameObject bottom=GameObject.Find("Apartment/InteriorWalls/CENTER_TOP_RIGHT");
        if(!top||!right||!bottom)return false;var tb=top.GetComponent<Renderer>().bounds;var rb=right.GetComponent<Renderer>().bounds;var bb=bottom.GetComponent<Renderer>().bounds;
        float xmin=Mathf.Max(tb.min.x,bb.min.x),xmax=rb.min.x,zmin=bb.max.z,zmax=tb.min.z;
        b=new Bounds(new Vector3((xmin+xmax)/2,1.35f,(zmin+zmax)/2),new Vector3(xmax-xmin,2.7f,zmax-zmin));return true;
    }

    static List<OverlapRecord> FindOverlaps(Transform root)
    {
        var list=new List<OverlapRecord>();var rs=root.GetComponentsInChildren<Renderer>(true).Where(r=>r.enabled&&r.gameObject.activeInHierarchy).ToArray();
        for(int i=0;i<rs.Length;i++)for(int j=i+1;j<rs.Length;j++)
        {
            if(!AreLogicalNeighbors(rs[i].transform,rs[j].transform,root))continue;
            Bounds a=rs[i].bounds,c=rs[j].bounds;if(!a.Intersects(c))continue;
            Vector3 min=Vector3.Max(a.min,c.min),max=Vector3.Min(a.max,c.max),size=max-min;
            if(size.x<=.0005f||size.y<=.0005f||size.z<=.0005f)continue;
            var ib=new Bounds((min+max)*.5f,size);
            list.Add(new OverlapRecord{objectA=PathOf(rs[i].transform,root),objectB=PathOf(rs[j].transform,root),intersectionBounds=new BoundsRecord(ib),note="overlap detected; may be intentional"});
        }
        return list;
    }
    static bool AreLogicalNeighbors(Transform a,Transform b,Transform root)
    {
        string ga=MajorGroup(a,root),gb=MajorGroup(b,root);if(ga==gb)return true;
        bool aa=ga=="ExteriorWalls"||ga=="InteriorWalls"||ga=="Windows",ab=gb=="ExteriorWalls"||gb=="InteriorWalls"||gb=="Windows";
        return aa&&ab;
    }
    static string MajorGroup(Transform t,Transform root){while(t.parent&&t.parent!=root)t=t.parent;return t.name;}
    static bool CombinedBounds(Transform t,out Bounds b){b=default;bool found=false;foreach(var r in t.GetComponentsInChildren<Renderer>(true)){if(!found){b=r.bounds;found=true;}else b.Encapsulate(r.bounds);}return found;}
    static string InferMeshType(MeshFilter mf){if(!mf||!mf.sharedMesh)return null;string n=mf.sharedMesh.name;return n.IndexOf("Cube",StringComparison.OrdinalIgnoreCase)>=0?"Cube":n.IndexOf("Sphere",StringComparison.OrdinalIgnoreCase)>=0?"Sphere":n.IndexOf("Cylinder",StringComparison.OrdinalIgnoreCase)>=0?"Cylinder":n.IndexOf("Plane",StringComparison.OrdinalIgnoreCase)>=0?"Plane":n.IndexOf("Quad",StringComparison.OrdinalIgnoreCase)>=0?"Quad":"Mesh";}
    static string PathOf(Transform t,Transform apartment)
    {
        var names=new List<string>();Transform p=t;while(p){names.Add(p.name);if(p==apartment)break;p=p.parent;}names.Reverse();return string.Join("/",names);
    }
}
