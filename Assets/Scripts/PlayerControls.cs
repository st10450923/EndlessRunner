using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rb;
    public float ForwardSpeed = 1;
    float VeerInput;
    bool Dead = false;
    float SpeedMultiplier=1;
    public float VeerSpeed = 2;
    public bool SpeedBoosted = false;

    GameEngine gameEngine;
    public float JumpForce = 400f;
    [Serialize] public LayerMask GroundMask;
    private void Awake()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
    }

    private void FixedUpdate()
    {
        if (Dead) return;
        Vector3 Run = transform.forward * ForwardSpeed  *SpeedMultiplier* Time.fixedDeltaTime;
        Vector3 Veer = transform.right * VeerInput*SpeedMultiplier* Time.fixedDeltaTime;
        rb.MovePosition(rb.position + Run + Veer);
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
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down,(height/2)+0.1f, GroundMask);
        Debug.DrawRay(transform.position, Vector3.down,Color.red, GroundMask);
        if (isGrounded)
            rb.AddForce(Vector3.up*JumpForce);
    }

    public void KillPlayer()
    {
        Dead = true;
        gameEngine.GameOver();
    }
    public void SpeedBoost(int Duration,float BoostAmount)
    {
        if (SpeedBoosted == true)
        {
            CancelInvoke("EndSpeedBoost");
            SpeedMultiplier = BoostAmount;
            Invoke("EndSpeedBoost", Duration);
        }
        else
        {
            SpeedBoosted = true;
            SpeedMultiplier = BoostAmount;
            Invoke("EndSpeedBoost", Duration);
        }
    }
    void EndSpeedBoost()
    {
        SpeedBoosted = false;
        SpeedMultiplier = 1;
    }
}
