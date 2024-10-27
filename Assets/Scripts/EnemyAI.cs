using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform playerRef;
    protected float retreatPercent = 0.0f;

    public float decisionCD = 3.0f;
    public float currentDecisionCD = 0.0f;

    protected float distanceToAttack;

    public bool canAttack = false;
    public float attackCd = 3.0f;
    public float currentAttackCd = 0.0f;
    public bool disabledAI = true;

    public Transform LOSPos;

    public enum EnemyState {Chasing, Stopped, Retreating, Reparing, Charging, Wandering}
    public EnemyState currentState;

    public LayerMask layerIgnore = 7;

    protected Enemy enemyStats;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        distanceToAttack = agent.stoppingDistance*3f;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStats = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (disabledAI) return;

        // If cd complete make decision:
        currentDecisionCD -= Time.deltaTime;
        if(currentDecisionCD <= 0.0f)
        {
            currentState = MakeDecision();
            currentDecisionCD = decisionCD;
        }

        if (retreatPercent > 0.0f && enemyStats != null && enemyStats.currentHealth / enemyStats.maxHealth <= retreatPercent)
            currentState = EnemyState.Retreating;

        // Always preform current decision:
        switch (currentState)
        {
            case EnemyState.Chasing:
                Chasing();
                break;
            case EnemyState.Stopped:
                Stopped();
                break;
            case EnemyState.Retreating:
                Retreating();
                break;
            case EnemyState.Reparing:
                Reparing();
                break;
            case EnemyState.Charging:
                Charging();
                break;
            case EnemyState.Wandering:
                Wandering();
                break;
            default:
                break;
        }

        TryToAttack();

        if(currentAttackCd >= 0.0f)
            currentAttackCd -= Time.deltaTime;
    }

    protected virtual void Chasing()
    {
        agent.SetDestination(playerRef.position);
        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);

    }

    protected virtual void Stopped()
    {
        agent.ResetPath();
        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);

    }

    protected virtual void Retreating()
    {
        //Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z)
        Vector3 myVec = new Vector3(this.gameObject.transform.position.x, 0, this.gameObject.transform.position.z);
        Vector3 playerVec = new Vector3(playerRef.position.x, 0, playerRef.position.z);

        Vector3 moveVec = Vector3.Normalize((myVec - playerVec));;
        agent.Move(moveVec * 5.0f * Time.deltaTime);

        Vector3 lookAtvector = new Vector3(playerRef.position.x, this.gameObject.transform.position.y, playerRef.position.z);
        this.gameObject.transform.LookAt(lookAtvector);
    }

    protected virtual void Reparing()
    {

    }
    protected virtual void Charging()
    {

    }
    protected virtual void Wandering()
    {

    }

    protected virtual EnemyState MakeDecision()
    {
        if(Vector3.Distance(this.gameObject.transform.position, playerRef.position) < agent.stoppingDistance)
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

    protected virtual void TryToAttack()
    {
        if (!canAttack) return;

        Vector3 aimLoc = playerRef.gameObject.GetComponentInChildren<MouseLook>().gameObject.transform.position;
        Vector3 rayDirection = aimLoc - LOSPos.position;

        RaycastHit hit;
        if (Physics.Raycast(LOSPos.position, rayDirection, out hit, distanceToAttack, ~layerIgnore))
        {
            if (hit.transform.GetComponentInParent<PlayerStats>() != null)
            {
                // enemy can see the player!

                if(currentAttackCd <= 0.0f)
                {
                    Debug.DrawLine(LOSPos.position, hit.point, Color.green, 2, false);
                    Shoot(LOSPos.position, hit.point, GetComponent<Enemy>().damage);
                    currentAttackCd = attackCd;
                }
            }
            else
            {
                // there is something obstructing the view
            }
        }
    }

    protected virtual void Shoot(Vector3 startPos, Vector3 endPos, float damage)
    {
        Debug.Log("Pang");
    }

    public void StartAI()
    {
        disabledAI = false;
    }
}
