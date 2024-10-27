using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private TutorialManager.TutorialState stateToGoFrom;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInParent<PlayerStats>() != null)
        {
            if (TutorialManager.Instance.currentState == stateToGoFrom)
            {
                if(stateToGoFrom == TutorialManager.TutorialState.waitForItemPickUp)
                    TutorialManager.Instance.ItemPickUp();

                if (stateToGoFrom == TutorialManager.TutorialState.waitForSlide)
                    TutorialManager.Instance.SlideDone();

                if (stateToGoFrom == TutorialManager.TutorialState.waitForJump)
                    TutorialManager.Instance.JumpDone();

                if (stateToGoFrom == TutorialManager.TutorialState.waitForWallJump)
                    TutorialManager.Instance.WallJumpDone();
                Destroy(this.gameObject);

            }
        }
    }
}
