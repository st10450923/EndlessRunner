using UnityEngine;

public class ShieldMesh : MonoBehaviour
{
    PlayerControls playerControls;
    MeshRenderer Shield;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
        Shield = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.isShielded)
            Shield.enabled = true;
        else
            Shield.enabled = false;
    }
}
