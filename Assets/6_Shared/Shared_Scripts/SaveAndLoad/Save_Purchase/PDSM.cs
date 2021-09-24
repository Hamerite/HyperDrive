//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PDSM : MonoBehaviour {
    public static void SaveData(S5_ShipSelect s5_ShipSelect) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(GetPath());

        formatter.Serialize(stream, new SPD(s5_ShipSelect));
        stream.Close();
    }

    public static SPD LoadData() {
        if (File.Exists(GetPath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            SPD data = formatter.Deserialize(stream) as SPD;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Application.persistentDataPath + "/PDS"; }
}
