using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;               // Oyuncu
    private NavMeshAgent agent;            // Yürüme sistemi
    private Animator animator;             // Animasyon kontrolü
    private Health playerHealth;           // Oyuncunun can sistemi

    public float attackDistance = 2.0f;    // Ne kadar yakında saldırı başlar
    public float lookSpeed = 5f;           // Dönme hızı
    public float attackDamage = 10f;       // Vurduğunda vereceği hasar
    public float attackCooldown = 1.5f;    // Saldırı bekleme süresi
    private float lastAttackTime = 0f;     // Son saldırı zamanı

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (target != null)
            playerHealth = target.GetComponent<Health>();
    }

    void Update()
    {
        if (target == null || agent == null)
            return;

        // Oyuncuya olan mesafeyi hesapla
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            // Oyuncuya doğru yürü
            agent.isStopped = false;
            agent.SetDestination(target.position);

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            // Yeterince yaklaştıysa dur ve saldır
            agent.isStopped = true;
            animator.SetBool("isWalking", false);

            // Oyuncuya dön
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookPos),
                Time.deltaTime * lookSpeed
            );

            // Saldırı aralığı kontrolü
            if (Time.time - lastAttackTime > attackCooldown)
            {
                animator.SetTrigger("attack");

                // Oyuncunun canını azalt
                if (playerHealth != null)
                    playerHealth.TakeDamage(attackDamage);

                lastAttackTime = Time.time;
            }
        }
    }
}
