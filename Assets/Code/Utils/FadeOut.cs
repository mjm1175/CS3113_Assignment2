using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FadeOut : MonoBehaviour
{
    public float Duration = 5f;

    private bool _isFading;
    private Text _text;

    public IEnumerator TextFade()
    {
        int i = 0;
        while (i < 1)
        {
            yield return new WaitForSeconds(Duration * 2);
            StartCoroutine(FadeTextToZeroAlpha(1f));

            i += 1;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
        while (_text.color.a > 0.0f)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - (Time.deltaTime / t));
            yield return null;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if (_text.enabled && !_isFading)
        {
            _isFading = true;
            StartCoroutine(TextFade());
        }
    }
}