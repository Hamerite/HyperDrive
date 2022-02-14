//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SDSM : MonoBehaviour {
    public static void SaveData(S3_GameOverManager S3_GameOverManager) {
        using(FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new SSD(S3_GameOverManager));
        }
    }

    public static SSD LoadData() {
        if (!File.Exists(GetPath())) return null;

        using (FileStream stream = new FileStream(GetPath(), FileMode.Open)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SSD data = formatter.Deserialize(stream) as SSD;
            return data;
        }
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Path.Combine(Path.Combine(Application.persistentDataPath, "HDSD"), "SSD"); }
}
