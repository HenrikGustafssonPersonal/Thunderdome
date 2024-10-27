using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine;

public class FlyerAI : EnemyAI
{
    [SerializeField]
    private GameObject laserBeam;

    [SerializeField]
    private float healingFactor = 3.0f;

    [SerializeField]
    private GameObject repairingBeam;

    [SerializeField]
    private Transform beamStart1;
    [SerializeField]
    private Transform beamStart2;

    [SerializeField]
    private float flyerDistanceToAttack = 10.0f;


    public float healingCD = 0.2f;
    public float currentHealingCD = 0.0f;

    public bool isHealing = false;

    private Transform posTarget;

    public GameObject reparingTarget;

    private bool leftGun = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToAttack = flyerDistanceToAttack;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        posTarget = GameObject.FindGameObjectWithTag("BehindPlayer").transform;
        repairingBeam.SetActive(false);
    }
    protected override EnemyState MakeDecision()
    {
        if(currentState == EnemyState.Reparing && reparingTarget != null)
        {
            // If flyer is repairing:
            if(reparingTarget.GetComponent<MeleeEnemy>().currentHealth < reparingTarget.GetComponent<MeleeEnemy>().maxHealth)
                return EnemyState.Reparing;

            // The flyer is not repairing:
            if(reparingTarget != null)
            {
                reparingTarget.GetComponent<MeleeEnemyAI>().gettingHealed = false;
                reparingTarget.GetComponent<MeleeEnemy>().setHealingParticles(false);
                reparingTarget = null;
            }

            repairingBeam.SetActive(false);

        }

        GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> injuredMeleeEnemies = new List<GameObject>();
        foreach (GameObject g in enemies)
        {
            if (g.GetComponent<MeleeEnemy>() != null)
                if(g.GetComponent<MeleeEnemy>().currentHealth <= g.GetComponent<MeleeEnemy>().maxHealth * g.GetComponent<MeleeEnemy>().retreatPercent)
                    if(g.GetComponent<MeleeEnemyAI>().gettingHealed == false)
                        injuredMeleeEnemies.Add(g);
        }


        if (injuredMeleeEnemies.Count > 0)
        {
            reparingTarget = injuredMeleeEnemies[0];
            reparingTarget.GetComponent<MeleeEnemyAI>().gettingHealed = true;
            canAttack = false;
            return EnemyState.Reparing;
        }
        else
        {
            canAttack = true;
            return EnemyState.Chasing;
        }

    }
    protected override void Reparing()
    {
        // Check if this enemy dead:
        if(GetComponent<FlyerEnemy>().currentHealth <= 0.0f)
        {
            repairingBeam.SetActive(false);
            reparingTarget.GetComponent<MeleeEnemyAI>().gettingHealed = false;
            reparingTarget.GetComponent<MeleeEnemy>().setHealingParticles(false);
            return;
        }
        else if (reparingTarget == null || reparingTarget.GetComponent<MeleeEnemy>().currentHealth <= 0.0f )
        {
            if (reparingTarget != null)
                reparingTarget.GetComponent<MeleeEnemy>().setHealingParticles(false);

            repairingBeam.SetActive(false);
            reparingTarget = null;


            return;
        }
        // Travel to target:
        Vector3 targetPos = reparingTarget.GetComponent<MeleeEnemy>().repairingPos.position;
        agent.SetDestination(targetPos);

        // Look at target:
        Vector3 lookAtvector = new Vector3(reparingTarget.transform.position.x, this.transform.position.y, reparingTarget.transform.position.z);
        this.gameObject.transform.LookAt(lookAtvector);

        // If in range:
        Vector3 aimTarget = reparingTarget.GetComponent<MeleeEnemy>().repairBeamTarget.transform.position;
        if (Vector3.Distance(this.transform.position, aimTarget) <= distanceToAttack)
        {
            repairingBeam.SetActive(true);
            reparingTarget.GetComponent<MeleeEnemy>().setHealingParticles(true);
            // Shoot healing beam at traget:
            LineRenderer lr = repairingBeam.GetComponent<LineRenderer>();
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, aimTarget);

            // Heal traget:
            if (currentHealingCD <= 0.0f)
            {
                reparingTarget.GetComponent<MeleeEnemy>().TakeDamge(-healingFactor);
                currentHealingCD = healingCD;
            }
        }


        if (currentHealingCD > 0.0f)
            currentHealingCD -= Time.deltaTime;

    }

    protected override void Chasing()
    {
        agent.SetDestination(posTarget.position);
        Vector3 lookAtvector = new Vector3(playerRef.position.x, playerRef.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);

    }

    protected override void Shoot(Vector3 startPos, Vector3 endPos, float damage)
    {
        Transform leftOrRight;
        if (leftGun)
            leftOrRight = beamStart1;
        else
            leftOrRight = beamStart2;
        leftGun = !leftGun;

        GameObject laser1 = Instantiate(laserBeam, leftOrRight.position, Quaternion.LookRotation(endPos - startPos, Vector3.up));

        laser1.GetComponent<LaserBeam>().startPos = leftOrRight.position;

        laser1.GetComponent<LaserBeam>().endPos = endPos;

        laser1.GetComponent<LaserBeam>().damage = damage;
    }

}
