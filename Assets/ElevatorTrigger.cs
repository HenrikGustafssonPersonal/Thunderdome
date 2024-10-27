using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerStats>() != null)
        {
            GetComponentInParent<ElevatorControls>().StartElevator();
        }
    }
}
