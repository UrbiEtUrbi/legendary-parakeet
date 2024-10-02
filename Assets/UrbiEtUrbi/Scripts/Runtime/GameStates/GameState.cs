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

        var pos = GameObject.Find("StartPosition");
        var startPos = Vector3.zero;
        if (pos != null)
        {
            startPos = pos.transform.position;
        }
        Player = PoolManager.Spawn<PlayerInstance>(PlayerInstanceKey, transform, startPos, default);

    }


    public virtual void Cleanup()
    {
        if (Player != null)
        {
            PoolManager.Despawn(Player);
        }
    }

}
