using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public GridSpawner gridSpawner_A;

    float cellDimension;
    int columns;
    int rows;
    // Start is called before the first frame update
    void Start()
    {
        if(gridSpawner_A == null)
        {
            gridSpawner_A = FindObjectOfType<GridSpawner>();
        }

        cellDimension = (gridSpawner_A.nPixels) / (gridSpawner_A.unitsPerPixel);
        columns = gridSpawner_A.Columns;
        rows = gridSpawner_A.Rows;
    }

    /*
     * int j = Mathf.FloorToInt(hit.point.x / cellDimension) + columns / 2;
                int i = Mathf.FloorToInt(hit.point.y / cellDimension) + rows / 2;
                Debug.Log("Uncle Ray Ray hit someone @ " + j + ", " + i);   
     */
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitInfo = Physics.RaycastAll(ray.origin, ray.direction);
            foreach (RaycastHit h in hitInfo)
            {
                // if (h.collider.name == "UncleRayTarget")
                // {
                int j = Mathf.FloorToInt((h.point.x-(cellDimension/2.0f)) / cellDimension) + columns / 2 + 1;
                int i = Mathf.FloorToInt((h.point.y - (cellDimension / 2.0f)) / cellDimension) + rows / 2 + 1;
                Debug.Log("Uncle Ray Ray hit someone @ " + j + ", " + i);
                // }
                gridSpawner_A.Touch(i, j);
            }
        }
    }
}
