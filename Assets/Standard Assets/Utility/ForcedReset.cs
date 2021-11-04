using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//[RequireComponent(typeof (GUITexture))]
[RequireComponent(typeof(Image))]
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // if we have forced a reset ...
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            //... reload the scene
            //Application.LoadLevelAsync(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
