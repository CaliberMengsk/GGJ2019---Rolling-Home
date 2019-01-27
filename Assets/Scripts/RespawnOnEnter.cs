using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnEnter : MonoBehaviour
{
    public Level level;
    private void OnTriggerEnter(Collider other)
    {
        Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Respawn();
    }

    public void Respawn()
    {
        level.Respawn();
    }
}
