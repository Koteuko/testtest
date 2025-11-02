using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Vector3 respawnPoint;
    
    void Start()
    {
        if(respawnPoint == Vector3.zero)
            respawnPoint = transform.position;
    }

    void Update()
    {
        if(transform.position.y < -10f)
            transform.position = respawnPoint;
    }
}
