using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    //View bobbing depending on players speed and state
    //states: Walking, In air
    
    public float effectIntensity;
    public float effectIntensityX;
    public float effectSpeed;
    public PositionFollower followerInstance;
    private Vector3 originalOffset;
    private float SinTime;

    //Adjust squished arms when crouching
    public PlayerMovementScript player;

    void Start()
    {
    
        originalOffset = followerInstance.Offset;

    }

    void Update()
    {
        Vector3 scaleTmp = transform.localScale;


        scaleTmp = new Vector3((transform.localScale.x / player.transform.localScale.x),
                                           (1/ player.transform.localScale.y),
                                           (1 / player.transform.localScale.z));

        transform.localScale = scaleTmp;

        //Get current movement state from game manager

        BobbingEffect();

    }
    public void BobbingEffect()
    {
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"),0f,Input.GetAxis("Horizontal"));
        if (inputVector.magnitude > 0f && player.state.ToString() == "running")
        {
            SinTime += Time.deltaTime * effectSpeed;
            
        }
        else if(player.state.ToString() == "falling")
        {

        }
        else
        {

        }

        float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(SinTime));
        Vector3 sinAmountX = followerInstance.transform.right * effectIntensity * Mathf.Cos(SinTime) * effectIntensityX;

        followerInstance.Offset = new Vector3
        {
            x = originalOffset.x,
            y = originalOffset.y + sinAmountY,
            z = originalOffset.z
        };

        followerInstance.Offset += sinAmountX;
    }
}
