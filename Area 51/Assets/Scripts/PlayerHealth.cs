using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Regeneration")]
    public float regenRate = 3f;         
    public float regenDelay = 2f;        
    private float lastDamageTime;

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged;
    public HealthBar healthBar;
    void Awake()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay; 
    }

    void Update()
    {
     
        if (currentHealth > 0 && Time.time - lastDamageTime >= regenDelay)
        {
            currentHealth += Mathf.RoundToInt(regenRate * Time.deltaTime);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

   
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastDamageTime = Time.time; 
        onHealthChanged?.Invoke(currentHealth, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
