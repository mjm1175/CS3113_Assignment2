using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public void Restart()
    {
        // cleanup
        PublicVars.Game.InitializeGame();

        PublicVars.TransitionManager.FadeToScene("Menu", PublicVars.GENERAL_FADE_TIME);
    }
}
