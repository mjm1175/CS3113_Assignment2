using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void StartBtn()
    {
        // TODO: change if starting scene changes; make sure all public vars are reset
        SceneManager.LoadScene("Cell");
    }

    public void Quit()
    {
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #elif (UNITY_WEBGL)
            // TODO: change to not show button at all
            Application.OpenURL("about:blank");
        #endif
    }
}
