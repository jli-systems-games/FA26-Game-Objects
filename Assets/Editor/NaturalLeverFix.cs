using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// One-time, surgical migration. Does not rebuild the scene or replace materials.
[InitializeOnLoad]
public static class NaturalLeverFix
{
    const string Pending = "Temp/NaturalLeverFix.pending";
    static NaturalLeverFix() { if (!Application.isBatchMode) EditorApplication.update += ApplyPending; }

    public static void Configure(GameObject lever)
    {
        // Keep the domino-facing left edge at -1.8m; trim only the right edge.
        // The old right edge penetrated CatchRamp and blocked hinge rotation.
        var collider = lever.GetComponent<BoxCollider>();
        var visual = lever.transform.Find("Visual");
        if (!collider || !visual) throw new InvalidOperationException("Lever collider/Visual is missing.");
        collider.size = new Vector3(3.4f, .25f, 1.1f);
        collider.center = new Vector3(-.1f, 0, 0);
        visual.localPosition = collider.center;
        visual.localScale = collider.size;
    }

    static void ApplyPending()
    {
        if (!File.Exists(Pending)) return;
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        if (EditorApplication.isPlayingOrWillChangePlaymode) { EditorApplication.isPlaying = false; return; }
        EditorApplication.update -= ApplyPending;
        try { Apply(); File.Delete(Pending); }
        catch (Exception e) { Debug.LogError("[Natural Lever Fix] " + e); }
    }

    [MenuItem("Tools/Rube Goldberg/Fix Natural Lever Passage")]
    public static void Apply()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        var scene = SceneManager.GetSceneByPath(RubeGoldbergSceneBuilder.ScenePath);
        bool wasLoaded = scene.IsValid() && scene.isLoaded;
        if (!wasLoaded) scene = EditorSceneManager.OpenScene(RubeGoldbergSceneBuilder.ScenePath, OpenSceneMode.Additive);
        var root = scene.GetRootGameObjects().First(g => g.name == "RubeGoldbergMachine");
        var lever = root.transform.Find("Stage03_Lever/LeverArm");
        if (!lever) throw new InvalidOperationException("LeverArm was not found.");
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        var before = renderers.SelectMany(r => r.sharedMaterials).ToArray();
        Directory.CreateDirectory("Assets/RubeGoldberg/Backups");
        string backup = "Assets/RubeGoldberg/Backups/BeforeNaturalLever_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".unity";
        if (!EditorSceneManager.SaveScene(scene, backup, true)) throw new IOException("Could not save scene backup.");
        Configure(lever.gameObject);
        bool preserved = before.SequenceEqual(renderers.SelectMany(r => r.sharedMaterials));
        if (!preserved) throw new InvalidOperationException("Unexpected material change.");
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene)) throw new IOException("Could not save corrected scene.");
        Directory.CreateDirectory("Logs");
        File.WriteAllText("Logs/NaturalLeverFixApplied.txt", "Applied to " + scene.path + "\nMaterials preserved: True\nBackup: " + backup + "\nCollider size: (3.4, 0.25, 1.1); center: (-0.1, 0, 0)\n");
        if (!wasLoaded) EditorSceneManager.CloseScene(scene, true);
        Debug.Log("[Natural Lever Fix] Saved; existing materials preserved. Backup: " + backup);
    }
}
