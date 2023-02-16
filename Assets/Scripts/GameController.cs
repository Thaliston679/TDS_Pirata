using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject playerPrefab;
    public GameObject[] enemyPrefab;

    public CameraFollow cam;

    public float spawnRadius = 10f;
    [SerializeField] private float timeUntilNextSpawn = 5f;
    private float timerUntilNextSpawn = 5f;

    [SerializeField] private float matchDuration = 60f;
    private float matchTimer = 0;
    private float elapsedTime = 0;

    public LayerMask whatIsObstacles;

    [SerializeField] Scrollbar scrollTimeEnemy;
    [SerializeField] Scrollbar scrollTimeMatch;

    public GameObject panelEndGame;
    public TextMeshProUGUI defeatedEnemiesTxt;
    public TextMeshProUGUI matchTimeElapsedTxt;

    float defeatedEnemies;

    bool inGame;

    public TextMeshProUGUI inGameTxt;

    private void Update()
    {
        if (player != null && inGame)
        {
            EnemySpawn();

            MatchTime();

            elapsedTime += Time.deltaTime;
        }

        if (player == null && matchTimer > 0 && inGame)
        {
            inGame = false;

            Invoke(nameof(EndGame), 2f);
        } 
    }
    
    void MatchTime()
    {
        matchTimer -= Time.deltaTime;

        if(matchTimer <= 0)
        {
            inGame = false;

            Invoke(nameof(EndGame), 0.01f);
        }
    }

    void EnemySpawn()
    {
        timerUntilNextSpawn -= Time.deltaTime;

        if (timerUntilNextSpawn <= 0)
        {
            Vector2 randomPos = UnityEngine.Random.insideUnitCircle.normalized * spawnRadius + (Vector2)player.transform.position;

            // Verifica se há colisões na área de spawn
            if (Physics2D.OverlapCircle(randomPos, 3, whatIsObstacles) == null)
            {
                GameObject enemy = Instantiate(enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)], randomPos, Quaternion.identity);
                //Virar inimigo na direcao do player

                Vector3 direction = player.transform.position - enemy.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

                timerUntilNextSpawn = timeUntilNextSpawn;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(player != null)Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
    }

    public void ConfigValues()
    {
        Debug.Log(scrollTimeEnemy.value);
        Debug.Log(scrollTimeMatch.value);

        switch (scrollTimeEnemy.value)
        {
            case 0:
                timeUntilNextSpawn = 10;
                break;
            case 0.5f:
                timeUntilNextSpawn = 5;
                break;
            case 1:
                timeUntilNextSpawn = 3;
                break;
        }

        switch (scrollTimeMatch.value)
        {
            case 0:
                matchDuration = 60;
                break;
            case 0.5f:
                matchDuration = 120;
                break;
            case 1:
                matchDuration = 180;
                break;
        }
    }

    public void PlayGame()
    {
        matchTimer = matchDuration;

        DestroyEnemiesAndPlayer();

        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        FindPlayer();

        inGame = true;
    }

    public void PlayAgain()
    {
        Camera.main.orthographicSize = 5;

        DestroyEnemiesAndPlayer();

        elapsedTime = 0;
        defeatedEnemies = 0;

        matchTimer = matchDuration;

        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        FindPlayer();

        inGame = true;
    }

    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam.target = player;
    }

    public void TimeUntilNextSpawn(float value)
    {
        timeUntilNextSpawn = value;
    }

    public void EndGame()
    {
        Debug.Log("EndGame;");

        DestroyEnemiesAndPlayer();

        panelEndGame.SetActive(true);

        defeatedEnemiesTxt.text = $"Inimigos derrotados\n{defeatedEnemies}";

        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        string formattedElapsedTIme = timeSpan.ToString("mm\\:ss");

        matchTimeElapsedTxt.text = $"Tempo de partida\n{formattedElapsedTIme}";
    }

    public void DestroyEnemiesAndPlayer()
    {
        if (player != null) Destroy(player);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    public void AddEnemyDefeated()
    {
        defeatedEnemies++;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LateUpdate()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        string formattedElapsedTIme = timeSpan.ToString("mm\\:ss");

        inGameTxt.text = $"Tempo de jogo: {formattedElapsedTIme}\nInimigos derrotados: {defeatedEnemies}";
    }
}
