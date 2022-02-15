//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GDSM{
    public static void SaveData(GameManager gameManager) {
        using(FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new SGD(gameManager));
        }
    }

    public static SGD LoadData(){
        if (!File.Exists(GetPath())) return null;

        using (FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SGD data = formatter.Deserialize(stream) as SGD;
            return data;
        }
    }

    static string GetPath() { return Path.Combine(Path.Combine(Application.persistentDataPath, "HDSD"), "GSD"); }
}
