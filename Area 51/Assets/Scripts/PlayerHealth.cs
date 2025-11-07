using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DamageEvent : UnityEvent<int> { }

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
    public DamageEvent onTakeDamage; // NEW: Enemy can trigger damage

    [Header("UI")]
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay;

        healthBar?.SetMaxHealth(maxHealth);
        healthBar?.SetHealth(currentHealth);

        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Update()
    {
        if (currentHealth > 0 && Time.time - lastDamageTime >= regenDelay)
        {
            int regenAmount = Mathf.RoundToInt(regenRate * Time.deltaTime);
            if (regenAmount > 0)
            {
                currentHealth += regenAmount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

                healthBar?.SetHealth(currentHealth);
                onHealthChanged?.Invoke(currentHealth, maxHealth);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        lastDamageTime = Time.time;
        healthBar?.SetHealth(currentHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0) Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthBar?.SetHealth(currentHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
        onDeath?.Invoke();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
