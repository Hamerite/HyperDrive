//Created by Dylan LeClair 29/09/21
//Last modified 29/09/21 (Dylan LeClair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class IDSM : MonoBehaviour {
    public static void SaveData(PlayerInfoManager playerInfoManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(GetPath());

        formatter.Serialize(stream, new SID(playerInfoManager));
        stream.Close();
    }

    public static SID LoadData() {
        if (File.Exists(GetPath())) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetPath(), FileMode.Open);

            SID data = formatter.Deserialize(stream) as SID;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() { if (File.Exists(GetPath())) File.Delete(GetPath()); }

    static string GetPath() { return Application.persistentDataPath + "/ISD"; }
}
