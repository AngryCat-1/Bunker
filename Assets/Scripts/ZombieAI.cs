using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float rotationSpeed = 3f;
    public float attackCooldown = 2f; // Время задержки после атаки
    public float damage = 15;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public bool isAttacking = false; // Флаг, указывающий, что зомби атакует
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
            // Если зомби атакует, не обрабатываем движение
            return;
        }

        // Добавьте вашу логику атаки здесь, например, при определенных условиях
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if ((distanceToPlayer <= detectionRange || isTakenDamage)&& !isAttacking)
        {
            // Устанавливаем целью игрока
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = moveSpeed;

            // Поворачиваем зомби к игроку
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Запускаем анимацию движения
            animator.SetBool("IsWalking", true);
            animator.Play("walk");
        }
        else
        {
            // Если игрок слишком далеко, зомби прекращает движение
            navMeshAgent.speed = 0;
            animator.SetBool("IsWalking", false);
        }
    }

    private void Attack()
    {
        // Запускаем анимацию атаки
        
        print("ATTACK");

        // Останавливаем движение и устанавливаем флаг атаки
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
        isAttacking = true;

        // Начинаем задержку перед возобновлением движения
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
