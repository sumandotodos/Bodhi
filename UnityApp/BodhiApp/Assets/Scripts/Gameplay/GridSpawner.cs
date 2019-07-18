using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridSpawner : MonoBehaviour
{
    //public GameObject CellPrefab;
    public PrefabSelector prefabSelector;
    public Transform CellsParent;
    public int Rows = 16;
    public int Columns = 12;
    public int NumberOfBichos = 5;
    public float unitsPerPixel = 100.0f;
    public float nPixels = 64.0f;

    System.Random random = new System.Random();
    int TotalFaded = 0;
    private Cell[,] cells;
    int touchScore;
    Vector2 touchScoreAveragePosition;

    public void BuildGrid(int _Columns, int _Rows, int _Bichos)
    {
        Columns = _Columns;
        Rows = _Rows;
        NumberOfBichos = _Bichos;

        AddInitialCells();
        AddBichos();
        AddNeighbors();
    }

    private void AddInitialCells()
    {
        cells = new Cell[Rows, Columns];
        float totalHalfWidth = ((nPixels / unitsPerPixel) * (Columns - 1.0f)) / 2.0f;
        float totalHalfHeight = ((nPixels / unitsPerPixel) * (Rows - 1.0f)) / 2.0f;
        GameObject CellPrefab = prefabSelector.GetPrebafByPlayerPrefs();
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                GameObject newGO = (GameObject)Instantiate(CellPrefab);
                newGO.transform.SetParent(CellsParent);
                newGO.transform.localScale = Vector3.one;
                newGO.transform.localPosition =
                 new Vector3((nPixels / unitsPerPixel) * j - totalHalfWidth,
                             (nPixels / unitsPerPixel) * i - totalHalfHeight,
                             0);
                cells[i, j] = newGO.GetComponent<Cell>();
                cells[i, j].Start();
            }
        }
    }

    private void AddBichos()
    {
        NumberOfBichos = Mathf.Min(NumberOfBichos, Columns * Rows);
        for (int k = 0; k < NumberOfBichos; ++k)
        {
            int i, j = 0;
            do
            {
                j = Random.Range(0, Columns);
                i = Random.Range(0, Rows);
            } while (cells[i, j].getBicho());
            cells[i, j].setBicho();
        }
    }

    private void AddNeighbors()
    {
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                if (cells[i, j].getBicho())
                {
                    incNeighbor(i - 1, j);
                    incNeighbor(i - 1, j - 1);
                    incNeighbor(i, j - 1);
                    incNeighbor(i + 1, j - 1);
                    incNeighbor(i + 1, j);
                    incNeighbor(i + 1, j + 1);
                    incNeighbor(i, j + 1);
                    incNeighbor(i - 1, j + 1);
                }
            }
        }
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


    public void Touch(int i, int j, Vector2 screenCoords)
    {
        if (cells[i, j].getBicho())
        {
            Raycaster.GetSingleton().SetActive(false);
            ++TotalFaded;
            GameController.GetSingleton().BichoFound(screenCoords);
            GameController.GetSingleton().ReportClearedCells(TotalFaded);
            //cells[i, j].clearBicho();
            FadeCellAt(i, j, 1);
            return;
        }
        touchScore = 0;
        touchScoreAveragePosition = Vector2.zero;
        AddPropagator(i, j, 1);
        if (touchScore > 0)
        {
            touchScoreAveragePosition *= (1.0f / ((float)touchScore));
        }
        GameController.GetSingleton().TouchScore(touchScore, 
            GridToLocalCoordinates((float)j,
            (float)i));
    }


    public void AddPropagator(int i, int j, int delay)
    {
        if (j < 0) return;
        if (i < 0) return;
        if (j > Columns - 1) return;
        if (i > Rows - 1) return;

        if (cells[i, j].isTerminal())
        {
            //++touchScore;
            //touchScoreAveragePosition += new Vector2(i, j);
            //FadeCellAt(i, j, delay);
            return;
        }
        else if (cells[i,j].isFrontier())
        {
            ++touchScore;
            ++TotalFaded;
            GameController.GetSingleton().ReportClearedCells(TotalFaded);
            FadeCellAt(i, j, delay);
            return;
        }
        //Debug.Log("<color=red>" + j + "," + i + "</color>");
        ++TotalFaded;
        GameController.GetSingleton().ReportClearedCells(TotalFaded);
        ++touchScore;
        FadeCellAt(i, j, delay);
        AddPropagator(i + 1, j, delay+1);
        AddPropagator(i - 1, j, delay+1);
        AddPropagator(i, j + 1, delay+1);
        AddPropagator(i, j - 1, delay+1);
    }


    public Vector3 GridToLocalCoordinates(float column, float row)
    {
        float totalHalfWidth = ((nPixels / unitsPerPixel) * (Columns - 1.0f)) / 2.0f;
        float totalHalfHeight = ((nPixels / unitsPerPixel) * (Rows - 1.0f)) / 2.0f;
        return new Vector3((nPixels / unitsPerPixel) * column - totalHalfWidth,
                             (nPixels / unitsPerPixel) * row - totalHalfHeight,
                             0);
    }
}
