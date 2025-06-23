using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;
    bool Dead = false;
    GameEngine gameEngine;
    public Animator animator;
    [Serialize] public LayerMask GroundMask;
    //For movement calculations
    public float ForwardSpeed = 1;
    public bool PointsBoosted = false;
    public float VeerSpeed = 2;
    public float FallMultiplier = 40f;
    float VeerInput;
    public float JumpForce = 400f;
    bool isGrounded = true;
    //For Speed boost pickup
    public float SpeedMultiplier = 1;
    public bool SpeedBoosted = false;
    //For shield buff
    public bool isShielded = false;
    //For double jump buff
    public bool hasDoubleJump = false;
    public bool DoubleJumpAvailable = true;

    private CharacterController controller;

    private void Awake()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        StartCoroutine(SubscribeToEvents());
    }
    private IEnumerator SubscribeToEvents()
    {
        while (EventManager.Inst == null)
        {
            yield return null; 
        }

        EventManager.Inst.OnDoubleJumpPickup += HandleDoubleJumpPickup;
        EventManager.Inst.OnDoubleJumpPickup += HandleShieldPickup;
        //Debug.Log("Successfully subscribed to events");
    }

    private void OnDisable()
    {
        if (EventManager.Inst != null)
        {
            EventManager.Inst.OnDoubleJumpPickup -= HandleDoubleJumpPickup;
            EventManager.Inst.OnDoubleJumpPickup -= HandleShieldPickup;
        }
        StopAllCoroutines();
    }
    private void FixedUpdate()
    {
        if (Dead) return;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, GroundMask);
        if (isGrounded)
            animator.SetBool("isJumping", false);
        //Movement Controlls
        Vector3 Run = transform.forward * ForwardSpeed * SpeedMultiplier * Time.fixedDeltaTime;
        Vector3 Veer = transform.right * VeerInput * SpeedMultiplier * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + Run + Veer);
    }
    private void Update()
    {
        if (Dead) return;
        VeerInput = Input.GetAxis("Horizontal") * VeerSpeed;
        if (Input.GetKeyDown(KeyCode.Escape))
            gameEngine.TogglePause();
        if (transform.position.y < -2)
            KillPlayer();
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.S))
            Drop();
    }
    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce * ForwardSpeed);
            DoubleJumpAvailable = true;
            animator.SetBool("isJumping", true);
            animator.Play("Jumping"); // Play the jump animation
        }
        else if (hasDoubleJump && DoubleJumpAvailable)
        {
            rb.AddForce(Vector3.up * JumpForce * ForwardSpeed);
            DoubleJumpAvailable = false;
            animator.SetBool("isJumping", true);
            animator.Play("Jumping"); // Play the jump animation again for double jump
        }
    }
    void Drop()
    {
        rb.AddForce(-Vector3.up * JumpForce * FallMultiplier * ForwardSpeed);
    }

    public void KillPlayer()
    {
        Dead = true;
        gameEngine.GameOver();
    }

    //Functions for speed boost
    public void SpeedBoost(int duration, float BoostAmount)
    {
        if (SpeedBoosted == true)
        {
            CancelInvoke("EndSpeedBoost");
            SpeedMultiplier = BoostAmount;
            Invoke("EndSpeedBoost", duration);
            Debug.Log("Speed Boost Extended");
        }
        else
        {
            SpeedBoosted = true;
            SpeedMultiplier = BoostAmount;
            Invoke("EndSpeedBoost", duration);
            Debug.Log("Speed Boost Gained");
        }
    }
    void EndSpeedBoost()
    {
        SpeedBoosted = false;
        SpeedMultiplier = 1;
        //Debug.Log("Speed Boost Lost");
    }
    // Functions for shield
    public void HandleShieldPickup(int duration)
    {
        if (isShielded)
        {
            CancelInvoke(nameof(EndShieldBuff));
        }
        else
        {
            isShielded = true;
        }
        Invoke(nameof(EndShieldBuff), duration);
    }
    public void EndShieldBuff()
    {
        CancelInvoke(nameof(EndShieldBuff));
        isShielded = false;
    }
    // Functions for Double Jump 
    private void HandleDoubleJumpPickup(int duration)
    {
        if (hasDoubleJump)
        {
            CancelInvoke(nameof(EndDoubleJump ));
        }
        else
        {
            hasDoubleJump = true;
            DoubleJumpAvailable = true;
        }
    Invoke(nameof(EndDoubleJump), duration);
    }
    public void EndDoubleJump()
    {
        CancelInvoke(nameof(EndDoubleJump));
        hasDoubleJump = false;
    }
}