using UnityEngine;

public class Halo : MonoBehaviour
{
    MeshRenderer halo;
    PlayerControls playerControls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
        halo = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.PointsBoosted)
            halo.enabled = true;
        else
            halo.enabled = false;
    }
}
