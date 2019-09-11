using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WritingController : MonoBehaviour
{
    public Text NotebookText;

    public void OnTextChanged(string newText)
    {
        NotebookText.text = newText;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
