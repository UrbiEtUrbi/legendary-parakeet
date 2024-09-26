using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    public void EnterState(GameState gameState)
    {

        //exit current state

        switch (gameState)
        {
            case GameState.Day:
                break;

            case GameState.Night:
                break;

            case GameState.Prep:
                break;
        }
    }

}


public enum GameState
{
    Day,
    Night,
    Prep
}
