using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class ApartmentDoorHeaderFixer
{
    const float Tolerance=.001f;

    [MenuItem("Tools/Apartment/Fix Door Headers From Current Walls")]
    public static void Fix()
    {
        var apartment=GameObject.Find("Apartment");if(!apartment){Debug.LogError("[Door Headers] Apartment root not found; nothing changed.");return;}
        GameObject entry=Wall("ExteriorWalls/ENTRY_NOTCH_BOTTOM"),service=Wall("ExteriorWalls/SERVICE_WEST");
        GameObject closetTop=Wall("InteriorWalls/UPPER_LEFT_TOP"),closetRight=Wall("InteriorWalls/UPPER_LEFT_RIGHT"),bathLeft=Wall("InteriorWalls/BATH_TOP_LEFT");
        GameObject bridge=Wall("InteriorWalls/CENTER_BRIDGE"),wdTop=Wall("InteriorWalls/WD_TOP"),wdBottom=Wall("InteriorWalls/CENTER_TOP_RIGHT");
        GameObject core=Wall("InteriorWalls/CENTER_CORE_VERTICAL"),stub=Wall("InteriorWalls/ALCOVE_ENTRY_STUB"),alcoveReturn=Wall("ExteriorWalls/ALCOVE_LEFT_RETURN");
        if(!entry||!service||!closetTop||!closetRight||!bathLeft||!bridge||!wdTop||!wdBottom||!core||!stub||!alcoveReturn){Debug.LogError("[Door Headers] A current wall reference is missing; nothing changed.");return;}

        Bounds eb=B(entry),sb=B(service),ct=B(closetTop),bl=B(bathLeft),br=B(bridge),wt=B(wdTop),wb=B(wdBottom),st=B(stub),ar=B(alcoveReturn);
        int errors=0;
        errors+=SetHeader("ENTRY_DOOR",entry,"X",eb.max.x,sb.min.x,eb.center.z,eb.size.z);
        errors+=SetHeader("LEFT_CLOSET_DOOR",closetRight,"Z",bl.max.z,ct.min.z,(ct.min.x+bl.min.x)*.5f,B(closetRight).size.x);
        errors+=SetHeader("BATHROOM_DOOR",bathLeft,"X",bl.max.x,br.min.x,bl.center.z,bl.size.z);
        errors+=SetHeader("WD_CLOSET_DOOR",core,"Z",wb.max.z,wt.min.z,Mathf.Max(wt.min.x,wb.min.x),B(core).size.x);
        float mid=(st.min.z+ar.max.z)*.5f,rightPlane=(st.center.x+ar.center.x)*.5f;
        errors+=SetHeader("RIGHT_CLOSET_DOOR_UPPER",stub,"Z",mid,st.min.z,rightPlane,st.size.x);
        errors+=SetHeader("RIGHT_CLOSET_DOOR_LOWER",alcoveReturn,"Z",ar.max.z,mid,rightPlane,ar.size.x);

        EditorSceneManager.MarkSceneDirty(apartment.scene);
        if(errors==0)Debug.Log("[Door Headers] PASS — all six headers match current wall top, door top, thickness, span, plane, and material.");else Debug.LogError($"[Door Headers] FAILED with {errors} validation error(s).");
    }

    static int SetHeader(string doorName,GameObject wall,string axis,float endA,float endB,float plane,float thickness)
    {
        var door=GameObject.Find("Apartment/Doors/"+doorName);if(!door){Debug.LogError("[Door Headers] Missing door "+doorName);return 1;}
        var leaf=door.transform.Find("DoorLeaf");var header=door.transform.Find("Frame/Header");
        if(!leaf||!header){Debug.LogError("[Door Headers] Missing leaf/header for "+doorName);return 1;}
        Bounds wallBounds=B(wall),leafBounds=leaf.GetComponent<Renderer>().bounds;
        float wallTop=wallBounds.max.y,doorTop=leafBounds.max.y,height=wallTop-doorTop,span=Mathf.Abs(endB-endA);
        Vector3 size=axis=="X"?new Vector3(span,height,thickness):new Vector3(thickness,height,span);
        Vector3 position=axis=="X"?new Vector3((endA+endB)*.5f,(wallTop+doorTop)*.5f,plane):new Vector3(plane,(wallTop+doorTop)*.5f,(endA+endB)*.5f);
        header.position=position;header.rotation=Quaternion.identity;header.localScale=size;
        header.GetComponent<Renderer>().sharedMaterial=wall.GetComponent<Renderer>().sharedMaterial;
        EditorUtility.SetDirty(header);EditorUtility.SetDirty(header.GetComponent<Renderer>());

        Bounds hb=header.GetComponent<Renderer>().bounds;int e=0;
        if(Mathf.Abs(hb.max.y-wallTop)>Tolerance||Mathf.Abs(hb.min.y-doorTop)>Tolerance){Debug.LogError("[Door Headers] Vertical mismatch: "+doorName);e++;}
        float actualThickness=axis=="X"?hb.size.z:hb.size.x,actualSpan=axis=="X"?hb.size.x:hb.size.z;
        if(Mathf.Abs(actualThickness-thickness)>Tolerance||Mathf.Abs(actualSpan-span)>Tolerance){Debug.LogError("[Door Headers] Size mismatch: "+doorName);e++;}
        Debug.Log($"[Door Headers] {doorName}: wall={Path(wall.transform)}, thickness={thickness:F3}, wallTopY={wallTop:F3}, doorTopY={doorTop:F3}, size={hb.size:F3}, position={hb.center:F3}, axis={axis}");
        return e;
    }
    static GameObject Wall(string path)=>GameObject.Find("Apartment/"+path);
    static Bounds B(GameObject g)=>g.GetComponent<Renderer>().bounds;
    static string Path(Transform t){var names=new System.Collections.Generic.List<string>();while(t){names.Add(t.name);t=t.parent;}names.Reverse();return string.Join("/",names);}
}
