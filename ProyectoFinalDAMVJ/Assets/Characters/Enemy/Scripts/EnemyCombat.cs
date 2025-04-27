using TMPro;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public int maxHealth = 80;
    public int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void PerformAttack(PlayerCombat player, TextMeshProUGUI battlelog)
    {
        int damage = 15; // example damage value
        player.TakeDamage(damage, battlelog);
        Debug.Log($"Enemy attacks! Player Health: {player.currentHealth}");
    }
    
    public void TakeDamage(int damage, TextMeshProUGUI battlelog)
    {
        currentHealth -= damage;
        battlelog.text += $"\n{this.name} takes {damage} damage. Remaining Health: {currentHealth}";
        Debug.Log($"{this.name} takes {damage} damage. Remaining Health: {currentHealth}");
    }
}
