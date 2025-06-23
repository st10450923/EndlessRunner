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
                Destroy(gameObject);
                playerControls.EndShieldBuff();
                SFXManager.Inst.PlaySFX(SFXManager.Inst.PlayerBlock);
            }
            else
            {
                SFXManager.Inst.PlaySFX(SFXManager.Inst.PlayerHit);
                playerControls.KillPlayer();
            }
        }
        
    }
}
