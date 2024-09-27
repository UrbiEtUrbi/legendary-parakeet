using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{

    public GameStateType StateType;

    public string PlayerInstanceKey;
    [HideInInspector]
    public PlayerInstance Player;


    public virtual void Init()
    {
        if (string.IsNullOrEmpty(PlayerInstanceKey))
        {
            return;
        }
        Player = PoolManager.Spawn<PlayerInstance>(PlayerInstanceKey, transform, default, default);

    }


    public virtual void Cleanup()
    {
        if (Player != null)
        {
            PoolManager.Despawn(Player);
        }
    }

}
