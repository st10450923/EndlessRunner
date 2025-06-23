using UnityEngine;

public class Obstacle : MonoBehaviour
{

    PlayerControls playerControls;
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (playerControls.isShielded == true)
            {
                SFXManager.Inst.PlaySFX(SFXManager.Inst.PlayerBlock,1);
                Destroy(gameObject);
                playerControls.EndShieldBuff();
            }
            else
            {
                SFXManager.Inst.PlaySFX(SFXManager.Inst.PlayerHit,1);
                playerControls.KillPlayer();
            }
        }
        
    }
}
