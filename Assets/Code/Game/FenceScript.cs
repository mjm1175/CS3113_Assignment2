using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            if (other.GetComponent<Inventory>().CheckItem(ItemType.WireCutter)){
                PublicVars.TransitionManager.CrossFadeTo(PublicVars.TransitionManager.RegularMusic, PublicVars.MUSIC_TRANSITION_TIME);
                PublicVars.TransitionManager.FadeToScene("Win", PublicVars.GENERAL_FADE_TIME);
            }
        }
    }
}
