using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    [SerializeField]
    Image Image;

    float Step;

    private void Awake()
    {
        Step =1f/ (Image.rectTransform.sizeDelta.x * 0.16f);
    }

    public void SetValue(float value)
    {
        var val = Mathf.Round(value / Step) * Step;
        Image.fillAmount = val;
    }

}
