using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject quitBtn;

    private void Start() {
        #if (UNITY_EDITOR)
            quitBtn.SetActive(true);
        #elif (UNITY_STANDALONE) 
            quitBtn.SetActive(true);
        #elif (UNITY_WEBGL)
            quitBtn.SetActive(false);
        #endif
    }

    public void StartBtn()
    {
        Room.Enter("Cell");
    }

    public void Quit()
    {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #endif
    }
}
