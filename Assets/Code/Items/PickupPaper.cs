using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupPaper : MonoBehaviour
{
    public Text pickupText;
    public Text storyText;

    private void Start() {
        pickupText.enabled = false;   
        storyText.enabled = false; 
    }

        public IEnumerator TextFade(Text text){
            int i=0;
            while (i < 1){
                yield return new WaitForSeconds(5f);
                StartCoroutine(FadeTextToZeroAlpha(1f, text));
                
                i+=1;
        }
    }

     public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = true;
            if (Input.GetKeyDown("j")){
                PublicVars.TransitionManager.PickupPaperSound.Play();
                pickupText.enabled = false;
                storyText.enabled = true; 
                PublicVars.PaperCount++;
                Destroy(gameObject);
                return;   
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")){
            pickupText.enabled = false;
            //StartCoroutine(TextFade(storyText));
            //Destroy(storyText.gameObject, 5.0f);
        }
    }
}
