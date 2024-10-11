using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameCycleManager : MonoBehaviour
{

    [SerializeField]
    TMP_Text DebugLabel;


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

    public void EnterState(GameStateType gameState)
    {
        Debug.Log(gameState);
        if (gameState == GameStateType.Day)
        {
            DebugLabel.text = $"Time Until Nightfall:";
        } 
        else
        {
            DebugLabel.text = $"Time Until Daybreak:";
        }
        

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
            StartCoroutine(TimeStage());

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
            Bar.SetValue(1 - timer / CurrentStateInstance.Duration);
            if (timer >= CurrentStateInstance.Duration)
            {

                CurrentStateInstance.OnEndStage();
                
                yield break;
            }
        }

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
                    EnterState(GameStateType.Night);
                    break;
                case GameStateType.Night:
                    EnterState(GameStateType.Day);
                    break;
            }
        }
    }

}


public enum GameStateType
{
    Day,
    Night,
}
