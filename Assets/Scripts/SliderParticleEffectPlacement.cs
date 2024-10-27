using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderParticleEffectPlacement : MonoBehaviour
{
    //Used to place sliding particles in players position, with its own rotation.

    private Transform playerPosition;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3 (playerPosition.position.x, transform.position.y, playerPosition.position.z);
     }
}
