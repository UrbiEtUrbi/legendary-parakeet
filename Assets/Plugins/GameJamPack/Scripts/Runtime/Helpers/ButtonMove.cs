using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonMove : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{




    [SerializeField]
    Vector2 move;
    Button b;

    public void OnPointerDown(PointerEventData eventData)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            (transform.GetChild(i).transform as RectTransform).anchoredPosition += move;
        }
        b.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            (transform.GetChild(i).transform as RectTransform).anchoredPosition -= move;
        }
        SoundManager.Instance.Play("button_click");
        b.OnPointerUp(eventData);
    }

    private void Start()
    {

        b = GetComponent<Button>();
    }
    

}
