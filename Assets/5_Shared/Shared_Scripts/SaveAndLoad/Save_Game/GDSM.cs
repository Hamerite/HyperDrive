//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GDSM{
    public static void SaveData(GameManager gameManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(GetPath());

        formatter.Serialize(stream, new SGD(gameManager));
        stream.Close();
    }

    public static SGD LoadData(){
        if (File.Exists(GetPath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            SGD data = formatter.Deserialize(stream) as SGD;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Application.persistentDataPath + "/GDS"; }
}
