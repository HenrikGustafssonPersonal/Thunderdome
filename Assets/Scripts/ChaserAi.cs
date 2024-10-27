using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ChaserAi : EnemyAI
{
    [SerializeField]
    private float knockBackAmount = 30f;

    private Animator anim;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToAttack = agent.stoppingDistance;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStats = GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        anim.SetBool("Moving", false);
    }

    protected override EnemyState MakeDecision()
    {
        if (Vector3.Distance(this.gameObject.transform.position, playerRef.position) < distanceToAttack)
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
        Debug.Log("Sliced!");
        //Apply knockback in direction enemy is attacking
        GameManager.instance.playerKnockBack(endPos - startPos, knockBackAmount);
        GameManager.instance.playerDamage(damage);
        GameManager.instance.drawDamageArrow(startPos);


    }
}
