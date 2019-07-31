using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*[System.Serializable]
public class IntuitionPoints
{
    public int points;
}*/

[System.Serializable]
public class MsgIndex
{
    public int from;
}

public class LoadSaveController : MonoBehaviour
{

    //public static int persist_intuitionPoints;

    /*public static void SaveVirtualGames()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.001.dat", FileMode.Create);
        formatter.Serialize(file, persist_virtualBottleGames);
        file.Close();

    }

    public static void LoadVirtualGames()
    {
        if(File.Exists(Application.persistentDataPath + "/save.001.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.001.dat", FileMode.Open);
            persist_virtualBottleGames = (VirtualBottleGames)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            persist_virtualBottleGames = new VirtualBottleGames();
        }
    }*/

    public static void SaveLatestMessageId(int from)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.003.dat", FileMode.Create);
        MsgIndex data = new MsgIndex();
        data.from = from;
        formatter.Serialize(file, data);
        file.Close();
    }

    public static int LoadLatestMessageId()
    {
        if (File.Exists(Application.persistentDataPath + "/save.003.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.003.dat", FileMode.Open);
            int from = ((MsgIndex)formatter.Deserialize(file)).from;
            file.Close();
            return from;
        }
        else
        {
            return 0;
        }
    }
    /*
    public static void SavePoints()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.004.dat", FileMode.Create);
        IntuitionPoints data = new IntuitionPoints();
        data.points = persist_intuitionPoints;
        formatter.Serialize(file, data);
        file.Close();
    }

    public static void LoadPoints()
    {
        if (File.Exists(Application.persistentDataPath + "/save.004.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.004.dat", FileMode.Open);
            int points = ((IntuitionPoints)formatter.Deserialize(file)).points;
            file.Close();
            persist_intuitionPoints = points;
        }
        else
        {
            persist_intuitionPoints = 0;
        }
    }

    public static void SaveRealGames()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/save.002.dat", FileMode.Create);
        formatter.Serialize(file, persist_realBottleGames);
        file.Close();

    }

    public static void LoadRealGames()
    {
        if (File.Exists(Application.persistentDataPath + "/save.002.dat"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.002.dat", FileMode.Open);
            persist_realBottleGames = (RealBottleGames)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            persist_realBottleGames = new RealBottleGames();
        }
    }
    */
}
