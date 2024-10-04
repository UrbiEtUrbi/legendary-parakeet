using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
public class PopupBase : MonoBehaviour
{


    [SerializeField]
    RectTransform RectTransform;


    protected void Init()
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Tween.UIAnchoredPosition(RectTransform, new Vector2(0, 0), 0.5f, Ease.OutBack);

    }

    public void Hide()
    {

        Tween.UIAnchoredPosition(RectTransform, new Vector2(0, -339), 0.5f, Ease.InBack).OnComplete(() => gameObject.SetActive(false));
    }

   


}
