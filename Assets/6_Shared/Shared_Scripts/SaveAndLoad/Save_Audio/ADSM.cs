//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ADSM {
    public static void SaveData(AudioManager audioManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(GetPath());

        formatter.Serialize(stream, new SAD(audioManager));
        stream.Close();
    }

    public static SAD LoadData() {
        if (File.Exists(GetPath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.Open(GetPath(), FileMode.Open);

            SAD data = formatter.Deserialize(stream) as SAD;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Application.persistentDataPath + "/ASD"; }
}
