using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{

    public GameStateType StateType;

    public string PlayerInstanceKey;
    [HideInInspector]
    public PlayerInstance Player;



    public float Duration;


    public virtual void Init()
    {
        if (string.IsNullOrEmpty(PlayerInstanceKey))
        {
            return;
        }

        var pos = GetComponentInChildren<StartPosition>().gameObject;
        var startPos = Vector3.zero;
        if (pos != null)
        {
            startPos = pos.transform.position;
        }
        Player = PoolManager.Spawn<PlayerInstance>(PlayerInstanceKey, transform, startPos, default);
        var rb = Player.GetComponentInChildren<Rigidbody2D>();
        if (rb)
        {
            rb.position = startPos;
        }
        Debug.Log($"{pos} ->{startPos} {rb.position} {pos.name}");
        

    }

    public void DisablePlayer()
    {
        Player.gameObject.SetActive(false);
    }

    public void EnablePlayer()
    {
        Player.gameObject.SetActive(true);
    }

    public virtual void OnEndStage()
    {
        if (Player)
        {
            Player.gameObject.SetActive(false);
        }
    }


    public virtual void Cleanup()
    {
        if (Player != null)
        {
            PoolManager.Despawn(Player);
        }
    }

}
