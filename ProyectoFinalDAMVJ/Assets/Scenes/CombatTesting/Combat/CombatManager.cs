using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatManager : MonoBehaviour
{
    public enum CombatState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }
    public CombatState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    
    private PlayerCombat player;
    private EnemyCombat enemy;
    private Transform playerStart;
    private List<Transform>  enemiesStart;

    void Start()
    {
        state = CombatState.START;
        playerStart = FindObjectsOfType<Transform>().Where(t => t.name == "PlayerLocation").First();
        enemiesStart = FindObjectsOfType<Transform>().Where(t => t.name == "EnemyLocation").OrderBy(t => t.position.x).ToList();
    
        SetupBattle();
    }

    void SetupBattle()
    {
        int enemiesSpawned = 0;
        // Instantiate player and enemy at predetermined positions.
        player = Instantiate(playerPrefab, playerStart.position + new Vector3(0,5,0), Quaternion.identity).GetComponent<PlayerCombat>();
        while (enemiesSpawned < numberOfEnemies)
        {
            enemy = Instantiate(enemyPrefab, enemiesStart[enemiesSpawned].position + new Vector3(0,5,0), Quaternion.identity).GetComponent<EnemyCombat>();
            enemiesSpawned++;
        }
        

        // Start the turn sequence.
        StartCoroutine(CombatSequence());
    }

    IEnumerator CombatSequence()
    {
        // Transition to player's turn after setup.
        state = CombatState.PLAYER_TURN;
        while (state != CombatState.WON && state != CombatState.LOST)
        {
            switch (state)
            {
                case CombatState.PLAYER_TURN:
                    // Wait for player action (e.g., via UI buttons, input handling).
                    yield return StartCoroutine(PlayerTurn());
                    // After the player's action, check enemy health.
                    if (enemy.currentHealth <= 0)
                    {
                        state = CombatState.WON;
                        break;
                    }
                    state = CombatState.ENEMY_TURN;
                    break;

                case CombatState.ENEMY_TURN:
                    // Execute enemy logic.
                    yield return StartCoroutine(EnemyTurn());
                    // After enemy action, check player health.
                    if (player.currentHealth <= 0)
                    {
                        state = CombatState.LOST;
                        break;
                    }
                    state = CombatState.PLAYER_TURN;
                    break;
            }
            yield return null;
        }

        // Handle end-of-battle logic.
        EndBattle();
    }

    IEnumerator PlayerTurn()
    {
        // Here you’d bring up your UI for the player to select an action.
        // For example, wait until the player selects “Attack” or “Defend”.
        //yield return new WaitUntil(() => player.actionCompleted);
        yield return new WaitForSeconds(0.5f);
        player.InstantKill(enemy);
        // Optionally, process player input results here (damage calculations, animations, etc.)
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator EnemyTurn()
    {
        // Simple enemy logic: attack after a short delay.
        yield return new WaitForSeconds(1f);
        enemy.PerformAttack(player);
        yield return new WaitForSeconds(0.5f);
    }

    void EndBattle()
    {
        if (state == CombatState.WON)
        {
            Debug.Log("Victory!");
            // Trigger victory animations, sounds, or transitions.
        }
        else if (state == CombatState.LOST)
        {
            Debug.Log("Defeat!");
            // Trigger defeat logic.
        }
    }
}
