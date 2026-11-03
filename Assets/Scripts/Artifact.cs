using UnityEngine;

public class Artifact : MonoBehaviour
{
    [Header("Display")]
    public string artifactName = "Unnamed Artifact";

    [TextArea(5, 15)]
    public string description = "Description goes here.";
}