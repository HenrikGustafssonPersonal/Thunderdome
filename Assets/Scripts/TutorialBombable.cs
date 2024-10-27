using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBombable : Hitable
{
    public override void Destroy()
    {
        TutorialManager.Instance.BombDone();
        Destroy(this.gameObject, 0.1f);
    }
}
