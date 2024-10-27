using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNextWaveScript : Hitable
{

    //Only use for trigger item starting next round
    public override void TakeDamge(float amout)
    {
        //Play animation?

        GameManager.instance.startNextRound();
    }

}
