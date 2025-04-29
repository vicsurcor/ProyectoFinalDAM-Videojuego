using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CombatManager : MonoBehaviour
{
    public enum CombatState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }
    public CombatState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public int numberOfEnemies;
    public Button attackButton;
    public Button defendButton;
    public Button skillButton;
    public Button retreatButton;
    public GameObject skillPanel;
    
    private bool areSkillsActive = false;

    private PlayerCombat player;
    private List<EnemyCombat> enemies;
    private Transform playerStart;
    private List<Transform>  enemiesStart;
    private TextMeshProUGUI battlelog;
    private List<bool> enemyTurns = new List<bool>();
    
    

    void Start()
    {
        state = CombatState.START;
        battlelog  = FindObjectsOfType<TextMeshProUGUI>().Where(x => x.name == "BattleLog").First();
        playerStart = FindObjectsOfType<Transform>().Where(t => t.name == "PlayerLocation").First();
        enemiesStart = FindObjectsOfType<Transform>().Where(t => t.name == "EnemyLocation").OrderBy(t => t.position.x).ToList();
        
        skillButton.onClick.AddListener(ShowSkillPanel);
        attackButton.onClick.AddListener(PlayerAttack);
        defendButton.onClick.AddListener(PlayerDefend);
        // retreatButton.onClick.AddListener(PlayerRetreat);
        skillPanel.SetActive(false);

        while (enemyTurns.Count < enemiesStart.Count)
        {
            enemyTurns.Add(false);
        }
        
        SetupBattle();
    }

    void SetupBattle()
    {
        int enemiesSpawned = 0;
        // Instantiate player and enemy at predetermined positions.
        player = Instantiate(playerPrefab, playerStart.position + new Vector3(0,5,0), Quaternion.identity).GetComponent<PlayerCombat>();
        while (enemiesSpawned < numberOfEnemies)
        {
            enemies.Add(Instantiate(enemyPrefab, enemiesStart[enemiesSpawned].position + new Vector3(0,5,0), Quaternion.identity).GetComponent<EnemyCombat>());
            enemiesSpawned++;
        }
        

        // Start the turn sequence.
        StartCoroutine(CombatSequence());
    }

    IEnumerator CombatSequence()
    {
        List<bool> enemiesDefeated = new List<bool>();
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
                    foreach (var enemy in enemies)
                    {
                        if (enemy.currentHealth <= 0)
                        {
                            enemiesDefeated[enemies.IndexOf(enemy)] = true;
                        }
                        else if(enemy.currentHealth > 0)
                        {
                            enemiesDefeated[enemies.IndexOf(enemy)] = false;
                        }

                    }
                    if (enemies.All(e => e == true))
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
        if (player.isDefending){ player.isDefending = false; }
        // Here you’d bring up your UI for the player to select an action.
        // For example, wait until the player selects “Attack” or “Defend”.
        yield return new WaitUntil(() => player.actionCompleted);
        // yield return new WaitForSeconds(0.5f);
        // // player.InstantKill(enemy, battlelog);
        // // Optionally, process player input results here (damage calculations, animations, etc.)
        //yield return new WaitForSeconds(0.5f);
    }

    IEnumerator EnemyTurn()
    {
        int i = 0;
        while (i < enemyTurns.Count)
        {
            yield return new WaitForSeconds(1f);
            enemies[i].PerformAttack(player, battlelog);
            yield return new WaitForSeconds(0.5f);
            i++;
        }
        // Simple enemy logic: attack after a short delay.
        
    }

    void EndBattle()
    {
        if (state == CombatState.WON)
        {
            battlelog.text += "\nVictory!";
            Debug.Log("Victory!");
            // Trigger victory animations, sounds, or transitions.
        }
        else if (state == CombatState.LOST)
        {
            battlelog.text += "\nDefeat!";
            Debug.Log("Defeat!");
            // Trigger defeat logic.
        }
    }

    void ShowSkillPanel()
    { 
        if (areSkillsActive == false)
        {
            // Bring the SkillPanel to the front
            skillPanel.transform.SetAsLastSibling();

            // Activate the panel if it's not already active
            if (!skillPanel.activeSelf)
            {
                skillPanel.SetActive(true);
            }
            areSkillsActive = true;
        }
        else if (areSkillsActive == true)
        {
            skillPanel.transform.SetAsFirstSibling();
            skillPanel.SetActive(false);
            areSkillsActive = false;
        }
    }
    void PlayerAttack()
    {
        //TODO: Enemy Picker Method()
        player.Attack(enemy, battlelog);
    }
    void PlayerDefend()
    {
        player.Defend(battlelog);
    }
}
