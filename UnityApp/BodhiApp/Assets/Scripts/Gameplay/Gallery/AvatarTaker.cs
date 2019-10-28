using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AvatarImage
{
    public byte[] data;
}

public class AvatarTaker : MonoBehaviour
{
    public Texture2D MaskTexture;
    public Texture2D DefaultUserTexture;
    public Texture2D MockImage;

    public int TextureSize = 256;

    System.Action<Texture2D> ImageGottenCallback;

    public Texture2D Sphericalize(Texture2D tex)
    {
        Texture2D newTex = new Texture2D(3 * tex.width, tex.height);
        for(int i = 0; i < tex.height; ++i)
        {
            for(int j=0; j < 3*tex.width; ++j)
            {
                newTex.SetPixel(j, i, tex.GetPixel(j % tex.width, i));
            }
        }
        newTex.Apply();
        return newTex;
    }

    public Texture2D ApplyMaskTexture(Texture2D tex)
    {

        int targetSize = MaskTexture.width;
        Texture2D crop = CropTextureToSquare(tex);
        TextureScale.Bilinear(crop, targetSize, targetSize);
        Texture2D result = new Texture2D(targetSize, targetSize, TextureFormat.ARGB32, true);
        for(int i = 0; i < targetSize; ++i)
        {
            for(int j = 0; j < targetSize; ++j)
            {
                Color maskPixel = MaskTexture.GetPixel(j, i);
                Color cropPixel = crop.GetPixel(j, i);
                if (maskPixel.a > 0.0f)
                {
                    result.SetPixel(j, i, maskPixel * cropPixel);
                }
                else
                {
                    result.SetPixel(j, i, new Color(0, 0, 0, 0));
                }
            }
        }
        result.Apply();
        return result;
              
    }

    public Texture2D Rescale(Texture2D tex, int newWidth, int newHeight)
    {
        Texture2D newTex = new Texture2D(tex.width, tex.height);
        newTex.SetPixels(tex.GetPixels());
        TextureScale.Bilinear(newTex, newWidth, newHeight);
        return newTex;
    }

    private Texture2D RotateTexture(Texture2D tex)
    {
        Texture2D result = new Texture2D(tex.height, tex.width);
        for(int i = 0; i < tex.height; ++i)
        {
            for(int j = 0; j < tex.width; ++j)
            {
                result.SetPixel(i, j, tex.GetPixel(j, i));
            }
        }
        result.Apply();
        return result;
    }

    public Texture2D GetDefaultUserAvatar()
    {

        return DefaultUserTexture;
    }

   public Texture2D CropTextureToSquare(Texture2D inTex)
    {
        if(inTex.width > inTex.height)
        {
            Texture2D newTex = new Texture2D(inTex.height, inTex.height);
            int JOffset = (inTex.width - inTex.height) / 2;
            for(int i = 0; i < inTex.height; ++i)
            {
                for(int j = 0; j < inTex.height; ++j)
                {
                    newTex.SetPixel(j, i, inTex.GetPixel(JOffset + j, i));
                }
            }
            newTex.Apply();
            return newTex;
        }
        else
        {
            Texture2D newTex = new Texture2D(inTex.width, inTex.width);
            int IOffset = (inTex.height - inTex.width) / 2;
            for (int i = 0; i < inTex.width; ++i)
            {
                for (int j = 0; j < inTex.width; ++j)
                {
                    newTex.SetPixel(j, i, inTex.GetPixel(j, i + IOffset));
                }
            }
            newTex.Apply();
            return newTex;
        }

    }

    public void GetAvatarFromCamera(System.Action<Texture2D> callback)
    {
        GetImageFromCamera((tex) =>
        {
            Texture2D newAvatar = Application.platform != RuntimePlatform.IPhonePlayer ? RotateTexture(tex) : tex;
            //TextureScale.Bilinear(newAvatar, TextureSize, TextureSize);
            callback(newAvatar);
        });
    }

    public void GetAvatarFromGallery(System.Action<Texture2D> callback)
    {
        GetImageFromGallery((tex) =>
        {
            Texture2D newAvatar = tex;
            //TextureScale.Bilinear(newAvatar, TextureSize, TextureSize);
            callback(newAvatar);
        });
    }

    public void GetImageFromGallery(System.Action<Texture2D> callback)
    {
#if UNITY_EDITOR
        callback(MockImage);
#else
        ImageGottenCallback = callback;
        NativeGallery.GetImageFromGallery(GetImagePathCallback, "Elige avatar", "image/*", -1);
#endif
    }

    public void GetImageFromCamera(System.Action<Texture2D> callback)
    {
#if UNITY_EDITOR
        callback(MockImage);
#else
        ImageGottenCallback = callback;
        NativeCamera.TakePicture(GetImagePathCallback, -1);
#endif
    }

    void GetImagePathCallback(string path)
    {
        if (ImageGottenCallback != null)
        {
            ImageGottenCallback(LoadPNG(path));
        }
    }

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

    public int SaveAvatar(Texture2D tex)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/avatar.jpg", FileMode.Create);
        AvatarImage data = new AvatarImage();
        data.data = tex.EncodeToJPG();
        formatter.Serialize(file, data);
        file.Close();
        return data.data.Length;
    }

    public Texture2D LoadAvatar()
    {
        if (File.Exists(Application.persistentDataPath + "/avatar.jpg"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/avatar.jpg", FileMode.Open);
            byte[] data = ((AvatarImage)formatter.Deserialize(file)).data;
            file.Close();
            Texture2D result = new Texture2D(2, 2);
            ImageConversion.LoadImage(result, data);
            result.Apply();
            return result;

        }
        else
        {
            return DefaultUserTexture;
        }
    }
}
