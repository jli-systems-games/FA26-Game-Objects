using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MuseumPolygonFloorTool
{
    [MenuItem("Tools/Museum/Create Polygon Floor")]
    public static void CreateFloor()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/MidtermMuseum.unity" || EditorApplication.isPlaying)
        {
            Debug.LogError("Open MidtermMuseum in Edit Mode first.");
            return;
        }
        GameObject museum = null;
        foreach (var root in scene.GetRootGameObjects())
            if (root.name == "Museum") museum = root;
        if (museum == null) { Debug.LogError("Museum root not found."); return; }
        if (museum.transform.Find("MuseumFloor_Polygon") != null)
        { Selection.activeGameObject = museum.transform.Find("MuseumFloor_Polygon").gameObject; return; }

        var oldFloor = museum.transform.Find("Floor");
        var oldRenderer = oldFloor != null ? oldFloor.GetComponent<Renderer>() : null;
        var material = oldRenderer != null ? oldRenderer.sharedMaterial :
            AssetDatabase.LoadAssetAtPath<Material>("Assets/MuseumFloor.mat");
        var outline = new Vector2[] {
            new Vector2(-4,-18), new Vector2(-12,-8), new Vector2(-12,10),
            new Vector2(-4,18), new Vector2(10,14), new Vector2(12,-6)
        };
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uv = new List<Vector2>();
        System.Action<Vector3,Vector3,Vector3> addTriangle = (a,b,c) => {
            int start = vertices.Count;
            vertices.Add(a); vertices.Add(b); vertices.Add(c);
            triangles.Add(start); triangles.Add(start+1); triangles.Add(start+2);
            uv.Add(new Vector2(a.x/4,a.z/4));
            uv.Add(new Vector2(b.x/4,b.z/4));
            uv.Add(new Vector2(c.x/4,c.z/4));
        };
        for (int i=0; i<outline.Length; i++)
        {
            var p=outline[i]; var q=outline[(i+1)%outline.Length];
            var a=new Vector3(p.x,0,p.y); var b=new Vector3(q.x,0,q.y);
            var c=new Vector3(p.x,-0.5f,p.y); var d=new Vector3(q.x,-0.5f,q.y);
            addTriangle(Vector3.zero,a,b);
            addTriangle(new Vector3(0,-0.5f,0),d,c);
            addTriangle(a,c,b); addTriangle(b,c,d);
        }
        var mesh = new Mesh { name="MuseumFloor_PolygonMesh" };
        mesh.SetVertices(vertices); mesh.SetTriangles(triangles,0); mesh.SetUVs(0,uv);
        mesh.RecalculateNormals(); mesh.RecalculateBounds();
        if (!AssetDatabase.IsValidFolder("Assets/MuseumGenerated"))
            AssetDatabase.CreateFolder("Assets","MuseumGenerated");
        var assetPath=AssetDatabase.GenerateUniqueAssetPath("Assets/MuseumGenerated/MuseumFloor_PolygonMesh.asset");
        AssetDatabase.CreateAsset(mesh,assetPath);
        Undo.IncrementCurrentGroup();
        int group=Undo.GetCurrentGroup(); Undo.SetCurrentGroupName("Create museum polygon floor");
        var floor=new GameObject("MuseumFloor_Polygon");
        Undo.RegisterCreatedObjectUndo(floor,"Create polygon floor");
        floor.transform.SetParent(museum.transform,false);
        floor.AddComponent<MeshFilter>().sharedMesh=mesh;
        floor.AddComponent<MeshRenderer>().sharedMaterial=material;
        floor.AddComponent<MeshCollider>().sharedMesh=mesh;
        if (oldFloor != null) {
            Undo.RecordObject(oldFloor.gameObject,"Keep original floor as backup");
            oldFloor.gameObject.SetActive(false);
        }
        Undo.CollapseUndoOperations(group);
        Selection.activeGameObject=floor;
        EditorSceneManager.MarkSceneDirty(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.SaveScene(scene);
        if (SceneView.lastActiveSceneView != null) SceneView.lastActiveSceneView.FrameSelected();
        Debug.Log("Created MuseumFloor_Polygon: 24 x 36 m bounds, 0.5 m thick, collider enabled; original Floor retained inactive.");
    }
}
