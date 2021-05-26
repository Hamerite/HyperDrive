//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PDSM : MonoBehaviour {
    public static void SaveData(S1_ShipSelect s1_ShipSelect) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PDS";

        FileStream stream;
        if (File.Exists(path)) stream = new FileStream(path, FileMode.Append);
        else stream = new FileStream(path, FileMode.Create);

        SPD data = new SPD(s1_ShipSelect);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SPD LoadData() {
        string path = Application.persistentDataPath + "/PDS";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SPD data = formatter.Deserialize(stream) as SPD;
            stream.Close();

            return data;
        }
        else { return null; }
    }
}
