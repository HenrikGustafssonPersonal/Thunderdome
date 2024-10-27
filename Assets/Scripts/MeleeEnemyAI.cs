using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine;

public class MeleeEnemyAI : EnemyAI
{
    [SerializeField]
    private GameObject laserBeam;

    [SerializeField]
    private Transform beamStart1;
    [SerializeField]
    private Transform beamStart2;

    [SerializeField]
    private float meleeDistanceToAttack = 50.0f;

    private float burstSpeed;

    public bool gettingHealed = false;

    private Animator anim;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToAttack = meleeDistanceToAttack;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        anim.SetBool("Moving", false);

        burstSpeed = GetComponent<MeleeEnemy>().burstSpeed;
        retreatPercent = GetComponent<MeleeEnemy>().retreatPercent;
    }

    protected override EnemyState MakeDecision()
    {
        if (GetComponent<Enemy>().currentHealth < GetComponent<Enemy>().maxHealth * retreatPercent)
        {
            canAttack = false;
            return EnemyState.Retreating;
        }

        if (Vector3.Distance(this.gameObject.transform.position, playerRef.position) < agent.stoppingDistance*1.1f)
        {
            canAttack = true;
            return EnemyState.Stopped;
        }
        else
        {
            canAttack = true;
            return EnemyState.Chasing;
        }
    }

    protected override void Chasing()
    {
        agent.SetDestination(playerRef.position);
        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);
        anim.SetBool("Moving", true);

    }

    protected override void Stopped()
    {
        agent.ResetPath();
        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);
        anim.SetBool("Moving", false);
    }

    protected override void Shoot(Vector3 startPos, Vector3 endPos, float damage)
    {
        for (int i = 0; i < 3; i++)
        {
            BurstBullet(startPos, endPos, damage, burstSpeed*i);
        }
    }

    private void BurstBullet(Vector3 startPos, Vector3 endPos, float damage, float burstCd)
    {
        float temp = 1f;
        DOTween.To(() => temp, x => temp = x, 100, burstCd).OnComplete(() =>
        {
            GameObject laser1 = Instantiate(laserBeam, beamStart1.position, Quaternion.LookRotation(endPos - startPos, Vector3.up));
            GameObject laser2 = Instantiate(laserBeam, beamStart2.position, Quaternion.LookRotation(endPos - startPos, Vector3.up));

            laser1.GetComponent<LaserBeam>().startPos = beamStart1.position;
            laser2.GetComponent<LaserBeam>().startPos = beamStart2.position;

            laser1.GetComponent<LaserBeam>().endPos = endPos;
            laser2.GetComponent<LaserBeam>().endPos = endPos;

            laser1.GetComponent<LaserBeam>().damage = damage;
            laser2.GetComponent<LaserBeam>().damage = damage;

        });
    }
}
