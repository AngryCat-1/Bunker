using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearEnemy : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 5.0f;
    public float chaseDistance = 100.0f;
    public float attackRadius = 2.6f;
    public float attackCooldown = 1.0f;

    private int currentPatrolPoint = 0;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float attackCooldownTimer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetNextPatrolPoint();
        attackCooldownTimer = attackCooldown;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance && FindObjectOfType<PlayerHealth>().currentHealth > 0)
        {
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(player.position);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }
        else
        {
            navMeshAgent.speed = patrolSpeed;

            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                SetNextPatrolPoint();
            }

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }

        // Обработка атаки медведя
        if (distanceToPlayer <= attackRadius)
        {
            Attack();
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        // Управление временем охлаждения атаки
        attackCooldownTimer -= Time.deltaTime;
    }

    private void SetNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        navMeshAgent.SetDestination(patrolPoints[currentPatrolPoint].position);
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    private void Attack()
    {
        if (attackCooldownTimer <= 0)
        {
            FindObjectOfType<PlayerHealth>().TakeDamage(200);
            attackCooldownTimer = attackCooldown;
        }

        animator.SetBool("isAttacking", true);
    }
}
