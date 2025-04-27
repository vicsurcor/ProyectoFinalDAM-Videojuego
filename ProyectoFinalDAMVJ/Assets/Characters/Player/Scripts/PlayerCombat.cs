using TMPro;
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

    public void Attack(EnemyCombat enemy, TextMeshProUGUI battlelog)
    {
        // Implement your damage logic here.
        int damage = 20; // example damage value
        enemy.TakeDamage(damage, battlelog);
        // Mark action as complete so the CombatManager can continue.
        actionCompleted = true;
    }

    public void InstantKill(EnemyCombat enemy, TextMeshProUGUI battlelog)
    {
        int damage = 100;
        enemy.TakeDamage(damage, battlelog);

        actionCompleted = true;
    }
    
    public void TakeDamage(int damage, TextMeshProUGUI battlelog)
    {
        currentHealth -= damage;
        battlelog.text += $"\nPlayer takes {damage} damage. Remaining Health: {currentHealth}";
        Debug.Log($"Player takes {damage} damage. Remaining Health: {currentHealth}");
    }
}
