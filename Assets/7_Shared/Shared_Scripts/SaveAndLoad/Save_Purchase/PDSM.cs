//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PDSM : MonoBehaviour {
    public static void SaveData(S5_ShipSelect s5_ShipSelect) {
        using(FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new SPD(s5_ShipSelect));
        }
    }

    public static SPD LoadData() {
        if (!File.Exists(GetPath())) return null;

        using (FileStream stream = new FileStream(GetPath(), FileMode.Open)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SPD data = formatter.Deserialize(stream) as SPD;
            return data;
        }
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Path.Combine(Path.Combine(Application.persistentDataPath, "HDSD"), "PSD"); }
}
