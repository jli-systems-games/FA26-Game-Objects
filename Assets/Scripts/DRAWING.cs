using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class DRAWING : MonoBehaviour


{

    //variables 

    public Camera m_camera;
    public GameObject brushRed;
    public GameObject brushBlue;
    public GameObject brushGreen;
    public GameObject brushWhite;

    public int brushCount;


    public LineRenderer currentLineRenderer;


    Vector2 lastPos;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Drawing();
    }


    void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, pointPos);
    }



// POINT TO MOUSE POSITION
    void PointToMousePosition() 
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            if (lastPos != mousePos)
            {
                AddPoint(mousePos);
                lastPos = mousePos;
            }
    }

    // DRAWING

    void Drawing()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(brushCount == 0)
            {
                CreateBrushRed();
            }
            if(brushCount == 1)
            {
                CreateBrushBlue();  
            }
            if(brushCount == 2)
            {   
                CreateBrushGreen();
            }
                
            if(brushCount == 3)
            {
                CreateBrushWhite();
            }
            
            else if(Input.GetKey(KeyCode.Mouse0))
            {
                PointToMousePosition();
            }

                else
                {
                    currentLineRenderer = null;
                }

        }

    }



// BRUSHES & COLOR SELECTION
    public void redButton()
    {
        brushCount = 0;
    }

    public void blueButton()
    {
        brushCount = 1;
    }

    public void greenButton()
    {
        brushCount = 2;
    }

    public void whiteButton()
    {
        brushCount = 3;
    }

//create brush

    void CreateBrushRed()
    {
        GameObject brushInstance = Instantiate(brushRed);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void CreateBrushBlue()
    {
        GameObject brushInstance = Instantiate(brushBlue);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void CreateBrushGreen()
    {
        GameObject brushInstance = Instantiate(brushGreen);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void CreateBrushWhite()
    {
        GameObject brushInstance = Instantiate(brushWhite);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    } 


}
