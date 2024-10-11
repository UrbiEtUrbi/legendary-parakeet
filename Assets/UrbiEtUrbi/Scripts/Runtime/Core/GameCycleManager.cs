using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameCycleManager : MonoBehaviour
{

    public TMP_Text DebugLabel;


    [SerializeField]
    SerializedDictionary<GameStateType, GameState> States;

    GameState CurrentStateInstance;
    public GameState GetCurrentState => CurrentStateInstance;
    public GameStateType GetCurrentStateType => GetCurrentState.StateType;

    Camera MainCamera;
    public Camera CurrentCamera;

    [HideInInspector]
    public UnityEvent<GameStateType> OnChangeState = new();

    [SerializeField]
    Bar Bar;


    float timer;

    IEnumerator timeCoroutine;


    private void Awake()
    {
        MainCamera = Camera.main;


    }

    public void exit()
    {
        CurrentStateInstance.Cleanup();
        Destroy(CurrentStateInstance.gameObject);
    }

    public void EnterState(GameStateType gameState)
    {
       
        

        //exit current state
        if (CurrentStateInstance != null)
        {
            OnChangeState.Invoke(gameState);
            CurrentStateInstance.Cleanup();
            Destroy(CurrentStateInstance.gameObject);
        }

        CurrentStateInstance = Instantiate(States[gameState]);
        CurrentStateInstance.Init();

        if (CurrentStateInstance.Duration > 0)
        {
            Bar.SetValue(1);
            StartCoroutine(timeCoroutine = TimeStage());

        }

        var camera = CurrentStateInstance.GetComponentInChildren<Camera>();
        
        MainCamera.gameObject.SetActive(camera == null);

        CurrentCamera = camera == null ? MainCamera : camera;
        
        

        //switch (gameState)
        //{
        //    case GameStateType.Day:
                
        //        break;

        //    case GameStateType.Night:
        //        break;

        //    case GameStateType.Prep:
        //        break;
        //}
    }


    public void EndStage()
    {
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
        CurrentStateInstance.OnEndStage();

    }
    IEnumerator TimeStage()
    {
        timer = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
            SetProgress(1 - timer / CurrentStateInstance.Duration);
            if (timer >= CurrentStateInstance.Duration)
            {
                CurrentStateInstance.OnEndStage();
                
                yield break;
              
            }
        }

    }

    public void SetProgress(float value)
    {
        Bar.SetValue(value);
    }
    

    public void CheatNextState()
    {
      
        if (timeCoroutine != null)
        {
            StopCoroutine(timeCoroutine);
            timeCoroutine = null;
        }
        if (CurrentStateInstance == null)
        {
            EnterState(GameStateType.Night);
        }
        else
        {
            switch (CurrentStateInstance.StateType)
            {
                case GameStateType.Day:

                    DebugLabel.text = $"Prep Time:";
                    EnterState(GameStateType.Night);
                    break;
                case GameStateType.Night:
                    DebugLabel.text = $"Time Until Nightfall:";
                    TheGame.Instance.RoundNumber++;
                    EnterState(GameStateType.Day);
                    break;
            }
        }
    }

}


