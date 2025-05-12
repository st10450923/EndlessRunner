using Unity.VisualScripting;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;
    bool Dead = false;
    //For movement calculations
    public float ForwardSpeed = 1;
    public bool PointsBoosted = false;
    public float VeerSpeed = 2;
    public float FallMultiplier = 2f;
    float VeerInput;
    public float JumpForce = 400f;
    bool isGrounded=true;
    //For Speed boost pickup
    public float SpeedMultiplier =1;
    public bool SpeedBoosted = false;
    //For shield buff
    public bool isShielded = false;

    GameEngine gameEngine;
    [Serialize] public LayerMask GroundMask;
    private void Awake()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
    }

    private void FixedUpdate()
    {
        if (Dead) return;

        //Movement Controlls
        Vector3 Run = transform.forward * ForwardSpeed  *SpeedMultiplier* Time.fixedDeltaTime;
        Vector3 Veer = transform.right * VeerInput*SpeedMultiplier* Time.fixedDeltaTime;
        rb.MovePosition(rb.position + Run + Veer);

        //Falling Modifier

        if (rb.angularVelocity.y<0)
        {
            rb.angularVelocity += Vector3.up * Physics.gravity.y * FallMultiplier * Time.deltaTime;
        }
    }
    private void Update()
    {
        if (Dead) return;
        if (transform.position.y<-2)
        {
            KillPlayer();
        }
        VeerInput = Input.GetAxis("Horizontal")*VeerSpeed;
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
    }
    void Jump()
    {
        float height = GetComponent<Collider>().bounds.size.y;
        isGrounded = Physics.Raycast(transform.position, Vector3.down,(height/2)+0.1f, GroundMask);
        Debug.DrawRay(transform.position, Vector3.down,Color.red, GroundMask);
        if (isGrounded)
            rb.AddForce(Vector3.up*JumpForce);
    }

    public void KillPlayer()
    {
        Dead = true;
        gameEngine.GameOver();
    }
    public void SpeedBoost(int duration,float BoostAmount)
    {
        if (SpeedBoosted == true)
        {
            CancelInvoke("EndSpeedBoost");
            Invoke("EndSpeedBoost", duration);
        }
        else
        {
            SpeedBoosted = true;
            SpeedMultiplier = BoostAmount;
            Invoke("EndSpeedBoost", duration);
        }
    }
    void EndSpeedBoost()
    {
        SpeedBoosted = false;
        SpeedMultiplier = 1;
    }
    //Functions for points multiplier pickup
    public void PointBoost(int duration, float BoostAmount)
    {
        if (PointsBoosted == true)
        {
            CancelInvoke("EndPointBoost");
            Invoke("EndPointBoost", duration);
        }
        else
        {
            PointsBoosted = true;
            gameEngine.PointsMultiplier = BoostAmount;
            Invoke("EndPointBoost", duration);
        }
    }
    void EndPointBoost()
    {
        PointsBoosted = false;
        gameEngine.PointsMultiplier = 1;
    }
    // Functions for shield pickup
    public void ShieldBuff(float duration)
    {
        if (isShielded == true)
        {
            CancelInvoke("EndShieldBuff");
            Invoke("EndShieldBuff", duration);
        }
        else
        {
            isShielded = true;
            Invoke("EndShieldBuff",duration);
        }
    }
    void EndShieldBuff()
    {
        isShielded = false;
    }
}
