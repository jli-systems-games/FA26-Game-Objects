
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SimpleDrawing : MonoBehaviour
{
    public Camera drawingCamera;

    public float brushSize = 0.05f;

    private Color brushColor = Color.black;

    private LineRenderer currentLine;

    private List<GameObject> allStrokes = new List<GameObject>();


    void Update()
    {
        // Start drawing
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        // Continue drawing
        if (Input.GetMouseButton(0) && currentLine != null)
        {
            Draw();
        }

        // Stop drawing
        if (Input.GetMouseButtonUp(0))
        {
            currentLine = null;
        }
    }

    void CreateNewLine()
    {
        GameObject newStroke = new GameObject("Stroke");

        currentLine = newStroke.AddComponent<LineRenderer>();

        currentLine.positionCount = 0;

        currentLine.startWidth = brushSize;
        currentLine.endWidth = brushSize;

        currentLine.material = new Material(
            Shader.Find("Sprites/Default")
        );

        currentLine.startColor = brushColor;
        currentLine.endColor = brushColor;

        currentLine.useWorldSpace = true;

        currentLine.numCapVertices = 5;
        currentLine.numCornerVertices = 5;

        currentLine.sortingOrder = 10;

        allStrokes.Add(newStroke);
    }

    void Draw()
    {
        Ray ray = drawingCamera.ScreenPointToRay(
            Input.mousePosition
        );

        Plane drawingPlane = new Plane(
            Vector3.forward,
            Vector3.zero
        );

        float distance;

        if (drawingPlane.Raycast(ray, out distance))
        {
            Vector3 mousePosition = ray.GetPoint(distance);

            if (currentLine.positionCount == 0 ||
                Vector3.Distance(
                    currentLine.GetPosition(
                        currentLine.positionCount - 1
                    ),
                    mousePosition
                ) > 0.02f)
            {
                currentLine.positionCount++;

                currentLine.SetPosition(
                    currentLine.positionCount - 1,
                    mousePosition
                );
            }
        }
    }





    // CLEAR/Reload Scene

    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(4);
    }
}
