using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SecondGalleryExhibits
{
    const string ScenePath = "Assets/Scenes/BasicGallery.unity";
    const string GalleryName = "GALLERY 02 | Atelier and Brass";

    [MenuItem("Tools/Add Sun and Fire to Second Gallery")]
    public static void Add()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

        var gallery = GameObject.Find(GalleryName);
        if (gallery == null) { Debug.LogError("Second Gallery root not found."); return; }

        var old = gallery.transform.Find("SUN | Solar Exhibit");
        if (old != null) Object.DestroyImmediate(old.gameObject);
        old = gallery.transform.Find("FIRE | Flame Exhibit");
        if (old != null) Object.DestroyImmediate(old.gameObject);

        BuildSun(gallery.transform);
        BuildFire(gallery.transform);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Sun and fire exhibits added to Second Gallery.");
    }

    [MenuItem("Tools/Rebuild Fire Model in Second Gallery")]
    public static void RebuildFireOnly()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != ScenePath)
            scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        var gallery = GameObject.Find(GalleryName);
        if (gallery == null) { Debug.LogError("Second Gallery root not found."); return; }

        var old = gallery.transform.Find("FIRE | Flame Model");
        if (old != null) Object.DestroyImmediate(old.gameObject);
        var previous = gallery.transform.Find("FIRE | Flame Exhibit");
        if (previous != null) Object.DestroyImmediate(previous.gameObject);

        BuildFire(gallery.transform);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("Standalone flame model rebuilt and saved in Second Gallery.");
    }
    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parent = System.IO.Path.GetDirectoryName(path).Replace("\\", "/");
        var leaf = System.IO.Path.GetFileName(path);
        EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, leaf);
    }

    static string SafeName(string name)
    {
        return name.Replace(" | ", "_").Replace(" ", "_").Replace("/", "_");
    }

    static Material Mat(string name, Color color, float emission)
    {
        const string folder = "Assets/Materials/SecondGalleryExhibits";
        EnsureFolder(folder);
        string path = folder + "/" + SafeName(name) + ".mat";
        var m = AssetDatabase.LoadAssetAtPath<Material>(path);
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (m == null)
        {
            m = new Material(shader);
            AssetDatabase.CreateAsset(m, path);
        }
        else m.shader = shader;
        m.name = name;
        m.color = color;
        m.EnableKeyword("_EMISSION");
        m.SetColor("_EmissionColor", color * emission);
        EditorUtility.SetDirty(m);
        return m;
    }

    static GameObject Part(Transform parent, string name, PrimitiveType type, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = GameObject.CreatePrimitive(type);
        go.name = name;
        go.transform.SetParent(parent, false);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    static void Pedestal(Transform parent, string prefix, float x, Material stone, Material brass)
    {
        Part(parent, prefix + " | walnut plinth", PrimitiveType.Cube, new Vector3(x, .42f, 1.2f), new Vector3(2.25f, .84f, 2.1f), stone);
        Part(parent, prefix + " | brass top", PrimitiveType.Cube, new Vector3(x, .86f, 1.2f), new Vector3(2.3f, .06f, 2.15f), brass);
    }

    static void BuildSun(Transform parent)
    {
        var root = new GameObject("SUN | Solar Exhibit").transform;
        root.SetParent(parent, false);
        float x = -4f;
        var dark = Mat("Exhibit | Midnight Walnut", new Color(.12f,.065f,.04f), .05f);
        var brass = Mat("Exhibit | Polished Brass", new Color(.8f,.48f,.16f), .25f);
        var corona = Mat("Sun | Corona", new Color(1f,.22f,.015f), 2.3f);
        var core = Mat("Sun | Photosphere", new Color(1f,.72f,.06f), 3.5f);
        var hot = Mat("Sun | Hot Spots", new Color(1f,.94f,.35f), 2.5f);
        Pedestal(root, "Sun", x, dark, brass);
        Part(root, "Solar corona", PrimitiveType.Sphere, new Vector3(x,2.05f,1.2f), new Vector3(1.72f,1.72f,1.72f), corona);
        Part(root, "Solar photosphere", PrimitiveType.Sphere, new Vector3(x,2.05f,1.2f), new Vector3(1.37f,1.37f,1.37f), core);
        Part(root, "Solar bright center", PrimitiveType.Sphere, new Vector3(x-.12f,2.12f,.55f), new Vector3(.48f,.48f,.16f), hot);
        for (int i=0;i<12;i++)
        {
            float a = i * Mathf.PI / 6f;
            var ray = Part(root, "Corona ray " + (i+1), PrimitiveType.Capsule,
                new Vector3(x + Mathf.Sin(a)*1.03f, 2.05f + Mathf.Cos(a)*1.03f, 1.2f),
                new Vector3(.20f,.52f,.20f), i%2==0 ? core : corona);
            ray.transform.localRotation = Quaternion.Euler(0,0,-a*Mathf.Rad2Deg);
        }
        var light = new GameObject("Sun | warm exhibit light").AddComponent<Light>();
        light.transform.SetParent(root,false);
        light.transform.localPosition = new Vector3(x,2.15f,.25f);
        light.type = LightType.Point; light.color = new Color(1f,.42f,.08f); light.intensity = 95; light.range = 5;
    }

    static Mesh FlameMesh(string name)
    {
        var mesh = new Mesh(); mesh.name = name;
        int segments=20;
        float[] radii={.34f,.46f,.41f,.29f,.15f};
        float[] heights={0f,.26f,.62f,1.02f,1.38f};
        var v=new Vector3[segments*5+1];
        var t=new int[segments*6*4];
        for(int r=0;r<5;r++)
            for(int i=0;i<segments;i++)
            {
                float a=2*Mathf.PI*i/segments;
                v[r*segments+i]=new Vector3(Mathf.Cos(a)*radii[r],heights[r],Mathf.Sin(a)*radii[r]);
            }
        v[segments*5]=new Vector3(.13f,1.72f,0);
        int ti=0;
        for(int r=0;r<4;r++) for(int i=0;i<segments;i++)
        {
            int n=(i+1)%segments, a=r*segments+i,b=r*segments+n,c=(r+1)*segments+i,d=(r+1)*segments+n;
            t[ti++]=a;t[ti++]=c;t[ti++]=b;t[ti++]=b;t[ti++]=c;t[ti++]=d;
        }
        int top=segments*5, last=4*segments;
        for(int i=0;i<segments;i++){int n=(i+1)%segments;t[ti++]=last+i;t[ti++]=top; t[ti++]=last+n;}
        var trimmed=new int[ti]; System.Array.Copy(t,trimmed,ti);
        mesh.vertices=v; mesh.triangles=trimmed; mesh.RecalculateNormals(); return mesh;
    }

    static GameObject Flame(Transform parent,string name,Vector3 pos,Vector3 scale,Material mat)
    {
        var go=new GameObject(name); go.transform.SetParent(parent,false); go.transform.localPosition=pos; go.transform.localScale=scale;
        var mf=go.AddComponent<MeshFilter>();
        string folder = "Assets/Models/SecondGalleryExhibits";
        EnsureFolder(folder);
        string meshPath = folder + "/" + SafeName(name) + ".asset";
        var mesh = FlameMesh(name + " mesh");
        var savedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        if (savedMesh == null) { AssetDatabase.CreateAsset(mesh, meshPath); savedMesh = mesh; }
        else { EditorUtility.CopySerialized(mesh, savedMesh); Object.DestroyImmediate(mesh); }
        mf.sharedMesh=savedMesh;
        var mr=go.AddComponent<MeshRenderer>(); mr.sharedMaterial=mat;
        return go;
    }

    static void BuildFire(Transform parent)
    {
        var root = new GameObject("FIRE | Flame Model").transform;
        root.SetParent(parent, false);
        root.localPosition = new Vector3(4f, .04f, 1.2f);
        root.localRotation = Quaternion.identity;
        root.localScale = Vector3.one;

        var red = Mat("Fire | Deep Ember", new Color(.95f,.055f,.008f), 1.8f);
        var orange = Mat("Fire | Bright Orange", new Color(1f,.24f,.012f), 2.8f);
        var gold = Mat("Fire | Golden Core", new Color(1f,.76f,.10f), 3.2f);

        var outer = Part(root, "Flame | outer silhouette", PrimitiveType.Capsule,
            new Vector3(0f,1.30f,0f), new Vector3(.92f,1.35f,.78f), red);
        outer.transform.localRotation = Quaternion.Euler(0f,0f,-3f);

        var leftTongue = Part(root, "Flame | left dancing tongue", PrimitiveType.Capsule,
            new Vector3(-.29f,1.84f,0f), new Vector3(.49f,.91f,.44f), orange);
        leftTongue.transform.localRotation = Quaternion.Euler(0f,0f,22f);

        var rightTongue = Part(root, "Flame | right dancing tongue", PrimitiveType.Capsule,
            new Vector3(.27f,1.76f,.04f), new Vector3(.46f,.80f,.42f), orange);
        rightTongue.transform.localRotation = Quaternion.Euler(0f,0f,-20f);

        Part(root, "Flame | central orange body", PrimitiveType.Capsule,
            new Vector3(0f,1.03f,-.08f), new Vector3(.64f,1.20f,.55f), orange);

        Part(root, "Flame | golden heart", PrimitiveType.Capsule,
            new Vector3(-.02f,.74f,-.36f), new Vector3(.38f,.78f,.32f), gold);

        var light = new GameObject("Flame | warm glow").AddComponent<Light>();
        light.transform.SetParent(root, false);
        light.transform.localPosition = new Vector3(0f,1.1f,-.65f);
        light.type = LightType.Point;
        light.color = new Color(1f,.20f,.025f);
        light.intensity = 5f;
        light.range = 5f;
    }
}
