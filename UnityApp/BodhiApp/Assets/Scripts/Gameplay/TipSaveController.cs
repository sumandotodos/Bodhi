using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class TipInfo
{
    public Dictionary<string, bool> TipShown = new Dictionary<string, bool>();
}

public class TipSaveController : MonoBehaviour {

   static TipSaveController instance;

    TipInfo tipInfo;

    private void Start()
    {
        instance = this;
        LoadTips();
    }

    static void LoadTips()
    {
        if (File.Exists(Application.persistentDataPath + "/tipsavedata.dat"))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/tipsavedata.dat", FileMode.Open);
            instance.tipInfo = (TipInfo)formatter.Deserialize(file);

            file.Close();

        }
        else
        {
            instance.tipInfo = new TipInfo();
        }
    }

    static void SaveTips()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/tipsavedata.dat", FileMode.Create);

        formatter.Serialize(file, instance.tipInfo);
        file.Close();

    }

    public static bool MustDoTip(string name)
    {
        if (instance.tipInfo.TipShown.ContainsKey(name)) return false;
        else
        {
            instance.tipInfo.TipShown[name] = true;
            SaveTips();
            return true;
        }
    }

    public static TipInfo GetTipsInfo()
    {
        return instance.tipInfo;
    }

    public  void SetTipInfo(TipInfo newInfo)
    {
        instance.tipInfo = newInfo;
        SaveTips();
    }
}
