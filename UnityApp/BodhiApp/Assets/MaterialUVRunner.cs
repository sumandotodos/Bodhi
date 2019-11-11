using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialUVRunner : MonoBehaviour
{
    public Renderer rend;
    List<Material> matList;
    // Start is called before the first frame update
    void Start()
    {
        Offset = Vector2.zero;
        matList = new List<Material>();    
    }

    public Vector2 UVSpeed;
   public Vector2 Offset;

    // Update is called once per frame
    void Update()
    {
        Offset += UVSpeed * Time.deltaTime;
        //rend.getma.SetTextureOffset("_MainTex", Offset);
        rend.GetSharedMaterials(matList);
        matList[0].SetTextureOffset("_MainTex", Offset);
    }
}
