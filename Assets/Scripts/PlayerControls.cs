using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;
    bool Dead = false;
    private bool isPaused;
    //For movement calculations
    public float ForwardSpeed = 1;
    public bool PointsBoosted = false;
    public float VeerSpeed = 2;
    public float FallMultiplier = 40f;
    float VeerInput;
    public float JumpForce = 400f;
    bool isGrounded=true;
    //For Speed boost pickup
    public float SpeedMultiplier =1;
    public bool SpeedBoosted = false;
    //For shield buff
    public bool isShielded = false;
    //For flight buff
    public bool hasDoubleJump = false;
    public bool DoubleJumpAvailable = true;
    GameEngine gameEngine;
    [Serialize] public LayerMask GroundMask;
    private void Awake()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
        Rigidbody rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (Dead) return;
        //Movement Controlls
        Vector3 Run = transform.forward * ForwardSpeed  *SpeedMultiplier* Time.fixedDeltaTime;
        Vector3 Veer = transform.right * VeerInput*SpeedMultiplier* Time.fixedDeltaTime;
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
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.S))
            Drop();
    }
    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down,0.1f, GroundMask);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, GroundMask);
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce * ForwardSpeed);
            DoubleJumpAvailable = true;
        }
        else if (hasDoubleJump&&DoubleJumpAvailable)
        {
            rb.AddForce(Vector3.up * JumpForce * ForwardSpeed);
            DoubleJumpAvailable = false;
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
    public void SpeedBoost(int duration,float BoostAmount)
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
        Debug.Log("Speed Boost Lost");
    }
    //Functions for points multiplier
    public void PointBoost(int duration, float BoostAmount)
    {
        if (PointsBoosted == true)
        {
            CancelInvoke("EndPointBoost");
            Invoke("EndPointBoost", duration);
            Debug.Log("Point Boost Extended");
        }
        else
        {
            PointsBoosted = true;
            gameEngine.PointsMultiplier = BoostAmount;
            Invoke("EndPointBoost", duration);
            Debug.Log("Point Boost Gained");
        }
    }
    void EndPointBoost()
    {
        PointsBoosted = false;
        gameEngine.PointsMultiplier = 1;
        Debug.Log("Point Boost Lost");
    }
    // Functions for shield
    public void ShieldBuff(float duration)
    {
        if (isShielded == true)
        {
            CancelInvoke("EndShieldBuff");
            Invoke("EndShieldBuff", duration);
            Debug.Log("Shield Extended");
        }
        else
        {
            isShielded = true;
            Invoke("EndShieldBuff",duration);
            Debug.Log("Shield Gained");
        }
    }
    public void EndShieldBuff()
    {
        CancelInvoke("EndShieldBuff");
        isShielded = false;
        Debug.Log("Shield Lost");
    }
    // Functions for Double Jump 
    public void DoubleJump(float duration)
    {
        if (hasDoubleJump == true)
        {
            CancelInvoke("EndDoubleJump");
            Invoke("EndDoubleJump",duration);
            Debug.Log("Double Jump Extended");
        }
        else
        {
            hasDoubleJump = true;
            DoubleJumpAvailable = true;
            Invoke("EndDoubleJump", duration);
            Debug.Log("Double Jump Gained");
        }
    }
    public void EndDoubleJump()
    {
        CancelInvoke("EndDoubleJump");
        hasDoubleJump = false;
        Debug.Log("Double Jump Lost");
    }
}
