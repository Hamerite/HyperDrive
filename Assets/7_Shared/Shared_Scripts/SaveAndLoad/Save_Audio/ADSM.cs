//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ADSM {
    public static void SaveData(AudioManager audioManager) {
        using(FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new SAD(audioManager));
        }
    }

    public static SAD LoadData() {
        if (!File.Exists(GetPath())) return null;

        using (FileStream stream = new FileStream(GetPath(), FileMode.Open)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SAD data = formatter.Deserialize(stream) as SAD;
            return data;
        }
    }

    static string GetPath() { return Path.Combine(Path.Combine(Application.persistentDataPath, "HDSD"), "ASD"); }
}
