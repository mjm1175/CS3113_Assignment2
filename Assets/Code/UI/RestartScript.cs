using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScript : MonoBehaviour
{
    public void Menu()
    {
        PublicVars.Game.InitializeGame();
        PublicVars.TransitionManager.FadeToScene("Menu", PublicVars.GENERAL_FADE_TIME);
    }
    public void Restart()
    {
        if (Room.CurrentCheckPoint != null)
        {
            PublicVars.Reset(false);
            Room.EnterCheckPoint();
            FenceScript.triggered = false;
        }
        else
        {
            PublicVars.Game.InitializeGame();
            PublicVars.TransitionManager.FadeToScene("Menu", PublicVars.GENERAL_FADE_TIME);
        }

    }
}
