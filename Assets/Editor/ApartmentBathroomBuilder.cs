using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentBathroomBuilder
{
    [MenuItem("Tools/Apartment/Build Bathroom Fixtures From Current Walls")]
    public static void Build()
    {
        var apartment = GameObject.Find("Apartment");
        if (!apartment) { Debug.LogError("[Bathroom] Apartment root not found; nothing changed."); return; }

        var west = GameObject.Find("Apartment/ExteriorWalls/SERVICE_WEST");
        var south = GameObject.Find("Apartment/ExteriorWalls/SERVICE_BOTTOM_MAIN");
        var east = GameObject.Find("Apartment/InteriorWalls/CENTER_CORE_VERTICAL");
        var north = GameObject.Find("Apartment/InteriorWalls/CENTER_BRIDGE");
        if (!west || !south || !east || !north) { Debug.LogError("[Bathroom] Bathroom boundary walls are missing; nothing changed."); return; }

        float xMin = west.GetComponent<Renderer>().bounds.max.x;
        float xMax = east.GetComponent<Renderer>().bounds.min.x;
        float zMin = south.GetComponent<Renderer>().bounds.max.z;
        float zMax = north.GetComponent<Renderer>().bounds.min.z;
        if (xMax - xMin < 1.9f || zMax - zMin < 1.7f) { Debug.LogError("[Bathroom] Current enclosure is too small or could not be measured."); return; }

        var old = apartment.transform.Find("BathroomFixtures");
        if (old) Object.DestroyImmediate(old.gameObject);
        var root = Group("BathroomFixtures", apartment.transform);

        var porcelain = MaterialAsset("Bathroom_Porcelain", new Color(.91f, .92f, .90f, 1));
        var inner = MaterialAsset("Bathroom_BasinInterior", new Color(.67f, .74f, .76f, 1));
        var metal = MaterialAsset("Bathroom_Metal", new Color(.48f, .53f, .57f, 1));
        var vanity = MaterialAsset("Bathroom_Vanity", new Color(.54f, .46f, .38f, 1));
        var dark = MaterialAsset("Bathroom_Dark", new Color(.10f, .12f, .13f, 1));

        BuildTub(root.transform, xMax, zMin, zMax, porcelain, inner, metal);
        BuildToilet(root.transform, xMin, zMin, porcelain, inner, metal);
        BuildSink(root.transform, xMin, zMax, porcelain, inner, metal, vanity, dark);

        MarkStatic(root);
        int errors = Validate(root.transform, xMin, xMax, zMin, zMax);
        EditorSceneManager.MarkSceneDirty(apartment.scene);
        Selection.activeGameObject = root;
        SceneView.lastActiveSceneView?.FrameSelected();
        if (errors == 0) Debug.Log($"[Bathroom] PASS — fixtures fitted to current enclosure x={xMin:F2}..{xMax:F2}, z={zMin:F2}..{zMax:F2}.");
        else Debug.LogError($"[Bathroom] FAILED with {errors} validation error(s).");
    }

    static void BuildTub(Transform parent, float eastFace, float southFace, float northFace, Material white, Material basin, Material metal)
    {
        var tub = Group("Bathtub", parent);
        float width = .75f, length = Mathf.Min(1.70f, northFace - southFace - .20f);
        float cx = eastFace - width * .5f - .05f, cz = southFace + length * .5f + .15f;
        float westX = cx - width * .5f, eastX = cx + width * .5f, northZ = cz + length * .5f, southZ = cz - length * .5f;
        Box("Tub_Base", tub.transform, new Vector3(cx, .08f, cz), new Vector3(width, .16f, length), white);
        Box("Tub_LeftWall", tub.transform, new Vector3(westX + .035f, .31f, cz), new Vector3(.07f, .46f, length), white);
        Box("Tub_RightWall", tub.transform, new Vector3(eastX - .035f, .31f, cz), new Vector3(.07f, .46f, length), white);
        Box("Tub_EndWall_01", tub.transform, new Vector3(cx, .31f, northZ - .035f), new Vector3(width - .14f, .46f, .07f), white);
        Box("Tub_EndWall_02", tub.transform, new Vector3(cx, .31f, southZ + .035f), new Vector3(width - .14f, .46f, .07f), white);
        Box("InnerBottom", tub.transform, new Vector3(cx, .175f, cz), new Vector3(width - .16f, .025f, length - .18f), basin);
        Box("Rim_Left", tub.transform, new Vector3(westX + .035f, .555f, cz), new Vector3(.085f, .04f, length), white);
        Box("Rim_Right", tub.transform, new Vector3(eastX - .035f, .555f, cz), new Vector3(.085f, .04f, length), white);
        Box("Rim_North", tub.transform, new Vector3(cx, .555f, northZ - .035f), new Vector3(width - .17f, .04f, .085f), white);
        Box("Rim_South", tub.transform, new Vector3(cx, .555f, southZ + .035f), new Vector3(width - .17f, .04f, .085f), white);
        Cylinder("Drain", tub.transform, new Vector3(cx, .195f, southZ + .24f), .045f, .012f, metal);

        // Plumbing is anchored to the existing north wall face and projects over the basin.
        float hardwareZ = northFace - .035f;
        Cylinder("FaucetControl", tub.transform, new Vector3(cx, .91f, hardwareZ), .07f, .045f, metal, new Vector3(90, 0, 0));
        Box("Spout", tub.transform, new Vector3(cx, .66f, hardwareZ - .11f), new Vector3(.06f, .055f, .24f), metal);
        Cylinder("ShowerPipe", tub.transform, new Vector3(cx, 1.46f, hardwareZ - .025f), .018f, 1.08f, metal);
        Box("ShowerArm", tub.transform, new Vector3(cx, 2.00f, hardwareZ - .10f), new Vector3(.035f, .035f, .20f), metal);
        Cylinder("ShowerHead", tub.transform, new Vector3(cx, 1.97f, hardwareZ - .21f), .085f, .035f, metal, new Vector3(72, 0, 0));
    }

    static void BuildToilet(Transform parent, float westFace, float southFace, Material white, Material water, Material metal)
    {
        var toilet = Group("Toilet", parent);
        float z = southFace + .78f;
        Box("Base", toilet.transform, new Vector3(westFace + .43f, .145f, z), new Vector3(.42f, .29f, .32f), white);
        Sphere("Bowl", toilet.transform, new Vector3(westFace + .58f, .39f, z), new Vector3(.50f, .23f, .42f), white);
        Cylinder("Seat", toilet.transform, new Vector3(westFace + .59f, .465f, z), .235f, .035f, white, Vector3.zero, new Vector3(1.15f, 1, .92f));
        Cylinder("BowlOpening", toilet.transform, new Vector3(westFace + .61f, .487f, z), .16f, .012f, water, Vector3.zero, new Vector3(1.12f, 1, .88f));
        Box("Tank", toilet.transform, new Vector3(westFace + .16f, .60f, z), new Vector3(.22f, .42f, .43f), white);
        Box("TankLid", toilet.transform, new Vector3(westFace + .16f, .825f, z), new Vector3(.25f, .035f, .46f), white);
        Box("FlushHandle", toilet.transform, new Vector3(westFace + .285f, .69f, z - .13f), new Vector3(.035f, .035f, .12f), metal);
    }

    static void BuildSink(Transform parent, float westFace, float northFace, Material white, Material basin, Material metal, Material wood, Material dark)
    {
        var sink = Group("BathroomSink", parent);
        float cx = westFace + .23f, cz = northFace - .35f;
        Box("VanityBase", sink.transform, new Vector3(cx, .40f, cz), new Vector3(.40f, .80f, .56f), wood);
        Box("Countertop", sink.transform, new Vector3(cx, .84f, cz), new Vector3(.44f, .08f, .60f), white);
        Sphere("Basin", sink.transform, new Vector3(cx + .025f, .875f, cz), new Vector3(.30f, .07f, .36f), basin);
        Box("CabinetReveal", sink.transform, new Vector3(cx + .205f, .48f, cz), new Vector3(.012f, .55f, .45f), dark);
        Cylinder("FaucetStem", sink.transform, new Vector3(cx - .15f, .98f, cz), .022f, .20f, metal);
        Box("FaucetSpout", sink.transform, new Vector3(cx - .075f, 1.07f, cz), new Vector3(.15f, .035f, .035f), metal);
        Cylinder("Handle_Left", sink.transform, new Vector3(cx - .13f, .93f, cz - .12f), .025f, .07f, metal);
        Cylinder("Handle_Right", sink.transform, new Vector3(cx - .13f, .93f, cz + .12f), .025f, .07f, metal);
    }

    static int Validate(Transform root, float xMin, float xMax, float zMin, float zMax)
    {
        int e = 0;
        string[] required = { "Bathtub/Tub_Base", "Bathtub/InnerBottom", "Bathtub/Spout", "Bathtub/ShowerPipe", "Bathtub/ShowerHead", "Toilet/Base", "Toilet/Bowl", "Toilet/Seat", "Toilet/Tank", "BathroomSink/VanityBase", "BathroomSink/Basin", "BathroomSink/FaucetStem" };
        foreach (var path in required) if (!root.Find(path)) { Debug.LogError("[Bathroom] Missing " + path); e++; }
        foreach (var r in root.GetComponentsInChildren<Renderer>())
        {
            var b = r.bounds;
            if (b.min.x < xMin - .001f || b.max.x > xMax + .001f || b.min.z < zMin - .001f || b.max.z > zMax + .001f || b.min.y < -.001f)
            { Debug.LogError("[Bathroom] WALL/FLOOR ERROR: " + r.name); e++; }
        }
        Bounds tub = BoundsOf(root.Find("Bathtub")), toilet = BoundsOf(root.Find("Toilet")), sink = BoundsOf(root.Find("BathroomSink"));
        if (tub.Intersects(toilet) || tub.Intersects(sink) || toilet.Intersects(sink)) { Debug.LogError("[Bathroom] Fixture bounds overlap."); e++; }
        return e;
    }

    static Bounds BoundsOf(Transform t) { var rs = t.GetComponentsInChildren<Renderer>(); var b = rs[0].bounds; for (int i = 1; i < rs.Length; i++) b.Encapsulate(rs[i].bounds); return b; }
    static GameObject Group(string name, Transform parent) { var g = new GameObject(name); g.transform.SetParent(parent, false); return g; }
    static GameObject Box(string name, Transform parent, Vector3 p, Vector3 s, Material m) { var g = GameObject.CreatePrimitive(PrimitiveType.Cube); Setup(g, name, parent, p, s, m); return g; }
    static GameObject Sphere(string name, Transform parent, Vector3 p, Vector3 s, Material m) { var g = GameObject.CreatePrimitive(PrimitiveType.Sphere); Setup(g, name, parent, p, s, m); return g; }
    static GameObject Cylinder(string name, Transform parent, Vector3 p, float radius, float height, Material m, Vector3 rotation = default, Vector3 stretch = default)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cylinder); if (stretch == default) stretch = Vector3.one;
        Setup(g, name, parent, p, new Vector3(radius * 2 * stretch.x, height * .5f * stretch.y, radius * 2 * stretch.z), m); g.transform.localEulerAngles = rotation; return g;
    }
    static void Setup(GameObject g, string name, Transform parent, Vector3 p, Vector3 s, Material m) { g.name = name; g.transform.SetParent(parent, false); g.transform.localPosition = p; g.transform.localScale = s; g.GetComponent<Renderer>().sharedMaterial = m; }
    static Material MaterialAsset(string name, Color color)
    {
        const string folder = "Assets/ApartmentShell/Materials"; string path = $"{folder}/{name}.mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path); if (!m) { m = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard")) { name = name }; AssetDatabase.CreateAsset(m, path); }
        m.color = color; EditorUtility.SetDirty(m); return m;
    }
    static void MarkStatic(GameObject g) { g.isStatic = true; foreach (Transform child in g.transform) MarkStatic(child.gameObject); }
}
