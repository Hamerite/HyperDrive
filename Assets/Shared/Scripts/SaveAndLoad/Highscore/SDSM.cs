//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SDSM : MonoBehaviour {
    public static void SaveData(S3_GameOverManager s3_GameOverManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SDS";

        FileStream stream;
        if (File.Exists(path)) stream = new FileStream(path, FileMode.Append);
        else stream = new FileStream(path, FileMode.Create);

        SSD data = new SSD(s3_GameOverManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SSD LoadData() {
        string path = Application.persistentDataPath + "/SDS";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SSD data = formatter.Deserialize(stream) as SSD;
            stream.Close();

            return data;
        }
        else { return null; }
    }
}
