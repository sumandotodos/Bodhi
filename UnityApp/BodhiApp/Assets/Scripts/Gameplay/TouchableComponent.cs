using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableComponent : MonoBehaviour
{
    public MonoBehaviour TargetScript;
    public string MethodName;


    public void Touch()
    {
        TargetScript.Invoke(MethodName, 0.0f);
    }
}
