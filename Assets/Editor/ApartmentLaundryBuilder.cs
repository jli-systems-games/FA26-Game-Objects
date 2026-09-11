using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentLaundryBuilder
{
    [MenuItem("Tools/Apartment/Build Stacked Laundry From Current Closet")]
    public static void Build()
    {
        var apartment = GameObject.Find("Apartment");
        var top = GameObject.Find("Apartment/InteriorWalls/WD_TOP");
        var right = GameObject.Find("Apartment/InteriorWalls/WD_RIGHT");
        var bottom = GameObject.Find("Apartment/InteriorWalls/CENTER_TOP_RIGHT");
        if (!apartment || !top || !right || !bottom) { Debug.LogError("[Laundry] Current W/D closet walls not found; nothing changed."); return; }

        Bounds tb = top.GetComponent<Renderer>().bounds, rb = right.GetComponent<Renderer>().bounds, bb = bottom.GetComponent<Renderer>().bounds;
        float zMax = tb.min.z, zMin = bb.max.z, backX = rb.min.x, frontLimit = Mathf.Max(tb.min.x, bb.min.x);
        float closetWidth = zMax - zMin, closetDepth = backX - frontLimit;
        if (closetWidth < .55f || closetDepth < .58f) { Debug.LogError("[Laundry] Measured closet is too small for a safe stack."); return; }

        float width = Mathf.Min(.60f, closetWidth - .10f);
        float depth = Mathf.Min(.66f, closetDepth - .07f);
        float height = .84f, gap = .03f;
        float centerZ = (zMin + zMax) * .5f, centerX = backX - .03f - depth * .5f;
        float frontX = centerX - depth * .5f;

        var old = apartment.transform.Find("Laundry"); if (old) Object.DestroyImmediate(old.gameObject);
        var root = Group("Laundry", apartment.transform);
        var body = MaterialAsset("Laundry_Body", new Color(.86f, .88f, .88f, 1));
        var panel = MaterialAsset("Laundry_Panel", new Color(.63f, .67f, .69f, 1));
        var dark = MaterialAsset("Laundry_Glass", new Color(.055f, .075f, .085f, 1));
        var metal = MaterialAsset("Laundry_Metal", new Color(.40f, .44f, .47f, 1));

        float washerBottom = .04f, dryerBottom = washerBottom + height + gap;
        BuildMachine("Washer", root.transform, centerX, centerZ, frontX, washerBottom, width, depth, height, body, panel, dark, metal, false);
        BuildMachine("Dryer", root.transform, centerX, centerZ, frontX, dryerBottom, width, depth, height, body, panel, dark, metal, true);
        Box("StackSpacer", root.transform, new Vector3(centerX, washerBottom + height + gap * .5f, centerZ), new Vector3(depth, gap, width), metal);

        MarkStatic(root);
        int errors = Validate(root.transform, frontLimit, backX, zMin, zMax, width, depth, height, washerBottom, dryerBottom);
        EditorSceneManager.MarkSceneDirty(apartment.scene);
        Selection.activeGameObject = root; SceneView.lastActiveSceneView?.FrameSelected();
        if (errors == 0) Debug.Log($"[Laundry] PASS — closet {closetWidth:F2}w x {closetDepth:F2}d; matched machines {width:F2}w x {depth:F2}d x {height:F2}h.");
        else Debug.LogError($"[Laundry] FAILED with {errors} validation error(s).");
    }

    static void BuildMachine(string name, Transform parent, float cx, float cz, float frontX, float bottom, float width, float depth, float height, Material body, Material panel, Material glass, Material metal, bool dryer)
    {
        var unit = Group(name, parent); float cy = bottom + height * .5f;
        Box("Body", unit.transform, new Vector3(cx, cy, cz), new Vector3(depth, height, width), body);
        Box("FrontPanel", unit.transform, new Vector3(frontX - .008f, cy - .02f, cz), new Vector3(.025f, height - .08f, width - .06f), body);
        float doorY = bottom + .38f;
        Cylinder("DoorOuterRing", unit.transform, new Vector3(frontX - .027f, doorY, cz), .205f, .035f, metal, new Vector3(0, 0, 90));
        Cylinder("DoorGlass", unit.transform, new Vector3(frontX - .048f, doorY, cz), .155f, .018f, glass, new Vector3(0, 0, 90));
        Box("ControlPanel", unit.transform, new Vector3(frontX - .028f, bottom + .755f, cz), new Vector3(.035f, .115f, width - .07f), panel);
        Cylinder("SelectorKnob", unit.transform, new Vector3(frontX - .052f, bottom + .755f, cz + (dryer ? .10f : -.10f)), .038f, .018f, glass, new Vector3(0, 0, 90));
        Box("Handle", unit.transform, new Vector3(frontX - .057f, doorY, cz + .165f), new Vector3(.025f, .035f, .105f), metal);
        Box(dryer ? "Feet_or_Trim" : "ToeKick", unit.transform, new Vector3(frontX - .025f, bottom + .045f, cz), new Vector3(.04f, .07f, width - .08f), panel);
        Box("Foot_Left", unit.transform, new Vector3(cx, bottom - .015f, cz - width * .36f), new Vector3(depth * .72f, .03f, .05f), metal);
        Box("Foot_Right", unit.transform, new Vector3(cx, bottom - .015f, cz + width * .36f), new Vector3(depth * .72f, .03f, .05f), metal);
    }

    static int Validate(Transform root, float frontLimit, float backX, float zMin, float zMax, float width, float depth, float height, float washerBottom, float dryerBottom)
    {
        int e = 0; Transform washer = root.Find("Washer"), dryer = root.Find("Dryer");
        string[] parts = { "Body", "FrontPanel", "DoorOuterRing", "DoorGlass", "ControlPanel", "Handle" };
        foreach (var p in parts) { if (!washer.Find(p)) { Debug.LogError("[Laundry] Washer missing " + p); e++; } if (!dryer.Find(p)) { Debug.LogError("[Laundry] Dryer missing " + p); e++; } }
        Bounds wb = washer.Find("Body").GetComponent<Renderer>().bounds, db = dryer.Find("Body").GetComponent<Renderer>().bounds;
        if (Mathf.Abs(wb.size.z - db.size.z) > .001f || Mathf.Abs(wb.size.x - db.size.x) > .001f || Mathf.Abs(wb.size.y - db.size.y) > .001f) { Debug.LogError("[Laundry] Machine dimensions are not identical."); e++; }
        if (Mathf.Abs(wb.center.x - db.center.x) > .001f || Mathf.Abs(wb.center.z - db.center.z) > .001f || db.min.y < wb.max.y + .029f) { Debug.LogError("[Laundry] Stack alignment or gap is incorrect."); e++; }
        if (wb.min.y < -.001f || wb.min.z < zMin || wb.max.z > zMax || wb.max.x > backX || wb.min.x < frontLimit) { Debug.LogError("[Laundry] Washer body does not fit closet bounds."); e++; }
        return e;
    }

    static GameObject Group(string n, Transform p) { var g = new GameObject(n); g.transform.SetParent(p, false); return g; }
    static GameObject Box(string n, Transform p, Vector3 pos, Vector3 scale, Material m) { var g = GameObject.CreatePrimitive(PrimitiveType.Cube); Setup(g, n, p, pos, scale, m); return g; }
    static GameObject Cylinder(string n, Transform p, Vector3 pos, float radius, float height, Material m, Vector3 rot)
    { var g = GameObject.CreatePrimitive(PrimitiveType.Cylinder); Setup(g, n, p, pos, new Vector3(radius * 2, height * .5f, radius * 2), m); g.transform.localEulerAngles = rot; return g; }
    static void Setup(GameObject g, string n, Transform p, Vector3 pos, Vector3 scale, Material m) { g.name = n; g.transform.SetParent(p, false); g.transform.localPosition = pos; g.transform.localScale = scale; g.GetComponent<Renderer>().sharedMaterial = m; }
    static Material MaterialAsset(string n, Color c)
    {
        string path = $"Assets/ApartmentShell/Materials/{n}.mat"; var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (!m) { m = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")) { name = n }; AssetDatabase.CreateAsset(m, path); }
        m.color = c; EditorUtility.SetDirty(m); return m;
    }
    static void MarkStatic(GameObject g) { g.isStatic = true; foreach (Transform c in g.transform) MarkStatic(c.gameObject); }
}
