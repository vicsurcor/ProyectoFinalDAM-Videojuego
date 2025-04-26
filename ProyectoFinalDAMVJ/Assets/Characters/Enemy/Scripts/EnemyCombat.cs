using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 80;
    public int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void PerformAttack(PlayerCombat player)
    {
        int damage = 15; // example damage value
        player.TakeDamage(damage);
        Debug.Log($"Enemy attacks! Player Health: {player.currentHealth}");
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{this.name} takes {damage} damage. Remaining Health: {currentHealth}");
    }
}
