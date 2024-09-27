using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField]
    string CustomSound;

    string defaultSound = "click";

    


    private void Start()
    {

        var button = GetComponent<Button>();
        if (button) {
            button.onClick.AddListener(() => SoundManager.Instance.Play(string.IsNullOrEmpty(CustomSound) ? defaultSound : CustomSound));
        }
    }
}
