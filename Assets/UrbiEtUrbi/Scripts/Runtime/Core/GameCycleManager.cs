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

    Camera MainCamera;
    public Camera CurrentCamera;

    [HideInInspector]
    public UnityEvent<GameStateType> OnChangeState = new();


    private void Awake()
    {
        MainCamera = Camera.main;


    }

    public void EnterState(GameStateType gameState)
    {

        DebugLabel.text = $"Current State: {gameState}";

        //exit current state
        if (CurrentStateInstance != null)
        {
            OnChangeState.Invoke(gameState);
            CurrentStateInstance.Cleanup();
            Destroy(CurrentStateInstance.gameObject);
        }

        CurrentStateInstance = Instantiate(States[gameState]);
        CurrentStateInstance.Init();

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

    public void CheatNextState()
    {
        if (CurrentStateInstance == null)
        {
            EnterState(GameStateType.Day);
        }
        else
        {
            switch (CurrentStateInstance.StateType)
            {
                case GameStateType.Day:
                    EnterState(GameStateType.Prep);
                    break;
                case GameStateType.Night:
                    EnterState(GameStateType.Day);
                    break;
                case GameStateType.Prep:
                    EnterState(GameStateType.Night);
                    break;
            }
        }
    }

}


public enum GameStateType
{
    Day,
    Night,
    Prep
}
