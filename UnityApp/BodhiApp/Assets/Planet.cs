using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    public bool OuterRing;
    public bool MiddleRing;
    public bool InnerRing;

    public GameObject OuterRingObject;
    public GameObject MiddleRingObject;
    public GameObject InnerRingObject;

    // Start is called before the first frame update
    void Start()
    {
        OuterRingObject.SetActive(OuterRing);
        MiddleRingObject.SetActive(MiddleRing);
        InnerRingObject.SetActive(InnerRing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
