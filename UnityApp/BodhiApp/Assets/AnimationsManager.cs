using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.Play("SpringUpOK");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
