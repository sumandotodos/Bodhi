using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    protected static Singleton instance = null;

    protected void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("<color=red>Error: more than one instance of singleton class</color>");
            Destroy(this.gameObject);
        }
    }

    public static Singleton GetSingleton()
    {
        return instance;
    }

}
