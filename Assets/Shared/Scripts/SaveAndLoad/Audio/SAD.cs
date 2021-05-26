//Created by Dylan LeClair 24/05/21
//Last modified 24/05/21 (Dylan Leclair)
[System.Serializable]
public class SAD{
    public bool[] mutes;
    public float[] volumes;

    public SAD(AudioManager audioManager){
        mutes = audioManager.GetMutes();
        volumes = audioManager.GetVolumes();
    }
}
