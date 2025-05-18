using UnityEngine;

public class BootsMesh : MonoBehaviour
{
    PlayerControls playerControls;
    MeshRenderer Boot;
    public Material Base;
    public Material Gold;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
        Boot = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.hasDoubleJump)
            Boot.material = Gold;
        else
            Boot.material = Base;
    }
}
