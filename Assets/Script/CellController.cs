using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CellController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask cellLayer;
    [SerializeField] CellModel model;
    [SerializeField] TMP_InputField inputFieldResolution;
    [SerializeField] Text speedPrompt;
    bool isSimulating=false;
    int speedSetting = 0;
    float cooldown =0.5f;
    float currentTime=0f;


    private void Awake()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        currentTime -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            ChangeCellState(true);

        }
        else if (Input.GetMouseButtonDown(1))
        {
            ChangeCellState(false);
        }

        if (isSimulating && currentTime <= 0)
        {
            model.CountNeighbours();
            model.PopulationControl();
            currentTime = cooldown;
        }
    }


    
    public void Simulate()
    {
        isSimulating = !isSimulating;
    }
    public void GeneratePlot()
    {
        
        model.GenerateNewGrid();
        model.GenerateField();
        cam.orthographicSize =(model.resolution / 2);
        cam.transform.position = new Vector3(model.resolution / 2, model.resolution / 2,-10);
    }
    public void BucketTool(bool FillorEmpty)
    {
        model.EffectWholeField(FillorEmpty);
    }


    public void RandomizeField()
    {
        model.RandomizeField();
    }

    //Edit Settings
    public void ChangeTickSpeed()
    {
        speedSetting++;
        speedSetting %= 3;
        switch (speedSetting)
        {
            case 0:
                cooldown = 0.5f;
                speedPrompt.text=">";
                break;
            case 1:
                cooldown = 0.25f;
                speedPrompt.text = ">>";
                break;
            case 2:
                cooldown = 0;
                speedPrompt.text = ">>>";
                break;

        }

    }

    public void ChangeResolution()
    {
        int.TryParse(inputFieldResolution.text, out model.resolution);
        if(model.resolution > 128)
        {
            model.resolution = 128;
        }
    }

    public void ChangeCellState(bool value)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, cellLayer);

        if (hit.collider != null)
        {
            Cell cell = hit.collider.GetComponent<Cell>();
            model.ChangeLife(value, cell);
        }
    }

    public void ChangeBrushColor(string colorName)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(colorName, out color))
        {
            model.brushColor = color;
        }
    }

   


}
