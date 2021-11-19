using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrateScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            if (other.GetComponent<Inventory>().CheckItem(ItemType.Crowbar)){
                PublicVars.TransitionManager.CrossFadeTo(PublicVars.TransitionManager.RegularMusic, PublicVars.MUSIC_TRANSITION_TIME);
                PublicVars.TransitionManager.FadeToScene("Win", PublicVars.GENERAL_FADE_TIME);
            }
        }
    }
}
