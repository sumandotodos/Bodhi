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

    float Factor;

    const float LineHeight = 1.8f;

    public void SetColor(Color c)
    {
        BackgroundImage.color = c;
        backgrFader.opaqueColor = c;
    }

    public float SetText(string Text)
    {
        Factor = (1920.0f / ((float)Screen.height));
        TextComponent.text = Text;
        float Height = LineHeight * Factor * TextComponent.cachedTextGenerator.GetPreferredHeight(Text, TextComponent.GetGenerationSettings(new Vector2(BackgroundImage.rectTransform.sizeDelta.x, BackgroundImage.rectTransform.sizeDelta.y)));
        Debug.Log("  ...  " + Height);
        return Height;
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

    public float GetEffectiveHeight()
    {
        return Adjust(Height);
    }

    static public float Adjust(float H)
    {
        return H - 35.0f + H / 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
