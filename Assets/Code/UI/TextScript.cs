using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class TextScript : MonoBehaviour
{
    private Text text;
    private void Start() {
        text = GetComponent<Text>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        //gameObject.SetActive(false);
        StartCoroutine(TextFade(text));
    }

    public IEnumerator TextFade(Text text){
        int i=0;
        while (i < 1){
            yield return new WaitForSeconds(2f);
            //gameObject.SetActive(true);
            StartCoroutine(FadeTextToFullAlpha(1f, text));

            yield return new WaitForSeconds(3f);
            StartCoroutine(FadeTextToZeroAlpha(1f, text));
            
            i+=1;
        }
    }
 
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
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
}