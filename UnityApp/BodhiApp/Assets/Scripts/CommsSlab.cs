using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommsSlab : Slab, OptionIndexReceiver
{
    public Color ChatColor;
    public Color AudioColor;
    public Color VideoColor;

    public Image ChatImage;
    public Image AudioImage;
    public Image VideoImage;

    int Activation = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }
     
    public void ReceiveIndex(int index)
    {
        Activation = index;
    }
}
