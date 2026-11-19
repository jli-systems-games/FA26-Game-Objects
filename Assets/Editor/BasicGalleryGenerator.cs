using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class BasicGalleryGenerator
{
    private const string ScenePath = "Assets/Scenes/BasicGallery.unity";
    private static Transform root;

    [MenuItem("Tools/Generate Basic Gallery")]
    public static void Create()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        root = new GameObject("ORBITAL GALLERY | Modular Station Interior").transform;

        var floor = Mat("Space Gallery - Graphite Floor", new Color(.12f, .16f, .20f));
        var wall = Mat("Space Gallery - Titanium Panels", new Color(.35f, .42f, .48f));
        var wallInset = Mat("Space Gallery - Recessed Panels", new Color(.20f, .27f, .33f));
        var trim = Mat("Space Gallery - Dark Alloy", new Color(.055f, .075f, .09f));
        var cyan = Mat("Space Gallery - Cyan Emission", new Color(.08f, .72f, .95f), true);
        var amber = Mat("Space Gallery - Amber Emission", new Color(1f, .42f, .08f), true);
        var artBlue = Mat("Space Gallery - Exhibit Azure", new Color(.06f, .28f, .62f));
        var artRed = Mat("Space Gallery - Exhibit Vermilion", new Color(.70f, .12f, .08f));
        var artViolet = Mat("Space Gallery - Exhibit Violet", new Color(.34f, .12f, .68f));
        var glass = Mat("Space Gallery - Holo Glass", new Color(.08f, .50f, .62f), true);
        var doorMat = Mat("Space Gallery - Bulkhead Doors", new Color(.24f, .31f, .37f));
        var whiteLight = Mat("Space Gallery - Light Panels", new Color(.78f, .91f, 1f), true);

        // 8 x 6 m room built from 2 m grid modules.
        Box("Deck", new Vector3(0, -.16f, 0), new Vector3(16, .32f, 12), floor, root);
        for (int x = -7; x <= 7; x += 2)
            Box("Deck seam X " + x, new Vector3(x, .005f, 0), new Vector3(.025f, .012f, 12), trim, root);
        for (int z = -5; z <= 5; z += 2)
            Box("Deck seam Z " + z, new Vector3(0, .006f, z), new Vector3(16, .012f, .025f), trim, root);
        // Amber floor markers guide visitors towards the two entrance doors.
        for (int i = -1; i <= 1; i++)
            Box("Entry Direction Marker " + i, new Vector3(i * .48f, .018f, -4.9f), new Vector3(.14f, .025f, .8f), amber, root);

        // Side walls: repeated 2 m panels, seams, and two continuous utility rails.
        for (int i = 0; i < 6; i++)
        {
            float z = -5 + i * 2;
            WallModule("Port Wall Module " + (i + 1), new Vector3(-7.88f, 2, z), new Vector3(.24f, 4, 1.96f), wall, wallInset, trim, true);
            WallModule("Starboard Wall Module " + (i + 1), new Vector3(7.88f, 2, z), new Vector3(.24f, 4, 1.96f), wall, wallInset, trim, true);
        }
        for (int z = -5; z <= 5; z += 2)
        {
            Box("Port utility rail", new Vector3(-7.70f, .34f, z), new Vector3(.12f, .18f, 2), trim, root);
            Box("Starboard utility rail", new Vector3(7.70f, .34f, z), new Vector3(.12f, .18f, 2), trim, root);
            Box("Port upper light rail", new Vector3(-7.68f, 3.65f, z), new Vector3(.06f, .045f, 1.8f), cyan, root);
            Box("Starboard upper light rail", new Vector3(7.68f, 3.65f, z), new Vector3(.06f, .045f, 1.8f), cyan, root);
        }

        // End walls use the same 2 m panel rhythm. The entrance wall has a 4.4 m double doorway.
        for (int x = -7; x <= 7; x += 2)
            WallModule("Far bulkhead module " + x, new Vector3(x, 2, 5.88f), new Vector3(1.96f, 4, .24f), wall, wallInset, trim, false);
        for (int x = -7; x <= -3; x += 2)
            WallModule("Entry port module " + x, new Vector3(x, 2, -5.88f), new Vector3(1.96f, 4, .24f), wall, wallInset, trim, false);
        for (int x = 3; x <= 7; x += 2)
            WallModule("Entry starboard module " + x, new Vector3(x, 2, -5.88f), new Vector3(1.96f, 4, .24f), wall, wallInset, trim, false);
        Box("Entry header module", new Vector3(0, 3.68f, -5.88f), new Vector3(4.4f, .64f, .24f), wall, root);
        Box("Entry header inset", new Vector3(0, 3.68f, -5.72f), new Vector3(3.7f, .08f, .04f), cyan, root);

        MakeDoor("Port Operable Bulkhead", new Vector3(-2.2f, 0, -5.88f), doorMat, true);
        MakeDoor("Starboard Operable Bulkhead", new Vector3(2.2f, 0, -5.88f), doorMat, false);
        DoorControl("Port door control", new Vector3(-2.48f, 1.25f, -5.55f), cyan);
        DoorControl("Starboard door control", new Vector3(2.48f, 1.25f, -5.55f), amber);

        // Six illuminated exhibition panels and a large feature display.
        Material[] exhibitMaterials = { artBlue, artRed, artViolet };
        for (int i = 0; i < 3; i++)
        {
            float z = -3.7f + i * 3.7f;
            Exhibit("Port Exhibit " + (i + 1), new Vector3(-7.70f, 2.15f, z), exhibitMaterials[i], cyan, true);
            Exhibit("Starboard Exhibit " + (i + 1), new Vector3(7.70f, 2.15f, z), exhibitMaterials[(i + 1) % 3], amber, false);
        }
        Exhibit("Feature Hologram", new Vector3(0, 2.3f, 5.70f), glass, cyan, false, 3.6f, 2.5f);

        // Low modular display plinths in the central aisle, with simple sculptural pieces.
        for (int i = 0; i < 3; i++)
        {
            float z = -1.2f + i * 2.5f;
            Box("Exhibit plinth " + (i + 1), new Vector3(0, .32f, z), new Vector3(1.5f, .64f, 1.3f), trim, root);
            Box("Plinth light band " + (i + 1), new Vector3(0, .63f, z), new Vector3(1.52f, .055f, 1.32f), cyan, root);
            var sculpture = GameObject.CreatePrimitive(i == 1 ? PrimitiveType.Sphere : PrimitiveType.Cube);
            sculpture.name = "Holographic sculpture " + (i + 1);
            sculpture.transform.SetParent(root);
            sculpture.transform.position = new Vector3(0, 1.15f, z);
            sculpture.transform.localScale = i == 1 ? new Vector3(.52f, .52f, .52f) : new Vector3(.52f, .85f, .35f);
            sculpture.GetComponent<Renderer>().sharedMaterial = i == 1 ? glass : exhibitMaterials[i];
        }

        // Modular ceiling panels, lighting tracks, and cool/warm spotlights.
        for (int x = -6; x <= 6; x += 2)
            for (int z = -4; z <= 4; z += 2)
                Box("Ceiling panel " + x + " " + z, new Vector3(x, 4.15f, z), new Vector3(1.96f, .2f, 1.96f), wallInset, root);
        for (int z = -4; z <= 4; z += 2)
        {
            Box("Ceiling light tile " + z, new Vector3(0, 4.025f, z), new Vector3(1.3f, .035f, .22f), whiteLight, root);
            for (int side = -1; side <= 1; side += 2)
            {
                var spot = new GameObject("Exhibit Spotlight").AddComponent<Light>();
                spot.type = LightType.Spot;
                spot.color = side < 0 ? new Color(.48f, .78f, 1f) : new Color(1f, .68f, .38f);
                spot.intensity = 480;
                spot.range = 7;
                spot.spotAngle = 52;
                spot.transform.SetParent(root);
                spot.transform.position = new Vector3(side * 4.8f, 3.85f, z);
                spot.transform.rotation = Quaternion.Euler(90, side < 0 ? 0 : 180, 0);
            }
        }
        var keyLight = new GameObject("Station Interior Key Light").AddComponent<Light>();
        keyLight.type = LightType.Directional;
        keyLight.intensity = .45f;
        keyLight.color = new Color(.68f, .82f, 1f);
        keyLight.transform.rotation = Quaternion.Euler(42, -25, 0);

        var camera = new GameObject("Gallery Visitor Camera").AddComponent<Camera>();
        camera.transform.position = new Vector3(0, 1.7f, -8.1f);
        camera.transform.rotation = Quaternion.Euler(0, 0, 0);
        camera.fieldOfView = 72;
        camera.gameObject.tag = "MainCamera";
        camera.gameObject.AddComponent<AudioListener>();
        camera.gameObject.AddComponent<GalleryExplorerController>();

        EditorSceneManager.SaveScene(scene, ScenePath);
        AssetDatabase.SaveAssets();
        Debug.Log("Modular sci-fi gallery created at " + ScenePath);
    }

    [MenuItem("Tools/Add Second Gallery at Corridor End")]
    public static void AddSecondGalleryAtCorridorEnd()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        const string secondName = "GALLERY 02 | Atelier and Brass";
        // Detect the corridor from its long, narrow side-wall renderers.
        float corridorEndZ = -6f;
        bool foundCorridor = false;
        foreach (var renderer in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
        {
            var b = renderer.bounds;
            if (b.center.z < -6.1f && Mathf.Abs(b.center.x) < 3.5f && b.size.z > 5f && b.size.x < .8f)
            {
                corridorEndZ = foundCorridor ? Mathf.Min(corridorEndZ, b.min.z) : b.min.z;
                foundCorridor = true;
            }
        }
        if (!foundCorridor)
        {
            Debug.LogError("Could not find the corridor end. Save the corridor in BasicGallery.unity, then run this command again.");
            return;
        }
        var oldGallery = GameObject.Find(secondName);
        if (oldGallery != null) Object.DestroyImmediate(oldGallery);

        // The entrance faces back toward the corridor, so the room is rotated 180 degrees.
        float entryPlaneZ = corridorEndZ + .05f;
        float roomCenterZ = entryPlaneZ - 5.88f;
        var gallery = new GameObject(secondName);
        gallery.transform.position = new Vector3(0, 0, roomCenterZ);
        gallery.transform.rotation = Quaternion.Euler(0, 180, 0);
        root = gallery.transform;

        BuildSecondGallery();
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Second gallery added at the corridor end. Room center Z: " + roomCenterZ.ToString("F2"));
    }

    private static void BuildSecondGallery()
    {
        var floor = Mat("Atelier - Sandstone Floor", new Color(.48f, .42f, .34f));
        var wall = Mat("Atelier - Warm Ivory", new Color(.78f, .72f, .61f));
        var wood = Mat("Atelier - Walnut Panels", new Color(.23f, .115f, .075f));
        var dark = Mat("Atelier - Dark Bronze", new Color(.105f, .085f, .065f));
        var brass = Mat("Atelier - Brushed Brass", new Color(.68f, .40f, .14f));
        var warmLight = Mat("Atelier - Warm Light", new Color(1f, .72f, .36f), true);
        var redArt = Mat("Atelier - Exhibit Garnet", new Color(.50f, .12f, .16f));
        var blueArt = Mat("Atelier - Exhibit Indigo", new Color(.12f, .20f, .43f));
        var greenArt = Mat("Atelier - Exhibit Jade", new Color(.10f, .34f, .25f));
        var paleArt = Mat("Atelier - Exhibit Parchment", new Color(.78f, .61f, .35f));

        // Same 16 x 12 m footprint and 4 m wall height as the first gallery.
        BoxLocal("Atelier stone floor", new Vector3(0, -.16f, 0), new Vector3(16, .32f, 12), floor, root);
        for (int x = -7; x <= 7; x += 2)
            BoxLocal("Atelier floor joint X " + x, new Vector3(x, .005f, 0), new Vector3(.022f, .012f, 12), dark, root);
        for (int z = -5; z <= 5; z += 2)
            BoxLocal("Atelier floor joint Z " + z, new Vector3(0, .006f, z), new Vector3(16, .012f, .022f), dark, root);

        // Repeating walnut-and-ivory wall bays keep a modular rhythm with a distinctly warmer finish.
        for (int i = 0; i < 6; i++)
        {
            float z = -5 + i * 2;
            AtelierWallBay("Atelier port bay " + (i + 1), new Vector3(-7.88f, 2, z), true, wall, wood, brass);
            AtelierWallBay("Atelier starboard bay " + (i + 1), new Vector3(7.88f, 2, z), true, wall, wood, brass);
        }
        for (int x = -7; x <= 7; x += 2)
            AtelierWallBay("Atelier far bay " + x, new Vector3(x, 2, 5.88f), false, wall, wood, brass);
        for (int x = -7; x <= -3; x += 2)
            AtelierWallBay("Atelier entry port bay " + x, new Vector3(x, 2, -5.88f), false, wall, wood, brass);
        for (int x = 3; x <= 7; x += 2)
            AtelierWallBay("Atelier entry starboard bay " + x, new Vector3(x, 2, -5.88f), false, wall, wood, brass);
        BoxLocal("Atelier entry lintel", new Vector3(0, 3.68f, -5.88f), new Vector3(4.4f, .64f, .24f), wood, root);
        BoxLocal("Atelier entry lintel brass", new Vector3(0, 3.55f, -5.72f), new Vector3(3.6f, .045f, .035f), brass, root);

        MakeLocalDoor("Atelier port double door", new Vector3(-2.2f, 0, -5.88f), true, wood, brass);
        MakeLocalDoor("Atelier starboard double door", new Vector3(2.2f, 0, -5.88f), false, wood, brass);

        Material[] art = { redArt, blueArt, greenArt, paleArt };
        for (int i = 0; i < 3; i++)
        {
            float z = -3.7f + i * 3.7f;
            AtelierArtwork("Atelier port painting " + (i + 1), new Vector3(-7.70f, 2.15f, z), art[i], brass, true);
            AtelierArtwork("Atelier starboard painting " + (i + 1), new Vector3(7.70f, 2.15f, z), art[i + 1], brass, false);
        }
        AtelierArtwork("Atelier signature piece", new Vector3(0, 2.25f, 5.70f), blueArt, brass, false, 3.4f, 2.2f);

        for (int i = 0; i < 3; i++)
        {
            float z = -1.2f + i * 2.5f;
            BoxLocal("Atelier sculpture pedestal " + (i + 1), new Vector3(0, .38f, z), new Vector3(1.45f, .76f, 1.25f), wood, root);
            BoxLocal("Atelier brass pedestal edge " + (i + 1), new Vector3(0, .74f, z), new Vector3(1.48f, .045f, 1.28f), brass, root);
            var sculpture = GameObject.CreatePrimitive(i == 1 ? PrimitiveType.Sphere : PrimitiveType.Capsule);
            sculpture.name = "Atelier sculpture " + (i + 1);
            sculpture.transform.SetParent(root, false);
            sculpture.transform.localPosition = new Vector3(0, 1.35f, z);
            sculpture.transform.localScale = i == 1 ? new Vector3(.65f, .65f, .65f) : new Vector3(.55f, .9f, .55f);
            sculpture.GetComponent<Renderer>().sharedMaterial = art[i];
        }

        for (int x = -6; x <= 6; x += 2)
            for (int z = -4; z <= 4; z += 2)
                BoxLocal("Atelier ceiling coffer " + x + " " + z, new Vector3(x, 4.15f, z), new Vector3(1.96f, .2f, 1.96f), wall, root);
        for (int z = -4; z <= 4; z += 2)
        {
            BoxLocal("Atelier ceiling diffuser " + z, new Vector3(0, 4.025f, z), new Vector3(1.4f, .04f, .24f), warmLight, root);
            var spot = new GameObject("Atelier painting light").AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.color = new Color(1f, .76f, .48f);
            spot.intensity = 420;
            spot.range = 7;
            spot.spotAngle = 48;
            spot.transform.SetParent(root, false);
            spot.transform.localPosition = new Vector3(0, 3.85f, z);
            spot.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
    }

    private static void AtelierWallBay(string name, Vector3 pos, bool sideWall, Material plaster, Material wood, Material brass)
    {
        Vector3 size = sideWall ? new Vector3(.24f, 4, 1.96f) : new Vector3(1.96f, 4, .24f);
        BoxLocal(name, pos, size, plaster, root);
        Vector3 insetOffset = sideWall ? new Vector3(-Mathf.Sign(pos.x) * .14f, 0, 0) : new Vector3(0, 0, -Mathf.Sign(pos.z) * .14f);
        Vector3 insetSize = sideWall ? new Vector3(.04f, 3.35f, 1.48f) : new Vector3(1.48f, 3.35f, .04f);
        BoxLocal(name + " walnut inset", pos + insetOffset, insetSize, wood, root);
        Vector3 brassOffset = sideWall ? new Vector3(-Mathf.Sign(pos.x) * .17f, -.92f, 0) : new Vector3(0, -.92f, -Mathf.Sign(pos.z) * .17f);
        Vector3 brassSize = sideWall ? new Vector3(.035f, .055f, 1.65f) : new Vector3(1.65f, .055f, .035f);
        BoxLocal(name + " brass rail", pos + brassOffset, brassSize, brass, root);
    }

    private static void MakeLocalDoor(string name, Vector3 hingePosition, bool extendsRight, Material wood, Material brass)
    {
        float direction = extendsRight ? 1 : -1;
        var hinge = new GameObject(name);
        hinge.transform.SetParent(root, false);
        hinge.transform.localPosition = hingePosition;
        var leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leaf.name = "Walnut door leaf";
        leaf.transform.SetParent(hinge.transform, false);
        leaf.transform.localPosition = new Vector3(direction * 1.1f, 1.5f, 0);
        leaf.transform.localScale = new Vector3(2.2f, 3f, .2f);
        leaf.GetComponent<Renderer>().sharedMaterial = wood;
        var inset = GameObject.CreatePrimitive(PrimitiveType.Cube);
        inset.name = "Door inset";
        inset.transform.SetParent(hinge.transform, false);
        inset.transform.localPosition = new Vector3(direction * 1.1f, 1.5f, -.11f);
        inset.transform.localScale = new Vector3(1.68f, 2.42f, .035f);
        inset.GetComponent<Renderer>().sharedMaterial = Mat("Atelier - Door Inset", new Color(.31f, .16f, .09f));
        var pull = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pull.name = "Brass door pull";
        pull.transform.SetParent(hinge.transform, false);
        pull.transform.localPosition = new Vector3(direction * .22f, 1.45f, -.15f);
        pull.transform.localScale = new Vector3(.09f, .5f, .055f);
        pull.GetComponent<Renderer>().sharedMaterial = brass;
    }

    private static void AtelierArtwork(string name, Vector3 pos, Material art, Material frame, bool onPortWall, float width = 1.7f, float height = 1.55f)
    {
        bool sideWall = Mathf.Abs(pos.x) > 1;
        Vector3 face = sideWall ? new Vector3(-Mathf.Sign(pos.x), 0, 0) : new Vector3(0, 0, -1);
        Vector3 frameSize = sideWall ? new Vector3(.13f, height + .2f, width + .2f) : new Vector3(width + .2f, height + .2f, .13f);
        Vector3 artSize = sideWall ? new Vector3(.06f, height, width) : new Vector3(width, height, .06f);
        BoxLocal(name + " brass frame", pos + face * .05f, frameSize, frame, root);
        BoxLocal(name, pos + face * .13f, artSize, art, root);
        Vector3 lightPos = pos + Vector3.up * (height * .5f + .14f) + face * .17f;
        Vector3 lightSize = sideWall ? new Vector3(.035f, .035f, width + .12f) : new Vector3(width + .12f, .035f, .035f);
        BoxLocal(name + " picture light", lightPos, lightSize, Mat("Atelier - Picture Light", new Color(1f, .72f, .36f), true), root);
    }

    private static void BoxLocal(string name, Vector3 localPosition, Vector3 localScale, Material material, Transform parent)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = localScale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
    }

    private static void WallModule(string name, Vector3 pos, Vector3 scale, Material shell, Material inset, Material trim, bool sideWall)
    {
        Box(name, pos, scale, shell, root);
        Vector3 insetOffset = sideWall ? new Vector3(Mathf.Sign(pos.x) * -.135f, 0, 0) : new Vector3(0, 0, -.135f * Mathf.Sign(pos.z));
        Vector3 insetScale = sideWall ? new Vector3(.035f, 2.9f, 1.55f) : new Vector3(1.55f, 2.9f, .035f);
        Box(name + " recessed face", pos + insetOffset + Vector3.up * .1f, insetScale, inset, root);
        for (int edge = -1; edge <= 1; edge += 2)
        {
            Vector3 edgeOffset = sideWall ? new Vector3(Mathf.Sign(pos.x) * -.145f, 0, edge * .93f) : new Vector3(edge * .93f, 0, -.145f * Mathf.Sign(pos.z));
            Vector3 edgeScale = sideWall ? new Vector3(.045f, 3.85f, .045f) : new Vector3(.045f, 3.85f, .045f);
            Box(name + " seam trim", pos + edgeOffset, edgeScale, trim, root);
        }
    }

    private static void MakeDoor(string name, Vector3 hingePosition, Material material, bool extendsRight)
    {
        var hinge = new GameObject(name);
        hinge.transform.SetParent(root);
        hinge.transform.position = hingePosition;
        float direction = extendsRight ? 1 : -1;
        var leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leaf.name = "Door Leaf";
        leaf.transform.SetParent(hinge.transform, false);
        leaf.transform.localPosition = new Vector3(direction * 1.1f, 1.5f, 0);
        leaf.transform.localScale = new Vector3(2.2f, 3f, .2f);
        leaf.GetComponent<Renderer>().sharedMaterial = material;
        var inset = GameObject.CreatePrimitive(PrimitiveType.Cube);
        inset.name = "Door center inset";
        inset.transform.SetParent(hinge.transform, false);
        inset.transform.localPosition = new Vector3(direction * 1.1f, 1.5f, -.115f);
        inset.transform.localScale = new Vector3(1.72f, 2.5f, .04f);
        inset.GetComponent<Renderer>().sharedMaterial = Mat("Space Gallery - Door Inset", new Color(.12f, .19f, .24f));
        var stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stripe.name = "Door status stripe";
        stripe.transform.SetParent(hinge.transform, false);
        stripe.transform.localPosition = new Vector3(direction * 1.1f, 1.5f, -.145f);
        stripe.transform.localScale = new Vector3(.08f, 2.25f, .035f);
        stripe.GetComponent<Renderer>().sharedMaterial = Mat("Space Gallery - Door Status Light", new Color(.08f, .72f, .95f), true);
    }

    private static void DoorControl(string name, Vector3 pos, Material active)
    {
        Box(name, pos, new Vector3(.16f, .34f, .08f), Mat("Space Gallery - Door Control", new Color(.07f, .09f, .11f)), root);
        Box(name + " indicator", pos + new Vector3(0, 0, -.05f), new Vector3(.08f, .1f, .025f), active, root);
    }

    private static void Exhibit(string name, Vector3 pos, Material art, Material glow, bool onLeftWall, float width = 1.65f, float height = 1.55f)
    {
        bool sideWall = Mathf.Abs(pos.x) > 1;
        Vector3 frameScale = sideWall ? new Vector3(.12f, height + .22f, width + .22f) : new Vector3(width + .22f, height + .22f, .12f);
        Vector3 canvasScale = sideWall ? new Vector3(.05f, height, width) : new Vector3(width, height, .05f);
        var framePos = pos + (sideWall ? new Vector3(-Mathf.Sign(pos.x) * .05f, 0, 0) : new Vector3(0, 0, -.05f));
        Box(name + " alloy frame", framePos, frameScale, Mat("Space Gallery - Display Frame", new Color(.07f, .09f, .11f)), root);
        var canvasPos = pos + (sideWall ? new Vector3(-Mathf.Sign(pos.x) * .12f, 0, 0) : new Vector3(0, 0, -.12f));
        Box(name + " artwork", canvasPos, canvasScale, art, root);
        Vector3 glowPos = canvasPos + (sideWall ? new Vector3(-Mathf.Sign(pos.x) * .04f, height * .5f + .04f, 0) : new Vector3(0, height * .5f + .04f, -.04f));
        Vector3 glowScale = sideWall ? new Vector3(.04f, .035f, width + .08f) : new Vector3(width + .08f, .035f, .04f);
        Box(name + " upper luminaire", glowPos, glowScale, glow, root);
    }

    private static void Box(string name, Vector3 pos, Vector3 scale, Material material, Transform parent)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.position = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material;
    }

    private static Material Mat(string name, Color color, bool emission = false)
    {
        const string folder = "Assets/Scenes/GeneratedMaterials";
        if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets/Scenes", "GeneratedMaterials");
        var path = folder + "/" + name.Replace("/", "-") + ".mat";
        var material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            material.name = name;
            AssetDatabase.CreateAsset(material, path);
        }
        material.color = color;
        if (emission && material.HasProperty("_EmissionColor"))
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color * 2.3f);
        }
        return material;
    }
}
