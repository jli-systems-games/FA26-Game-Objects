using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class MuseumViewingLoungeTool
{
    public static void Build()
    {
        if(EditorApplication.isPlaying)throw new System.InvalidOperationException("Exit Play Mode first.");
        var piano=GameObject.Find("Exhibit_ClassicalPiano");if(!piano)throw new System.InvalidOperationException("Piano not found.");
        var record=GameObject.Find("Exhibit_00_RecordPlayer");
        if(record){
            // Its old box enclosed the whole rug AND the tall furniture, making the rug a solid wall.
            foreach(var box in record.GetComponents<BoxCollider>()){Undo.RecordObject(box,"Fix record player collision");box.enabled=false;}
            foreach(var filter in record.GetComponentsInChildren<MeshFilter>()){
                if(filter.name.Contains("Focus") || !filter.sharedMesh)continue;
                var collider=filter.GetComponent<MeshCollider>();if(!collider)collider=Undo.AddComponent<MeshCollider>(filter.gameObject);
                collider.sharedMesh=filter.sharedMesh;collider.convex=false;
            }
        }
        if(!GameObject.Find("PianoViewingSofa")){
            var root=new GameObject("PianoViewingSofa");Undo.RegisterCreatedObjectUndo(root,"Add viewing sofa");
            root.transform.position=piano.transform.TransformPoint(new Vector3(0,0,-2.5f));root.transform.rotation=piano.transform.rotation;
            var fabric=Material("Sofa_Velvet",new Color(.11f,.13f,.24f));var trim=Material("Sofa_Trim",new Color(.035f,.04f,.065f));
            Box(root,"Base",new Vector3(0,.24f,0),new Vector3(2.5f,.32f,.92f),trim);
            Box(root,"Cushion",new Vector3(0,.48f,-.02f),new Vector3(2.2f,.23f,.85f),fabric);
            Box(root,"Back",new Vector3(0,.8f,-.44f),new Vector3(2.5f,.8f,.2f),fabric);
            Box(root,"ArmLeft",new Vector3(-1.16f,.64f,0),new Vector3(.2f,.5f,1f),fabric);
            Box(root,"ArmRight",new Vector3(1.16f,.64f,0),new Vector3(.2f,.5f,1f),fabric);
            var pillow=Box(root,"Pillow",new Vector3(-.84f,.68f,0),new Vector3(.44f,.18f,.65f),fabric);pillow.transform.localRotation=Quaternion.Euler(0,0,12);
            var sofa=root.AddComponent<MuseumViewingSofa>();
            sofa.headRest=Anchor(root,"RecliningView",new Vector3(-.8f,.94f,.08f));
            sofa.bodyRest=Anchor(root,"RecliningBody",new Vector3(.7f,.67f,0));sofa.bodyRest.localRotation=Quaternion.Euler(0,0,90);
            sofa.screen=piano.GetComponent<MuseumPiano>().performanceScreen.transform;
        }
        EditorSceneManager.MarkSceneDirty(piano.scene);EditorSceneManager.SaveScene(piano.scene);AssetDatabase.SaveAssets();
    }
    static Transform Anchor(GameObject root,string name,Vector3 position){var g=new GameObject(name);g.transform.SetParent(root.transform,false);g.transform.localPosition=position;return g.transform;}
    static Material Material(string name,Color color){string path="Assets/MuseumGenerated/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(m)return m;m=new Material(Shader.Find("Universal Render Pipeline/Lit")){name=name};m.color=color;AssetDatabase.CreateAsset(m,path);return m;}
    static GameObject Box(GameObject root,string name,Vector3 pos,Vector3 size,Material material){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=name;g.transform.SetParent(root.transform,false);g.transform.localPosition=pos;g.transform.localScale=size;g.GetComponent<Renderer>().sharedMaterial=material;return g;}
}
