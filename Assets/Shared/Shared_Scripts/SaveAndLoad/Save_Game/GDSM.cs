//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class GDSM{
    public static void SaveData(GameManager gameManager) {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GDS";

        FileStream stream;
        if (File.Exists(path)) stream = new FileStream(path, FileMode.Append);
        else stream = new FileStream(path, FileMode.Create);

        SGD data = new SGD(gameManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SGD LoadData(){
        string path = Application.persistentDataPath + "/GDS";

        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SGD data = formatter.Deserialize(stream) as SGD;
            stream.Close();

            return data;
        }
        else return null;
    }

    public static void DeleteData() {
        string path = Application.persistentDataPath + "/GDS";
        if (File.Exists(path)) File.Delete(path);
    }
}
