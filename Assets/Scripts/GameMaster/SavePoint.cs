using System;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponent<Respawn>() != null)
            other.transform.GetComponent<Respawn>().respawnPoint = transform.position;
    }
}
