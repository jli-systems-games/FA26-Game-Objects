using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Builds the iPhone sample exhibit (clearly labelled placeholder model) and the shared exhibit UI.
public static class MuseumExhibitTool
{
    const string Folder="Assets/MuseumGenerated";
    static Color C(string h){ColorUtility.TryParseHtmlString(h,out var c);return c;}
    static Material Mat(string name,Color c,float metal,float smooth,Color emission)
    {
        string path=Folder+"/"+name+".mat";
        var m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(m!=null)return m;
        m=new Material(Shader.Find("Universal Render Pipeline/Lit")){name=name};
        m.SetColor("_BaseColor",c);m.SetFloat("_Metallic",metal);m.SetFloat("_Smoothness",smooth);
        if(emission.maxColorComponent>0){m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",emission);m.globalIlluminationFlags=MaterialGlobalIlluminationFlags.BakedEmissive;}
        AssetDatabase.CreateAsset(m,path);return m;
    }
    static GameObject Prim(PrimitiveType t,string name,Transform parent,Vector3 pos,Vector3 scale,Material m,bool collider=false)
    {
        var g=GameObject.CreatePrimitive(t);g.name=name;g.transform.SetParent(parent,false);
        g.transform.localPosition=pos;g.transform.localScale=scale;g.GetComponent<Renderer>().sharedMaterial=m;
        if(!collider)Object.DestroyImmediate(g.GetComponent<Collider>());return g;
    }
    static TextMeshPro WorldText(string name,Transform parent,Vector3 pos,string text,float size,Color color)
    {
        var g=new GameObject(name,typeof(TextMeshPro));g.transform.SetParent(parent,false);g.transform.localPosition=pos;
        // TMP faces -Z by default = the plinth front
        var t=g.GetComponent<TextMeshPro>();t.text=text;t.fontSize=size;t.color=color;t.alignment=TextAlignmentOptions.Center;
        t.rectTransform.sizeDelta=new Vector2(2.4f,0.6f);return t;
    }

    [MenuItem("Tools/Museum/Add iPhone Sample Exhibit")]
    public static void Build()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(EditorApplication.isPlaying || scene.path!="Assets/Scenes/MidtermMuseum.unity"){Debug.LogError("Open MidtermMuseum first.");return;}
        if(GameObject.Find("Exhibit_01_iPhone")!=null){Selection.activeGameObject=GameObject.Find("Exhibit_01_iPhone");Debug.Log("iPhone exhibit already exists.");return;}
        var surface=GameObject.Find("DisplaySurface_6");
        var visitor=Object.FindFirstObjectByType<MuseumVisitorController>();
        var canvas=GameObject.Find("Canvas")?.GetComponent<Canvas>();
        if(surface==null||visitor==null||canvas==null){Debug.LogError("Need DisplaySurface_6, MuseumVisitor and Canvas.");return;}
        AssetDatabase.CopyAsset(scene.path,AssetDatabase.GenerateUniqueAssetPath(Folder+"/MidtermMuseum_BeforeIPhoneExhibit.unity"));
        Undo.IncrementCurrentGroup();int undo=Undo.GetCurrentGroup();

        var titanium=Mat("Exhibit_PhoneTitanium",C("#2A2F3A"),0.85f,0.82f,Color.black);
        var glassBlack=Mat("Exhibit_PhoneGlass",C("#07080D"),0.2f,0.95f,Color.black);
        var screenMat=Mat("Exhibit_PhoneScreen",C("#41E9EB"),0,0.9f,C("#41E9EB")*2);
        var silver=Mat("Exhibit_Silver",C("#A9B6C6"),0.8f,0.75f,Color.black);
        var ring=Mat("Exhibit_FocusGlow",C("#FF9BD2"),0,0.6f,C("#FF7FC8")*2.5f);
        var gold=Mat("Exhibit_WarmGold",C("#FFC478"),0.6f,0.7f,C("#FFB060")*0.6f);

        // ----- Exhibit object -----
        var root=new GameObject("Exhibit_01_iPhone");Undo.RegisterCreatedObjectUndo(root,"iPhone exhibit");
        root.transform.SetParent(surface.transform.parent,false);
        root.transform.position=surface.transform.position+Vector3.up*0.02f;
        var exhibit=root.AddComponent<MuseumExhibit>();

        Prim(PrimitiveType.Cylinder,"Stand",root.transform,new Vector3(0,0.03f,0),new Vector3(0.6f,0.03f,0.6f),silver);
        var model=new GameObject("PlaceholderPhone_5xScale").transform;model.SetParent(root.transform,false);model.localPosition=new Vector3(0,0.5f,0);
        Prim(PrimitiveType.Cube,"Body",model,Vector3.zero,new Vector3(0.36f,0.74f,0.05f),titanium);
        Prim(PrimitiveType.Cube,"FrontGlass",model,new Vector3(0,0,-0.026f),new Vector3(0.34f,0.72f,0.004f),glassBlack);
        var screen=Prim(PrimitiveType.Cube,"Screen",model,new Vector3(0,0,-0.029f),new Vector3(0.31f,0.66f,0.003f),screenMat);
        Prim(PrimitiveType.Cube,"Earpiece",model,new Vector3(0,0.345f,-0.031f),new Vector3(0.07f,0.012f,0.003f),silver);
        Prim(PrimitiveType.Cube,"SideButton",model,new Vector3(0.182f,0.15f,0),new Vector3(0.008f,0.09f,0.02f),silver);
        var lens=Prim(PrimitiveType.Cylinder,"CameraBump",model,new Vector3(-0.1f,0.28f,0.03f),new Vector3(0.09f,0.008f,0.09f),glassBlack);
        lens.transform.localRotation=Quaternion.Euler(90,0,0);

        // Mode 2 objects: things the phone absorbed (simple labelled placeholders)
        var absorbed=new GameObject("Mode2_AbsorbedObjects").transform;absorbed.SetParent(root.transform,false);absorbed.localPosition=new Vector3(0,0.5f,0);
        string[] names={"CAMERA","MAP","MUSIC","WALLET"};
        for(int i=0;i<4;i++){
            float a=i*Mathf.PI/2+0.4f;var p=new Vector3(Mathf.Cos(a)*0.75f,Mathf.Sin(i*1.7f)*0.15f,Mathf.Sin(a)*0.45f);
            var item=Prim(i==1?PrimitiveType.Cylinder:PrimitiveType.Cube,"Absorbed_"+names[i],absorbed,p,i==1?new Vector3(0.18f,0.01f,0.18f):new Vector3(0.18f,0.12f,i==3?0.03f:0.1f),i%2==0?silver:gold);
            WorldText("Tag_"+names[i],absorbed,p+Vector3.up*0.15f,names[i],1.2f,C("#E6F7FF"));
        }
        // Mode 3 object: orbit ring
        var orbit=new GameObject("Mode3_OrbitRing").transform;orbit.SetParent(root.transform,false);orbit.localPosition=new Vector3(0,0.85f,0);
        for(int i=0;i<24;i++){float a=i*Mathf.PI*2/24;Prim(PrimitiveType.Sphere,"OrbitDot_"+i,orbit,new Vector3(Mathf.Cos(a)*0.65f,0,Mathf.Sin(a)*0.65f),Vector3.one*0.025f,ring);}

        var focus=Prim(PrimitiveType.Cylinder,"FocusRing",root.transform,new Vector3(0,0.005f,0),new Vector3(2.2f,0.004f,1.25f),ring);
        WorldText("Label_Title",root.transform,new Vector3(0,0.05f,-0.68f),"iPHONE  ·  2007",2.2f,C("#E6F7FF")).transform.localRotation=Quaternion.Euler(60,0,0);
        WorldText("Label_Placeholder",root.transform,new Vector3(0,-0.3f,-0.79f),"PLACEHOLDER MODEL — not an Apple asset",1.1f,C("#FFC478"));

        exhibit.title="iPhone (1st generation)";
        exhibit.meta="Apple Inc.  ·  Announced 9 Jan 2007  ·  Released 29 Jun 2007 (US)";
        exhibit.body="The first iPhone combined a phone, an iPod and an internet device in one slab of glass. "+
            "Instead of a plastic keypad it used a 3.5-inch multi-touch screen, so the controls became software that could change after you bought it.\n\n"+
            "When the App Store opened in July 2008, the phone began absorbing other objects: the camera, the paper map, the music player, the flashlight and eventually the wallet. "+
            "Most people now carry this one object everywhere, and much of daily life — directions, payment, photos, conversation — passes through it.";
        exhibit.themeLink="This is where our route begins: technology leaving the desk and moving into the pocket, onto the body, and finally off the planet. "+
            "The cheap, tiny chips, batteries and sensors made for phones later flew in space (see mode 3).";
        exhibit.credit="Model: placeholder built from Unity primitives by the student, 5x scale. Not an official Apple model.";
        exhibit.model=model;exhibit.screen=screen.GetComponent<Renderer>();exhibit.focusHighlight=focus;exhibit.interactRange=3f;
        exhibit.modes=new[]{
            new MuseumExhibit.Mode{label="2007 · Glass replaces keys",caption="A single touchscreen replaced the keypad. Buttons became pictures that software could redraw at any time.",screenColor=C("#41E9EB"),spinSpeed=14},
            new MuseumExhibit.Mode{label="2008 · Apps absorb objects",caption="With the App Store, one device swallowed the camera, map, music player and wallet now floating around it.",screenColor=C("#AB73FF"),showObject=absorbed.gameObject,spinSpeed=30},
            new MuseumExhibit.Mode{label="2013 · A phone in orbit",caption="NASA's PhoneSat missions (2013) flew off-the-shelf smartphones as the computers of tiny satellites. Pocket tech became space hardware.",screenColor=C("#FF7FC8"),showObject=orbit.gameObject,lift=0.35f,spinSpeed=45},
        };
        absorbed.gameObject.SetActive(false);orbit.gameObject.SetActive(false);focus.SetActive(false);

        // ----- Shared UI (kept on top of ColorNoise so text stays readable) -----
        var font=TMP_Settings.defaultFontAsset;
        System.Func<string,Transform,GameObject> UI=(n,p)=>{var g=new GameObject(n,typeof(RectTransform));g.transform.SetParent(p,false);Undo.RegisterCreatedObjectUndo(g,n);return g;};
        System.Action<GameObject,Vector2,Vector2,Vector2,Vector2> Rect=(g,min,max,omin,omax)=>{var r=(RectTransform)g.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=omin;r.offsetMax=omax;};
        System.Func<GameObject,Color,Image> Img=(g,c)=>{var i=g.AddComponent<Image>();i.color=c;return i;};
        System.Func<string,Transform,float,Color,TextMeshProUGUI> Txt=(n,p,s,c)=>{var g=UI(n,p);var t=g.AddComponent<TextMeshProUGUI>();t.font=font;t.fontSize=s;t.color=c;t.raycastTarget=false;t.textWrappingMode=TextWrappingModes.Normal;Rect(g,Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero);return t;};

        var prompt=UI("ExhibitPrompt",canvas.transform);Rect(prompt,new Vector2(0.35f,0.2f),new Vector2(0.65f,0.27f),Vector2.zero,Vector2.zero);
        Img(prompt,new Color(0.02f,0.03f,0.07f,0.9f)).raycastTarget=false;
        var promptText=Txt("PromptText",prompt.transform,24,C("#E6F7FF"));promptText.alignment=TextAlignmentOptions.Center;promptText.text="[E]  INSPECT";
        prompt.SetActive(false);

        var panel=UI("ExhibitPanel",canvas.transform);Rect(panel,new Vector2(0.52f,0.1f),new Vector2(0.97f,0.95f),Vector2.zero,Vector2.zero);
        Img(panel,new Color(0.02f,0.025f,0.06f,0.94f));
        var accent=UI("AccentBar",panel.transform);Rect(accent,new Vector2(0,0),new Vector2(0,1),Vector2.zero,new Vector2(6,0));Img(accent,C("#41E9EB")).raycastTarget=false;
        var content=UI("Content",panel.transform);Rect(content,Vector2.zero,Vector2.one,new Vector2(30,20),new Vector2(-24,-20));
        var title=Txt("Title",content.transform,34,Color.white);Rect(title.gameObject,new Vector2(0,0.9f),Vector2.one,Vector2.zero,Vector2.zero);title.fontStyle=FontStyles.Bold;
        var meta=Txt("Meta",content.transform,16,C("#FFC478"));Rect(meta.gameObject,new Vector2(0,0.85f),new Vector2(1,0.9f),Vector2.zero,Vector2.zero);
        var body=Txt("Body",content.transform,19,C("#D8E6F2"));Rect(body.gameObject,new Vector2(0,0.5f),new Vector2(1,0.84f),Vector2.zero,Vector2.zero);body.alignment=TextAlignmentOptions.TopLeft;body.enableAutoSizing=true;body.fontSizeMin=13;body.fontSizeMax=19;
        var theme=Txt("ThemeLink",content.transform,17,C("#C9B3FF"));Rect(theme.gameObject,new Vector2(0,0.36f),new Vector2(1,0.49f),Vector2.zero,Vector2.zero);theme.alignment=TextAlignmentOptions.TopLeft;theme.enableAutoSizing=true;theme.fontSizeMin=12;theme.fontSizeMax=17;
        var buttons=new Button[3];
        for(int i=0;i<3;i++){
            var b=UI("ModeButton_"+(i+1),content.transform);Rect(b,new Vector2(i/3f,0.27f),new Vector2((i+1)/3f,0.34f),new Vector2(i==0?0:4,0),new Vector2(i==2?0:-4,0));
            Img(b,new Color(0.16f,0.12f,0.3f,0.95f));buttons[i]=b.AddComponent<Button>();
            var bt=Txt("Label",b.transform,14,Color.white);bt.alignment=TextAlignmentOptions.Center;bt.enableAutoSizing=true;bt.fontSizeMin=10;bt.fontSizeMax=15;bt.margin=new Vector4(6,2,6,2);
        }
        var caption=Txt("ModeCaption",content.transform,18,C("#FFFFFF"));Rect(caption.gameObject,new Vector2(0,0.12f),new Vector2(1,0.26f),Vector2.zero,Vector2.zero);caption.alignment=TextAlignmentOptions.TopLeft;caption.enableAutoSizing=true;caption.fontSizeMin=12;caption.fontSizeMax=18;
        var credit=Txt("Credit",content.transform,12,C("#8592A6"));Rect(credit.gameObject,new Vector2(0,0.06f),new Vector2(1,0.12f),Vector2.zero,Vector2.zero);
        var close=UI("CloseButton",content.transform);Rect(close,new Vector2(0.6f,0),new Vector2(1,0.055f),Vector2.zero,Vector2.zero);Img(close,C("#FF7FC8"));
        var closeBtn=close.AddComponent<Button>();var ct=Txt("Label",close.transform,15,C("#10101A"));ct.alignment=TextAlignmentOptions.Center;ct.text="CLOSE  [E / ESC]";ct.fontStyle=FontStyles.Bold;
        var hint=Txt("KeysHint",content.transform,13,C("#8592A6"));Rect(hint.gameObject,new Vector2(0,0),new Vector2(0.58f,0.055f),Vector2.zero,Vector2.zero);hint.alignment=TextAlignmentOptions.Left;hint.text="Press 1 / 2 / 3 or click a stage";
        panel.SetActive(false);

        var interactor=Undo.AddComponent<MuseumExhibitInteractor>(visitor.gameObject);
        interactor.visitor=visitor;interactor.promptRoot=prompt;interactor.promptText=promptText;interactor.panelRoot=panel;
        interactor.titleText=title;interactor.metaText=meta;interactor.bodyText=body;interactor.themeText=theme;interactor.captionText=caption;interactor.creditText=credit;
        interactor.modeButtons=buttons;interactor.closeButton=closeBtn;
        var src=Undo.AddComponent<AudioSource>(visitor.gameObject);src.playOnAwake=false;interactor.audioSource=src;

        // Only one AudioListener: keep the one on Main Camera (follows the visitor).
        var display=GameObject.Find("DisplayCamera");
        if(display!=null && display.TryGetComponent<AudioListener>(out var l) && Camera.main!=null && Camera.main.GetComponent<AudioListener>()!=null){Undo.RecordObject(l,"Disable extra listener");l.enabled=false;}

        Undo.CollapseUndoOperations(undo);Selection.activeGameObject=root;
        EditorSceneManager.MarkSceneDirty(scene);AssetDatabase.SaveAssets();EditorSceneManager.SaveScene(scene);
        Debug.Log("iPhone sample exhibit saved on plinth 6 (placeholder model).");
    }

    // Batchmode entry: open scene, build, quit.
    public static void BuildBatch(){EditorSceneManager.OpenScene("Assets/Scenes/MidtermMuseum.unity");Build();}
}
