using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTeleport : MonoBehaviour
{
    [SerializeField]
    private Transform teleportPosition;

    private GameObject player;
    private void Start()
    {
       player = GameObject.FindGameObjectWithTag("Player");
 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponentInParent<PlayerStats>() != null)
        {
            Debug.Log("Teleport player to"); 
            CharacterController cc = player.GetComponent<CharacterController>();

            cc.enabled = false;
            player.transform.position = teleportPosition.position;
            player.transform.rotation = teleportPosition.rotation;
            cc.enabled = true;
        }
    }
}
