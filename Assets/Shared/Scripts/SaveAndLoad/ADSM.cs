//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ADSM{
    public static void SaveData(AudioManager audioManager){
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ADS";

        FileStream stream;
        if (File.Exists(path)) stream = new FileStream(path, FileMode.Append);
        else stream = new FileStream(path, FileMode.Create);

        SAD data = new SAD(audioManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SAD LoadData(){
        string path = Application.persistentDataPath + "/ADS";

        if (File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SAD data = formatter.Deserialize(stream) as SAD;
            stream.Close();

            return data;
        }
        else { return null; }
    }
}
