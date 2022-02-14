//Created by Dylan LeClair 29/09/21
//Last modified 29/09/21 (Dylan LeClair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class IDSM : MonoBehaviour {
    public static void SaveData(PlayerInfoManager playerInfoManager) {
        using(FileStream stream = new FileStream(GetPath(), FileMode.OpenOrCreate)) {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, new SID(playerInfoManager));
        }
    }

    public static SID LoadData() {
        if (!File.Exists(GetPath())) return null;

        using (FileStream stream = new FileStream(GetPath(), FileMode.Open)) {
            BinaryFormatter formatter = new BinaryFormatter();
            SID data = formatter.Deserialize(stream) as SID;
            return data;
        }
    }

    static string GetPath() { return Path.Combine(Path.Combine(Application.persistentDataPath, "HDSD"), "ISD"); }
}
