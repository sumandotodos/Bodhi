using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRaycaster : MonoBehaviour
{
    static PlanetRaycaster raycaster = null;

    bool isActive = true;

    // Start is called before the first frame update

    private void Awake()
    {
        if (raycaster == null)
        {
            raycaster = this;
        }
    }

    void Start()
    {
    
    }

    public static PlanetRaycaster GetSingleton()
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
                Planet touchedPlanet = h.collider.gameObject.GetComponentInParent<Planet>();
                if(touchedPlanet != null)
                {
                    PlanetGameController.GetSingleton().ClickOnPlanet(touchedPlanet);
                }
            }
        }
    }
}
