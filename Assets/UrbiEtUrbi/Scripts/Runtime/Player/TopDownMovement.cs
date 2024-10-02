using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement : MonoBehaviour
{


    //fields
    [BeginGroup("Movement")]
    [SerializeField]
    float MaxSpeed;

    [SerializeField]
    bool FreezeVertical;

    //[SerializeField]
    //float MaxSpeedAbility;

    [SerializeField]
    float Acceleration;

    [EndGroup, SerializeField]
    float Drag;


    //private fields
    int m_GroundLayer;
    SpriteRenderer m_Sprite;
    Vector2 m_Velocity;
    Vector2 m_Speed;
    Rigidbody2D m_Rb;

    //properties
    public int Direction => m_Sprite.flipX ? -1 : 1;
    public SpriteRenderer Sprite => m_Sprite;

    //public fields
    [HideInInspector]
    public bool lockOrientation;

    //Vector2 KnockForce;

    //[SerializeField]
    //float knockForce;

    [SerializeField]
    Animator _Animator;

    [SerializeField]
    Transform Art;

    [HideInInspector]
    public Transform ArtObject;


    [HideInInspector]
    public bool Slowing;

    [SerializeField]
    TopDownTool TopDownTool;


    void OnEnable()
    {
        if (TheGame.Instance == null)
        {
            return;
        }

        ArtObject = Instantiate(Art);
        Debug.Log(ArtObject);
        GetComponent<PlayerInstance>().Art = ArtObject.GetComponent<SpriteRenderer>();
        m_GroundLayer = LayerMask.GetMask("Ground");
        m_Sprite = GetComponentInChildren<SpriteRenderer>();
        m_Rb = GetComponent<Rigidbody2D>();

        if (ControllerInput.Instance != null)
        {
            ControllerInput.Instance.Horizontal.AddListener(OnHorizontal);
            ControllerInput.Instance.Vertical.AddListener(OnVertical);
        }
        //ControllerInput.Instance.Jump.AddListener(OnJump);
        //ControllerInput.Instance.Attack.AddListener(OnAttack);
        //ControllerInput.Instance.Cast.AddListener(OnCast);
    }

    void OnDisable()
    {
        
        if (ControllerInput.Instance != null)
        {
            Destroy(ArtObject.gameObject);
            ControllerInput.Instance.Horizontal.RemoveListener(OnHorizontal);
            ControllerInput.Instance.Vertical.RemoveListener(OnVertical);
        }
        //ControllerInput.Instance.Jump.RemoveListener(OnJump);
        //ControllerInput.Instance.Attack.RemoveListener(OnAttack);
        //ControllerInput.Instance.Cast.RemoveListener(OnCast);
    }

    bool playingRun;


    Vector2 prevPosition;


    void FixedUpdate()
    {


        
        //if (TheGame.Instance.IsGameOver)
        //{
        //    m_Velocity = default;
        //    m_Rb.velocity = default;
        //    return;
        //}

        //if (!TheGame.Instance.IsGamePlaying)
        //{

        //    m_Speed = default;
        //    m_Velocity = default;
        //    m_Rb.velocity = default;
        //    return;
        //}



        if (m_Speed == default)
        {
            m_Velocity *= Drag;
        }
        else
        {

            m_Velocity += m_Speed;
        }

        var maxSpeed =  MaxSpeed;



        m_Velocity = m_Velocity.normalized * Mathf.Min((m_Velocity).magnitude, maxSpeed);// + KnockForce.magnitude);

        float unitsPerPixel = 1f / pixelsPerUnit;
        var velBefore = m_Velocity;
        m_Velocity = new Vector2(Mathf.Round(m_Velocity.x / unitsPerPixel), Mathf.Round(m_Velocity.y / unitsPerPixel)) * unitsPerPixel;

        Debug.Log($"{velBefore} {m_Velocity}");
        m_Rb.velocity = m_Velocity;

        if (_Animator.GetBool("Move") != m_Rb.velocity.magnitude > 0.2f)
        {
            _Animator.SetBool("Move", m_Rb.velocity.magnitude > 0.2f);
        }



        var pos = TheGame.Instance.GameCycleManager.CurrentCamera.ScreenToWorldPoint(Mouse.current.position.value);
        Vector3 direction = pos - transform.position;
        direction.z = 0;  // Ignore z-axis since it's 2D

        // Calculate the angle between the direction vector and the world up vector (0, 1, 0)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        m_Sprite.flipX = Mathf.Abs(angle) > 90f;

       

        if (!playingRun)
        {
            if (m_Rb.velocity.magnitude > 0.1f)
            {
                playingRun = true;
                //SoundManager.Instance.PlayLooped("duck_run", source: gameObject, transform);
            }
        }
        if (playingRun)
        {
            if (m_Rb.velocity.magnitude <= 0.1f)
            {
                playingRun = false;
               //SoundManager.Instance.CancelLoop(gameObject);
            }
            
        }

        Debug.Log(m_Rb.position - prevPosition);
        prevPosition = m_Rb.position;
        var position = m_Rb.position;
       
        position.x= Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;

        Debug.Log($"{unitsPerPixel} {Mathf.Round(position.x / unitsPerPixel)} {m_Rb.position} -> {position}");
    }

    [SerializeField]
    int pixelsPerUnit = 16;
    private void LateUpdate()
    {
        var position = m_Rb.position;
        float unitsPerPixel = 1f / pixelsPerUnit;
        position.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
        position.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;

     //   Debug.Log($"{unitsPerPixel} {Mathf.Round(position.x / unitsPerPixel)} {m_Rb.position} -> {position}");
        ArtObject.position = position;
    }

    public void Revive()
    {
        //Animator.SetBool("IsDead", false);
        m_Velocity = default;
        m_Speed = default;
    }

    void OnHorizontal(float amount)
    {
        m_Speed = new Vector2(amount * Acceleration, m_Speed.y);   
    }


    void OnVertical(float amount)
    {
        if (FreezeVertical)
        {
            return;
        }
        m_Speed = new Vector2(m_Speed.x, amount * Acceleration);
        // Animator.SetBool("IsSitting", amount < 0);
    }

    void OnCast()
    {
        //if (AttackTimer > 0)
        //{
        //    return;
        //}
        //if (!ControllerGame.Instance.HasIceMelee && !ControllerGame.Instance.HasSpike)
        //{
        //    return;
        //}
        //AttackTimer = AttackCooldown;


        //SoundManager.Instance.Play("spike_shoot");
        //Animator.SetBool("IsCasting", true);

    }

    public void Knock(Vector3 direction)
    {
        //KnockForce = direction * knockForce;
        //m_Rb.velocity += KnockForce;
        //m_Velocity += KnockForce;
    }

    void OnAttack()
    {


        //if (!ControllerGame.Instance.HasStick)
        //{
        //    return;
        //}
        //if (AttackTimer > 0)
        //{
        //    return;
        //}




        //AttackTimer = AttackCooldown;

        //SoundManager.Instance.Play("melee_attack");
        //Animator.SetBool("IsAttacking", true);


    }

    void OnEndAttack()
    {
        //Animator.SetBool("IsAttacking", false);
        //Animator.SetBool("IsCasting", false);
    }

    void OnJump(bool jump)
    {
        //if (jump)
        //{
        //    _Animator.SetBool("Channel", true);
        // //   Mask.Show();
        //}
        //else
        //{
        //    _Animator.SetBool("Channel", false);
        // //   Mask.Hide();

        //}
        //jumped = jump && onGround;
        //if (jumped)
        //{

        //    jumpApplied = false;

        //}

    }
}
