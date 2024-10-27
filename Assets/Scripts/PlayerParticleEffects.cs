using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem speedLines;
    public ParticleSystem fallLines;
    public ParticleSystem slidingParticles;
    public GameObject groundSlamObject;
    public Transform playerPosition;
    private Quaternion slidingDirection;
    private void Update()
    {
        if (slidingParticles.isPlaying)
        {
            
            slidingParticles.transform.localRotation =  slidingDirection;
            

        }

       
    }
    public void PlaySpeedLineAnimation()
    {
        speedLines.Play();

    }

    public void CreateGroundSlamAnimatiion()
    {
        Vector3 offSet = new Vector3(0, 0.9f, 0);
        GameObject groundParticles = Instantiate(groundSlamObject, playerPosition.position-offSet,playerPosition.rotation);
        Destroy(groundParticles, 2f);
    }

    public void PlayFallLineAnimation()
    {
        
        if(!fallLines.isPlaying) fallLines.Play();


    }

    public void StopFallLineAnimation()
    {
        if(fallLines.isPlaying)fallLines.Stop();

    }

    public void PlaySlideParticleAnimation(Quaternion rot)
    {
        slidingDirection = new Quaternion(rot.x, rot.y, rot.z, rot.w);
        slidingParticles.Play();

    }

    public void StopSlideParticleAnimation()
    {
        if (slidingParticles.isPlaying) slidingParticles.Stop();

    }

    
}
