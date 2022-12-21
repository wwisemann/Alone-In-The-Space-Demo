using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public int enemyHealt = 200;

    //Navmesh 
    public NavMeshAgent enemyAgent;
    public Transform player;
    public LayerMask enemyGroundLayer;
    public LayerMask playerGroundLayer;

    //Patrolling(Devriye)
    public Vector3 walkPoint;
    public float walkPointRange;
    public bool walkPointSet;

    //Range
    public float sightRange, attackRange;
    public bool enemySightRange, enemyAttackRange;

    //Attack
    public float attackDelay;
    public bool isAttacking;
    public Transform attackPoint;
    public GameObject projectile;
    public float projectileForce = 18f;
    public Animator enemyAnimator;
    private GameManager gameManager;

    //Effects
    public ParticleSystem deadEffect;
    public AudioSource enemyDeadSound;

    void Start()
    {
        enemyAgent= GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        enemySightRange = Physics.CheckSphere(transform.position, sightRange, playerGroundLayer);
        enemyAttackRange = Physics.CheckSphere(transform.position, attackRange, playerGroundLayer);

        if(!enemySightRange && !enemyAttackRange)
        {
            Patrolling();
            enemyAnimator.SetBool("Patrolling", true);
            enemyAnimator.SetBool("EnemyAttack", false);
            enemyAnimator.SetBool("PlayerDetected", false);
        }
        else if(enemySightRange && !enemyAttackRange)
        {
            DetectPlayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("EnemyAttack", false);
            enemyAnimator.SetBool("PlayerDetected", true);
        }
        else if(enemySightRange && enemyAttackRange)
        {
            AttackPlayer();
            enemyAnimator.SetBool("Patrolling", false);
            enemyAnimator.SetBool("EnemyAttack", true);
            enemyAnimator.SetBool("PlayerDetected", false);
        }
    }

    void Patrolling()
    {
        if(walkPointSet == false)
        {
            float randomZPos = Random.Range(-walkPointRange, walkPointRange);
            float randomXPos = Random.Range(-walkPointRange, walkPointRange);

            walkPoint = new Vector3(transform.position.x + randomXPos, transform.position.y , transform.position.z + randomZPos);

            if (Physics.Raycast(walkPoint, -transform.up, 2f, enemyGroundLayer))
            {
                walkPointSet = true;
            }
        }

        if (walkPointSet == true)
        {
            enemyAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

    }


    void DetectPlayer()
    {
        enemyAgent.SetDestination(player.position);
        transform.LookAt(player);
    }

    void AttackPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(player);

         
        if(isAttacking == false)
        {
            Rigidbody rb = Instantiate(projectile,attackPoint.position,Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            isAttacking = true;
            Invoke("ResetAttack", attackDelay);
        }

    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public void EnemyTakeDamage(int DamageAmount)
    {
        enemyHealt -= DamageAmount;
        

        if (enemyHealt <= 0)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddKill();
        enemyDeadSound.Play();
        
        Instantiate(deadEffect, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color= Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
