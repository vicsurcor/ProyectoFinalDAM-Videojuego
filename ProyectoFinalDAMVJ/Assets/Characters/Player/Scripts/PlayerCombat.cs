using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool actionCompleted; // Flag to sync with the CombatManager

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Attack(EnemyCombat enemy)
    {
        // Implement your damage logic here.
        int damage = 20; // example damage value
        enemy.TakeDamage(damage);
        // Mark action as complete so the CombatManager can continue.
        actionCompleted = true;
    }

    public void InstantKill(EnemyCombat enemy)
    {
        int damage = 100;
        enemy.TakeDamage(damage);

        actionCompleted = true;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player takes {damage} damage. Remaining Health: {currentHealth}");
    }
}
