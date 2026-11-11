using UnityEngine;

public class Artifact : MonoBehaviour
{
    [Header("Display")]
    public string artifactName = "Unnamed Artifact";

    [TextArea(5, 15)]
    public string description = "Description goes here.";

    [Header("Highlight")]
    public Material normalMaterial;
    public Material highlightMaterial;

    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetHighlight(bool on)
    {
        if (rend == null) return;
        if (normalMaterial == null || highlightMaterial == null) return;

        rend.material = on ? highlightMaterial : normalMaterial;
    }
}