using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

// Adds a 4th record (Aphex Twin "#21", supplied by the student) to the turntable exhibit,
// and a 4th button to the shared exhibit panel (other exhibits simply hide it).
public static class MuseumFourthRecordTool
{
    const string ClipPath="Assets/MuseumAudio/AphexTwin_21.wav";

    [MenuItem("Tools/Museum/Add Record 4 (Aphex Twin #21)")]
    public static void Add()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying||scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum and exit Play Mode.");return;}
        // Compressed + streamed so the long track stays light in memory (WebGL build).
        if(AssetImporter.GetAtPath(ClipPath) is AudioImporter ai){
            var st=ai.defaultSampleSettings;st.loadType=AudioClipLoadType.CompressedInMemory;st.compressionFormat=AudioCompressionFormat.Vorbis;st.quality=0.5f;
            ai.defaultSampleSettings=st;ai.loadInBackground=true;ai.SaveAndReimport();
        }
        var clip=AssetDatabase.LoadAssetAtPath<AudioClip>(ClipPath);
        var ex=GameObject.Find("Exhibit_00_RecordPlayer")?.GetComponent<MuseumExhibit>();
        var interactor=Object.FindFirstObjectByType<MuseumExhibitInteractor>();
        if(clip==null||ex==null||interactor==null){Debug.LogError("Need the clip, Exhibit_00_RecordPlayer and the interactor.");return;}
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        if(ex.modes.Length<4){
            Undo.RecordObject(ex,"record 4");
            var list=new System.Collections.Generic.List<MuseumExhibit.Mode>(ex.modes);
            list.Add(new MuseumExhibit.Mode{label="Aphex Twin  #21",caption="Record 4 — Aphex Twin, '#21'. Ambient electronic music made with synthesizers like the one next to this turntable. Press 4 again to stop.",music=clip,screenColor=Color.white});
            ex.modes=list.ToArray();
            ex.credit+=" Record 4: Aphex Twin – \"#21\" (copyright the artist/label; used without a licence in a non-commercial student project).";
        }

        if(interactor.modeButtons.Length<4){
            var third=interactor.modeButtons[2];
            var copy=Object.Instantiate(third.gameObject,third.transform.parent);copy.name="ModeButton_4";Undo.RegisterCreatedObjectUndo(copy,"button 4");
            var buttons=new Button[4];for(int i=0;i<3;i++)buttons[i]=interactor.modeButtons[i];buttons[3]=copy.GetComponent<Button>();
            for(int i=0;i<4;i++){   // re-lay the row as 4 equal columns
                var r=(RectTransform)buttons[i].transform;Undo.RecordObject(r,"layout");
                r.anchorMin=new Vector2(i/4f,r.anchorMin.y);r.anchorMax=new Vector2((i+1)/4f,r.anchorMax.y);
                r.offsetMin=new Vector2(i==0?0:4,0);r.offsetMax=new Vector2(i==3?0:-4,0);
            }
            Undo.RecordObject(interactor,"buttons");interactor.modeButtons=buttons;
            var hint=interactor.panelRoot.transform.Find("Content/KeysHint")?.GetComponent<TMPro.TMP_Text>();
            if(hint!=null){Undo.RecordObject(hint,"hint");hint.text="Press 1–4 or click a button";}
        }
        Undo.CollapseUndoOperations(undo);EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        Debug.Log("Record 4 (Aphex Twin #21) added to the turntable.");
    }
}
