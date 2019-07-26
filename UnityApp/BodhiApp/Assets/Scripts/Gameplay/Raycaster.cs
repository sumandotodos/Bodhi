using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public GridSpawner gridSpawner_A;

    static Raycaster raycaster = null;

    bool isActive = true;

    float cellDimension;
    int columns;
    int rows;
    // Start is called before the first frame update

    private void Awake()
    {
        if(raycaster == null)
        {
            raycaster = this;
        }
    }

    void Start()
    {
        if(gridSpawner_A == null)
        {
            gridSpawner_A = FindObjectOfType<GridSpawner>();
        }

        cellDimension = (gridSpawner_A.nPixels) / (gridSpawner_A.unitsPerPixel);
        columns = gridSpawner_A.Columns;
        rows = gridSpawner_A.Rows;
        isActive = true;
    }

    public static Raycaster GetSingleton()
    {
        return raycaster;
    }

    public void SetActive(bool active)
    {
        isActive = active;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitInfo = Physics.RaycastAll(ray.origin, ray.direction);
            foreach (RaycastHit h in hitInfo)
            {
                // if (h.collider.name == "UncleRayTarget")
                // {
                int j = Mathf.FloorToInt((h.point.x-(cellDimension)) / cellDimension) + (columns) / 2 + 1;
                int i = Mathf.FloorToInt((h.point.y - (cellDimension)) / cellDimension) + (rows) / 2 + 1;

                // }
                gridSpawner_A.Touch(i, j, Input.mousePosition);
            }
        }
    }
}
