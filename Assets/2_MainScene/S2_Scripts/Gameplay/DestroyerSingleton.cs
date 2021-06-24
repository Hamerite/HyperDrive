// Created by Dylan LeClair 05/06/21
//Last modified 05/06/21 (Dylan LeClair)
using UnityEngine;

public class DestroyerSingleton : MonoBehaviour {
    public static DestroyerSingleton Instance { get; private set; }

    private void Awake() { Instance = this; }
}
