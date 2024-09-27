using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

   
    public string Id;


    [SerializeField]
    float Frequency;

    [SerializeField]
    float Amplitude;

    Vector3 startPos;

    float timer = 0;

    [SerializeField]
    string sound;

    private void Start()
    {
       
        startPos = transform.position;

    }

    public PickupType PickupType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")){


            //TODO collect only if alive
            //if (ControllerGame.Player.CurrentHealth > 0)
            //{
            //    ControllerGame.ControllerPickups.Pickup(this);
            //    SoundManager.Instance.Play(sound);
            //    Destroy(this.gameObject);
            //}
        }
    }

    private void FixedUpdate()
    {
        transform.position = startPos + new Vector3(0,Mathf.Sin(timer * Frequency) * Amplitude, 0);
        timer += Time.fixedDeltaTime;
    }

}

[System.Serializable]
public enum PickupType
{
    MaxHealth,
}
