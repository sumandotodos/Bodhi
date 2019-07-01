using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridSpawner : MonoBehaviour
{
    public GameObject CellPrefab;
    public int Rows = 16;
    public int Columns = 12;
    public float Prob = 0.5f;
    System.Random random = new System.Random();

    public float unitsPerPixel = 100.0f;
    public float nPixels = 64.0f;

    private Cell[,] cells;

    // Start is called before the first frame update
    void Start()
    {
        BuildGrid();
    }

    private bool cellWillHavePrize()
    {
        float r = ((float)random.Next(0, 10000))/10000.0f;
        r = ((float)random.Next(0, 10000)) / 10000.0f;
        r = ((float)random.Next(0, 10000)) / 10000.0f;
        Debug.Log("Rrrandom: " + r);
        return r < Prob;
    }

    private void BuildGrid()
    {
        cells = new Cell[Rows, Columns];
        float totalHalfWidth = ((nPixels / unitsPerPixel) * Columns) / 2.0f;
        float totalHalfHeight = ((nPixels / unitsPerPixel) * Rows) / 2.0f;
        int totalBicho = 0;
        for (int i = 0; i < Rows; ++i)
        {
            for(int j = 0; j < Columns; ++j)
            {
                GameObject newGO = (GameObject)Instantiate(CellPrefab);
                newGO.transform.position =
                 new Vector3((nPixels / unitsPerPixel) * j - totalHalfWidth, 
                             (nPixels / unitsPerPixel) * i - totalHalfHeight, 
                             0);
                cells[i, j] = newGO.GetComponent<Cell>();
                cells[i, j].Start();
                if(cellWillHavePrize())
                {
                    cells[i, j].setBicho();
                    ++totalBicho;
                }
            }
        }
        for(int i = 0; i < Rows; ++i)
        {
            for(int j = 0; j < Columns; ++j)
            {
                if(cells[i,j].getBicho())
                {
                    incNeighbor(i - 1, j);
                    incNeighbor(i - 1, j-1);
                    incNeighbor(i, j-1);
                    incNeighbor(i + 1, j-1);
                    incNeighbor(i + 1, j);
                    incNeighbor(i + 1, j + 1);
                    incNeighbor(i, j + 1);
                    incNeighbor(i - 1, j + 1);
                }
            }
        }
        Debug.Log("<color=blue>Total bicho: " + totalBicho + "</color>");
    }

    private void incNeighbor(int i, int j)
    {
        if (i < 0) return;
        if (i > (Rows - 1)) return;
        if (j < 0) return;
        if (j > (Columns - 1)) return;
        cells[i, j].incNeighbors();
    }

    public void FadeCellAt(int i, int j)
    {
        FadeCellAt(i, j, 0);
    }

    public void FadeCellAt(int i, int j, int delay)
    {
        if (j < 0) return;
        if (i < 0) return;
        if (j > Columns - 1) return;
        if (i > Rows - 1) return;
        cells[i, j].touch(delay);
    }

    public void Touch(int i, int j)
    {
        if (cells[i, j].getBicho())
        {
            ContentsController.GetSingleton().BichoFound();
            Raycaster.GetSingleton().SetActive(false);
        }
        AddPropagator(i, j, 1);
    }

    public void AddPropagator(int i, int j, int delay)
    {
        if (j < 0) return;
        if (i < 0) return;
        if (j > Columns - 1) return;
        if (i > Rows - 1) return;
        if (cells[i, j].endPropagation())
        {
            FadeCellAt(i, j, delay);
            return;
        }
        //Debug.Log("<color=red>" + j + "," + i + "</color>");
        FadeCellAt(i, j, delay);
        AddPropagator(i + 1, j, delay+1);
        AddPropagator(i - 1, j, delay+1);
        AddPropagator(i, j + 1, delay+1);
        AddPropagator(i, j - 1, delay+1);
    }
}
