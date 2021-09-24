//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SDSM : MonoBehaviour {
    public static void SaveData(S3_GameOverManager S3_GameOverManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(GetPath());

        formatter.Serialize(stream, new SSD(S3_GameOverManager));
        stream.Close();
    }

    public static SSD LoadData() {
        if (File.Exists(GetPath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            SSD data = formatter.Deserialize(stream) as SSD;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Application.persistentDataPath + "/SDS"; }
}
