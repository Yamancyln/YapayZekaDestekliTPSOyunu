using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;

    public float attackDistance = 2.0f; // Saldırıya geçme mesafesi
    public float lookSpeed = 5f;        // Karaktere bakma hızı

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackDistance)
        {
            // Hedefe yürü
            agent.isStopped = false;
            agent.SetDestination(target.position);

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            // Dur ve saldır
            agent.isStopped = true;

            // Karaktere dön
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * lookSpeed);

            if (animator != null)
            {
                animator.SetBool("isWalking", false);
                animator.SetTrigger("attack");
            }
        }
    }
}


