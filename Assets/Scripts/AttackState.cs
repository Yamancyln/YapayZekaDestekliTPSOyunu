using UnityEngine;

// GDTitans youtube kanalının "Unity Third Person Game" video serisindeki "3D ENEMY AI in UNITY - (E02): CHASE AND ATTACK" adlı videodan yardım alınmıştır.
public class AttackState : StateMachineBehaviour
{
    Transform player;
    private PlayerHealth playerHealth;
    private float attackCooldown = 1.5f; // zombi başına saldırı süresi //chatgpt yardımı alındı
    private float nextAttackTime = 0f;  //chatgpt yardımı alındı
    private int damagePerHit = 10;  // bir zombinin saldırı başına verdiği hasar. 

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerHealth = player?.GetComponent<PlayerHealth>();
        nextAttackTime = Time.time;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);        
        animator.transform.LookAt(player);
        if (distance < 1.5f)
        {
            // Belirli aralıklarla saldırı yapar  //Bu kısım chatgpt yardımı ile yapıldı.
            if (Time.time >= nextAttackTime)
            {
                playerHealth.TakeDamage(damagePerHit);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        if (distance > 2)
            animator.SetBool("isAttacking", false);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
    }

    //OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
