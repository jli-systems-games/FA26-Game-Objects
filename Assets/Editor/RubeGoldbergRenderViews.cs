using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public static class RubeGoldbergRenderViews {
 public static void Run(){ShaderUtil.allowAsyncCompilation=false;EditorSceneManager.OpenScene(RubeGoldbergSceneBuilder.ScenePath);Directory.CreateDirectory("Logs/RubeViews");foreach(var camera in Object.FindObjectsByType<Camera>()){var rt=new RenderTexture(1280,720,24);camera.targetTexture=rt;camera.Render();camera.Render();RenderTexture.active=rt;var texture=new Texture2D(1280,720,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1280,720),0,0);texture.Apply();File.WriteAllBytes("Logs/RubeViews/"+camera.name+".png",texture.EncodeToPNG());camera.targetTexture=null;RenderTexture.active=null;Object.DestroyImmediate(texture);Object.DestroyImmediate(rt);}Debug.Log("[Machine Render] Four camera views saved");}
}



