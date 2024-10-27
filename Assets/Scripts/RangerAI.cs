using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System;

public class RangerAI : EnemyAI
{
    [SerializeField]
    private Transform[] pathFindingTargets;

    [SerializeField]
    private float pathfindingTargetDistance = 10.0f;

    private Animator anim;

    [SerializeField]
    private float dropTimer = 0.1f;
    [SerializeField]
    private float currentDropTimer;

    [Header("Charge Beam Settings")]
    [SerializeField]
    private float chargeCD = 3.0f;
    [SerializeField]
    private float currentChargeCD;
    public GameObject lockOnBeam;
    private LineRenderer lr;
    [SerializeField]
    private Transform lockOnBeamStartPos;
    [SerializeField]
    private GameObject chargingParticles;
    [SerializeField]
    private GameObject shootingParticles;
    [SerializeField]
    private GameObject beamHitParticles;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToAttack = 50.0f;
        agent.radius = distanceToAttack;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStats = GetComponent<Enemy>();
        anim = GetComponent<Animator>();

        // Set start values:
        currentAttackCd = attackCd;
        canAttack = true;
        currentChargeCD = chargeCD;

        lr = lockOnBeam.GetComponent<LineRenderer>();
        lr.enabled = false;
        chargingParticles.SetActive(false);
        shootingParticles.SetActive(false);
        SetupPathfindingTargets();
    }

    private void SetupPathfindingTargets()
    {
        pathFindingTargets[0].position = this.gameObject.transform.position + new Vector3(0, 0, pathfindingTargetDistance);
        pathFindingTargets[1].position = this.gameObject.transform.position + new Vector3(0, 0, -pathfindingTargetDistance);
        pathFindingTargets[2].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0,0 );
        pathFindingTargets[3].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, 0);

        pathFindingTargets[4].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0, pathfindingTargetDistance);
        pathFindingTargets[5].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0, -pathfindingTargetDistance);
        pathFindingTargets[6].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, pathfindingTargetDistance);
        pathFindingTargets[7].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, -pathfindingTargetDistance);

        pathfindingTargetDistance *= 2;
        pathFindingTargets[8].position = this.gameObject.transform.position + new Vector3(0, 0, pathfindingTargetDistance);
        pathFindingTargets[9].position = this.gameObject.transform.position + new Vector3(0, 0, -pathfindingTargetDistance);
        pathFindingTargets[10].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0, 0);
        pathFindingTargets[11].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, 0);

        pathFindingTargets[12].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0, pathfindingTargetDistance);
        pathFindingTargets[13].position = this.gameObject.transform.position + new Vector3(pathfindingTargetDistance, 0, -pathfindingTargetDistance);
        pathFindingTargets[14].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, pathfindingTargetDistance);
        pathFindingTargets[15].position = this.gameObject.transform.position + new Vector3(-pathfindingTargetDistance, 0, -pathfindingTargetDistance);
    }

    protected override EnemyState MakeDecision()
    {
        if (currentDropTimer >= 0.0f)
        {
            agent.ResetPath();
            lr.enabled = canAttack;
            chargingParticles.SetActive(canAttack);
            anim.SetBool("Charging", true);
            return EnemyState.Charging;
        }

        Vector3 aimLoc = playerRef.gameObject.GetComponentInChildren<MouseLook>().gameObject.transform.position;
        Vector3 rayDirection = aimLoc - LOSPos.position;

        RaycastHit hit;
        if (Physics.Raycast(LOSPos.transform.position, rayDirection, out hit, 10000.0f, ~layerIgnore))
        {
            Debug.DrawLine(LOSPos.transform.position, hit.point, Color.green, 2, false);
            if (hit.transform.GetComponentInParent<PlayerStats>() != null)
            {
                agent.ResetPath();
                lr.enabled = canAttack;
                chargingParticles.SetActive(canAttack);
                anim.SetBool("Charging", true);
                return EnemyState.Charging;
            }
            else if (currentChargeCD != chargeCD)
            {
                currentChargeCD = chargeCD;
            }
        }

        if (Vector3.Distance(this.gameObject.transform.position, playerRef.position) != distanceToAttack)
        {
            lr.enabled = false;
            chargingParticles.SetActive(false);
            anim.SetBool("Charging", false);
            return EnemyState.Wandering;
        }
        else
        {
            agent.ResetPath();
            lr.enabled = false;
            chargingParticles.SetActive(false);
            anim.SetBool("Charging", false);
            return EnemyState.Stopped;
        }

    }

    protected override void Charging()
    {
        if (!canAttack) return;

        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);

        Vector3 aimLoc = playerRef.gameObject.GetComponentInChildren<MouseLook>().gameObject.transform.position;
        Vector3 rayDirection = aimLoc - LOSPos.position;

        RaycastHit hit;
        if (Physics.Raycast(LOSPos.transform.position, rayDirection, out hit, 10000.0f, ~layerIgnore))
        {
            Debug.DrawLine(LOSPos.transform.position, hit.point, Color.green, 2, false);

            // If hitting player, reset dropCD:
            if (hit.transform.GetComponentInParent<PlayerStats>() != null)
                currentDropTimer = dropTimer;

            // Currently have droptimer:
            if (currentDropTimer >= 0.0f)
            {
                currentChargeCD -= Time.deltaTime;
                if (currentChargeCD <= 0.0F)
                    Shoot(LOSPos.transform.position, hit.point, enemyStats.damage);

                currentDropTimer -= Time.deltaTime;
            }
            else if (currentChargeCD != chargeCD)
            {
                currentChargeCD = chargeCD;
                MakeDecision();
            }   
            
        }
        // Display Beam:
        RenderBeamLine(aimLoc);
    }

    protected override void Wandering()
    {
        if (agent.hasPath)
            return;

        Vector3 aimLoc = playerRef.gameObject.GetComponentInChildren<MouseLook>().gameObject.transform.position;

        List<Transform> playerHits = new List<Transform>();
        List<Transform> otherHits = new List<Transform>();

        foreach (Transform target in pathFindingTargets)
        {
            Vector3 rayDirection = aimLoc - target.position;
            RaycastHit hit;
            if (Physics.Raycast(target.position, rayDirection, out hit, 10000.0f, ~layerIgnore))
            {
                Debug.DrawLine(target.position, hit.point, Color.blue, 2, false);
                if (hit.transform.GetComponentInParent<PlayerStats>() != null)
                    playerHits.Add(target);
                else
                    otherHits.Add(target);
            }
        }

        int choosenTargetIndex = 0;

        if (playerHits.Count > 0)
        {
            // Check all player hits until valid path is found:
            while (playerHits.Count > 0)
            {
                float smallestDist = 10000000.0f;
                // This loop finds the smallest of att playerhits:
                for (int i = 0; i < playerHits.Count; i++)
                {

                    float distance = (DistanceDifference(playerHits[i].position, aimLoc));
                    if (distance < smallestDist)
                    {
                        smallestDist = distance;
                        choosenTargetIndex = i;
                    }
                }
                float randomDiff = Random.Range(0.9f, 1.1f);
                if (CanPath(playerHits[choosenTargetIndex].position* randomDiff))
                {

                    agent.SetDestination(playerHits[choosenTargetIndex].position * randomDiff);
                    return;
                }
                playerHits.Remove(playerHits[choosenTargetIndex]);
            }
            // NO PATH FOUND:
            Debug.Log("NO PATH FOUND");


 
        }
        else
        {
            // Check all player hits until valid path is found:
            while (otherHits.Count > 0)
            {
                float smallestDist = 10000000.0f;
                // This loop finds the smallest of at otherHits:
                for (int i = 0; i < otherHits.Count; i++)
                {

                    float distance = (DistanceDifference(otherHits[i].position, aimLoc));
                    if (distance < smallestDist)
                    {
                        smallestDist = distance;
                        choosenTargetIndex = i;
                    }
                }
                float randomDiff = Random.Range(0.9f, 1.1f);
                if (CanPath(otherHits[choosenTargetIndex].position * randomDiff))
                {
                    agent.SetDestination(otherHits[choosenTargetIndex].position * randomDiff);
                    Debug.Log(otherHits[choosenTargetIndex]);
                    return;
                }
                otherHits.Remove(otherHits[choosenTargetIndex]);
            }
            // NO PATH FOUND:
            Debug.Log("NO PATH FOUND");
        }
    }

    private void RenderBeamLine( Vector3 endPos)
    {
        endPos -= new Vector3(0f, .2f, 0f);
        lr.SetPosition(0, lockOnBeamStartPos.position);
        lr.SetPosition(1, endPos);

        lr.material.SetColor("_Color", new Color(1f, (currentChargeCD / chargeCD), 1f, 1.0f-(currentChargeCD/chargeCD)));
        lr.widthMultiplier = 0.001f + currentChargeCD/40.0f;
    }
    private float DistanceDifference(Vector3 targetPos, Vector3 playerPos)
    {
        float targetPlayerDistance = Vector3.Distance(playerPos, targetPos);
        return Mathf.Abs(targetPlayerDistance - distanceToAttack);
    }

    private bool CanPath(Vector3 targetPos)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        //create path and check if it can be done
        // and check if navMeshAgent can reach its target
        if (agent.CalculatePath(targetPos, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
        {
            //move to target
            //agent.SetPath(navMeshPath);
            return true;
        }
        else
        {
            //Fail condition here
            return false;
        }
    }

    protected override void TryToAttack()
    {
        if (currentAttackCd <= 0.0f)
        {
            canAttack = true;
        }
        else
            canAttack = false;
    }
    protected override void Shoot(Vector3 startPos, Vector3 endPos, float damage)
    {
        Debug.Log("Pang");
        GameManager.instance.playerDamage(GetComponent<Enemy>().damage);
        GameManager.instance.drawDamageArrow(startPos);

        // Set shooting layer to max:
        anim.SetLayerWeight(1, 1);
        //Restart Animation:
        anim.Play("ResetFire", 1);

        // Shooting particles:
        shootingParticles.SetActive(true);
        ParticleSystem[] ps = shootingParticles.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in ps)
        {
            p.Play();
        }

        // Hit particles:
        GameObject tempParticles = Instantiate(beamHitParticles, endPos, beamHitParticles.transform.rotation);
        Destroy(tempParticles, 1.0f);


        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, 0.8f).OnComplete(() =>
        {
            anim.SetLayerWeight(1, 0);
            shootingParticles.SetActive(false);
            
        });



        currentChargeCD = chargeCD;
        canAttack = false;
        currentAttackCd = attackCd;
    }
}
