//Created by Dylan LeClair 15/09/21
//Last modified 23/09/21 (Dylan LeClair)
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour {
    [SerializeField] protected GameObject[] shipChoice;

    protected int index;
    protected float timer = 0;

    void Awake() {
        MenusManager.Instance.UsingMouse = Cursor.visible;
        Cursor.visible = false;

        SPD data = PDSM.LoadData();
        if (data != null) index = data.shipSelected;

        shipChoice[index].SetActive(true);
    }

    void Start() { StartCoroutine(LoadScene()); }

    IEnumerator LoadScene() {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainScene");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone || timer <= 2) {
            timer += Time.deltaTime;
            if(asyncOperation.progress >= 0.9f && timer >= 2) { asyncOperation.allowSceneActivation = true; }

            yield return null;
        }
    }
}
