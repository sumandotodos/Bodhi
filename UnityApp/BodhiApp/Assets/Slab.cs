using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slab : MonoBehaviour
{
    public int Index;
    public Image BackgroundImage;
    public Image FrameImage;
    public Text TextComponent;
    public UIFader backgrFader;
    public string id;
    float Height;

    public void SetColor(Color c)
    {
        BackgroundImage.color = c;
        backgrFader.opaqueColor = c;
    }

    public float SetText(string Text)
    {
        TextComponent.text = Text;
        return 8.0f * TextComponent.cachedTextGenerator.GetPreferredHeight(Text, TextComponent.GetGenerationSettings(new Vector2(BackgroundImage.rectTransform.sizeDelta.x, 1000.0f)));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetHeight(float _Height)
    {
        Height = _Height;
        Vector2 d = BackgroundImage.rectTransform.sizeDelta;
        d.y = Height*1.5f;
        BackgroundImage.rectTransform.sizeDelta = d;
        //BackgroundImage.rectTransform.sizeDelta.y = Height;
        d = FrameImage.rectTransform.sizeDelta;
        d.y = Height*1.5f;
        FrameImage.rectTransform.sizeDelta = d;
        // FrameImage.rectTransform.sizeDelta.y = Height;
        d = TextComponent.rectTransform.sizeDelta;
        d.y = Height;
        TextComponent.rectTransform.sizeDelta = d;
        //TextComponent.preferredHeight = Height;
    }

    public float GetHeight()
    {
        return Height;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
