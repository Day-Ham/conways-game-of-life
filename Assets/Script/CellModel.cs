using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellModel : MonoBehaviour
{
    public int resolution = 80;
    public Color brushColor = Color.white;


    [SerializeField] GameObject prefab;
    //Just Organizes the Cell clones to avoid clutter in the heirachy
    [SerializeField] GameObject cellContainer;
    Cell[,] grid;




    private void Awake()
    {
        grid = new Cell[resolution, resolution];
    }

    public void GenerateNewGrid()
    {
        grid = new Cell[resolution, resolution];
    }
    public void GenerateField()
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {

                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                cell.transform.parent = cell.transform;
                
                grid[x, y] = cell;
                grid[x, y].SetAlive(RandomAliveCell());


            }
        }
    }

    public void CountNeighbours()
    {
        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                int neighbours = 0;
                #region PerpendicularCheck
                //North
                if (i + 1 < resolution)
                {

                    if (grid[j, i + 1].isAlive)
                    {
                        neighbours++;
                    }
                }
                //East
                if (j + 1 < resolution)
                {
                    if (grid[j + 1, i].isAlive)
                    {
                        neighbours++;
                    }
                }
                //South
                if (i - 1 >= 0)
                {
                    if (grid[j, i - 1].isAlive)
                    {
                        neighbours++;
                    }
                }

                //West
                if (j - 1 >= 0)
                {
                    if (grid[j - 1, i].isAlive)
                    {
                        neighbours++;
                    }
                }
                #endregion
                #region DiagonalCheck
                //North East
                if (i + 1 < resolution && j + 1 < resolution)
                {
                    if (grid[j + 1, i + 1].isAlive)
                    {
                        neighbours++;
                    }
                }
                //South East
                if (i - 1 >= 0 && j + 1 < resolution)
                {
                    if (grid[j + 1, i - 1].isAlive)
                    {
                        neighbours++;
                    }
                }
                //South West
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (grid[j - 1, i - 1].isAlive)
                    {
                        neighbours++;
                    }
                }

                //North West
                if (j - 1 >= 0 && i + 1 < resolution)
                {
                    if (grid[j - 1, i + 1].isAlive)
                    {
                        neighbours++;
                    }
                }
                #endregion


                grid[j, i].numNeighbours = neighbours;


            }
        }
    }
    public void PopulationControl()
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                if (grid[x, y].isAlive)
                {
                    if (grid[x, y].numNeighbours != 2 && grid[x, y].numNeighbours != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                }
                else
                {
                    if (grid[x, y].numNeighbours == 3)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }


    bool RandomAliveCell()
    {
        int rand = Random.Range(0, 100);
        if (rand > 75)
        {
            return true;
        }
        return false;

    }
    public void EffectWholeField(bool isFilled)
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                grid[x, y].SetAlive(isFilled);
            }
        }
    }
    public void RandomizeField()
    {
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                grid[x, y].SetAlive(RandomAliveCell());
            }
        }
    }

    public void ChangeCellColor(Cell cell)
    {
        cell.ChangeColor(brushColor);
    }

    public void ChangeLife(bool value, Cell cell)
    {
        cell.SetAlive(value);
        ChangeCellColor(cell);
    }

}
