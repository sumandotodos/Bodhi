using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reportor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Report: " + this.transform.localPosition); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
