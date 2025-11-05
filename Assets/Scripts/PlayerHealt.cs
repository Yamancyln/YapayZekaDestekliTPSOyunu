using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;
    public Animator animator;
    public Slider healthBar;

    void Update()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Player Öldü!");
        }
        else
        {
            animator.SetTrigger("Player damage");
        }
    }
}
