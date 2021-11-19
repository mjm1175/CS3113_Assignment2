using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PaperCount : MonoBehaviour
{
    public TextMeshProUGUI paperCountText;

    void Update()
    {
        if (paperCountText) paperCountText.text = PublicVars.PaperCount.ToString();

    }
}
