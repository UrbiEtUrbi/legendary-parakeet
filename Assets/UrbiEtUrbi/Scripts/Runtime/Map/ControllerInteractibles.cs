using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInteractibles : MonoBehaviour
{


    List<InteractibleInstance> CurrentlyAvailable = new();

    bool isInteracting;

    [SerializeField]
    Image Circle;

    Dictionary<Interactible, Image> circles = new();

    private void Start()
    {
        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.Interact.AddListener(Interacting);
        }
    }

    private void OnDestroy()
    {
        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.Interact.RemoveListener(Interacting);
        }
    }


    public void Add(Interactible interactible) {
        if (CurrentlyAvailable.Find(x => x.Interactible == interactible) != null)
        {
            return;
        }
        else
        {
            CurrentlyAvailable.Add(new InteractibleInstance
            {
                Interactible = interactible,
                CurrentTime = 0

            });
        }
    }

    public void Remove(Interactible interactible)
    {
        var instance = CurrentlyAvailable.Find(x => x.Interactible == interactible);
        if (instance != null)
        {
            if (circles.ContainsKey(instance.Interactible))
            {
                Destroy(circles[instance.Interactible].gameObject);
                circles.Remove(instance.Interactible);
            }
            CurrentlyAvailable.Remove(instance);
        }
    }

    void Interacting(bool value)
    {

        if (isInteracting && !value && CurrentlyAvailable.Count > 0)
        {
            foreach (var instance in CurrentlyAvailable)
            {

                if (circles.ContainsKey(instance.Interactible))
                {
                    Destroy(circles[instance.Interactible].gameObject);
                    circles.Remove(instance.Interactible);
                }
                instance.CurrentTime = 0;
            }
        }
        isInteracting = value;
    }

    private void Update()
    {
        if (CurrentlyAvailable.Count > 0 && isInteracting)
        {

            for (int i = CurrentlyAvailable.Count - 1; i >= 0; i--)
            {
                var instance = CurrentlyAvailable[i];
                if (!circles.ContainsKey(instance.Interactible)){

                    circles.Add(instance.Interactible, Instantiate(Circle, TheGame.Instance.Canvas.transform));
                }
                var pos = TheGame.Instance.GameCycleManager.CurrentCamera.WorldToScreenPoint(instance.Interactible.transform.position);

                RectTransformUtility.ScreenPointToWorldPointInRectangle(TheGame.Instance.Canvas.transform as RectTransform, pos, null, out var canvasPos);

                circles[instance.Interactible].rectTransform.anchoredPosition = canvasPos;
                instance.CurrentTime += Time.deltaTime;
              //  var step = 1f / circles[instance.Interactible].mainTexture.height;
                var val = Mathf.Min(instance.CurrentTime / instance.Interactible.InteractTime,1f);
                circles[instance.Interactible].material.SetFloat("_AlphaCutoff",1- (0.999f*val));
                if (instance.CurrentTime >= instance.Interactible.InteractTime && !instance.Interacted)
                {
                    instance.Interacted = true;
                    instance.Interactible.OnInteract();

                }
            }

        }
    }



}

public class InteractibleInstance
{
    public Interactible Interactible;
    public float CurrentTime;
    public bool Interacted;
}
