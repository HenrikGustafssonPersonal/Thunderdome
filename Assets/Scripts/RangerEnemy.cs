using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerEnemy : Enemy
{
    public override void Destroy()
    {
        GetComponent<RangerAI>().lockOnBeam.SetActive(false);
        base.Destroy();
    }
}
