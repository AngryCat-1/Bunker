using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;
    public float attackCooldown = 2f; // ����� �������� ����� �����
    public float damage = 15;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public bool isAttacking = false; // ����, �����������, ��� ����� �������
    public bool isTakenDamage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GetComponent<ZombieHealth>().currentHealth < GetComponent<ZombieHealth>().maxHealth) { isTakenDamage = true; }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isAttacking)
        {
            // ���� ����� �������, �� ������������ ��������
            return;
        }

        // �������� ���� ������ ����� �����, ��������, ��� ������������ ��������
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if ((distanceToPlayer <= detectionRange || isTakenDamage)&& !isAttacking)
        {
            // ������������� ����� ������
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = moveSpeed;

            // ������������ ����� � ������
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // ��������� �������� ��������
            animator.SetBool("IsWalking", true);
            animator.Play("walk");
        }
        else
        {
            // ���� ����� ������� ������, ����� ���������� ��������
            navMeshAgent.speed = 0;
            animator.SetBool("IsWalking", false);
        }
    }

    private void Attack()
    {
        // ��������� �������� �����
        
        print("ATTACK");

        // ������������� �������� � ������������� ���� �����
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
        isAttacking = true;

        // �������� �������� ����� �������������� ��������
        StartCoroutine(AttackCooldown());
        
        animator.Play("attack");
    }

    private IEnumerator AttackCooldown()
    {
        

        yield return new WaitForSeconds(attackCooldown / 10 * 6);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange * 2)
        {
            FindObjectOfType<PlayerHealth>().TakeDamage(damage);
            

          
        }
        yield return new WaitForSeconds(attackCooldown / 10 * 4);
        isAttacking = false;
        navMeshAgent.isStopped = false;



    }
}
