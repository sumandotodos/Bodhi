using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterRaycaster : MonoBehaviour
{
    public string TagFilter = "";
    bool isActive = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitInfo = Physics.RaycastAll(ray.origin, ray.direction);
            foreach (RaycastHit h in hitInfo)
            {
                TouchableComponent touchedComponent = h.collider.gameObject.GetComponentInParent<TouchableComponent>();
                if (touchedComponent != null)
                {
                    touchedComponent.Touch();
                }
            }
        }
    }
}
