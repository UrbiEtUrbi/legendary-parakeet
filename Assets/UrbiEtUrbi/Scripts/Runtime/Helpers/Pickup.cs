using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class Pickup : PoolObject
{



    [SerializeField]
    TweenSettings<Vector3> TweenSettings;



    [SerializeField]
    string sound;

    Tween t;

    Vector3 startValueFirst;
    Vector3 endValueFirst;

    private void Awake()
    {
        startValueFirst = TweenSettings.startValue;
        endValueFirst = TweenSettings.endValue;
    }

    protected void Init()
    {
        //t.Stop();
        //TweenSettings.startValue = startValueFirst + transform.position;
        //TweenSettings.endValue = endValueFirst + transform.position;


        //Tween.Delay(Random.value,() => { t = Tween.Position(transform, TweenSettings); });
    }




    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){

    //        //TODO collect only if alive
    //        //if (ControllerGame.Player.CurrentHealth > 0)
    //        //{
    //            TheGame.Instance.ControllerPickups.Pickup(this);

    //        //    SoundManager.Instance.Play(sound);
    //        t.Stop();
    //        PoolManager.Despawn(this);
    //        //}
    //    }
    //}

    public void PickupResource()
    {
        TheGame.Instance.ControllerPickups.Pickup(this);
        t.Stop();
    }
  

}


