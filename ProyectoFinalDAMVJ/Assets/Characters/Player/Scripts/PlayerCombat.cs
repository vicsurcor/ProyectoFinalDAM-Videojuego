using TMPro;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool actionCompleted; // Flag to sync with the CombatManager
    public bool isDefending = false;

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
        if (isDefending == false)
        {
            currentHealth -= damage;
            battlelog.text += $"\nPlayer takes {damage} damage. Remaining Health: {currentHealth}";
            Debug.Log($"Player takes {damage} damage. Remaining Health: {currentHealth}");
        }
        else if (isDefending == true)
        {
            currentHealth -= damage/2;
            battlelog.text += $"\nPlayer takes {damage/2} damage while blocking. Remaining Health: {currentHealth}";
            Debug.Log($"Player takes {damage} damage while blocking. Remaining Health: {currentHealth}");
        }
        
    }

    public void Defend (TextMeshProUGUI battlelog)
    {
        if (isDefending == false)
        {
            battlelog.text += $"\nPlayer blocking.";
            Debug.Log($"Player blocking.");
            isDefending = true;
            actionCompleted = true;
        }
        else if (isDefending == true)
        {
            isDefending = false;
        }
        
    }
}
