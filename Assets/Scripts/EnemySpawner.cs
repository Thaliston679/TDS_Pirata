using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EnemySpawner : MonoBehaviour
{
    GameObject player;
    [SerializeField]GameObject playerPrefab;
    public GameObject[] enemyPrefab;

    public float spawnRadius = 10f;
    [SerializeField] private float timeUntilNextSpawn = 5f;

    [SerializeField] private float matchDuration;
    private float matchTimer = 0;

    public LayerMask whatIsObstacles;

    [SerializeField] Scrollbar scrollTimeEnemy;
    [SerializeField] Scrollbar scrollTimeMatch;

    private void Update()
    {
        if (player != null) EnemySpawn();

        if (player != null) MatchTime();

        if(player == null && matchTimer > 0)
        {
            //Derrota
        } 
    }
    
    void MatchTime()
    {
        matchTimer -= Time.deltaTime;

        if(matchTimer <= 0)
        {
            //Vitoria
        }
    }

    void EnemySpawn()
    {
        timeUntilNextSpawn -= Time.deltaTime;

        if (timeUntilNextSpawn <= 0)
        {
            Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius + (Vector2)player.transform.position;

            // Verifica se há colisões na área de spawn
            if (Physics2D.OverlapCircle(randomPos, 3, whatIsObstacles) == null)
            {
                GameObject enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], randomPos, Quaternion.identity);
                //Virar inimigo na direcao do player

                Vector3 direction = player.transform.position - enemy.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

                timeUntilNextSpawn = 5f;
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
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        FindPlayer();

        matchTimer = matchDuration;
    }

    public void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void TimeUntilNextSpawn(float value)
    {
        timeUntilNextSpawn = value;
    }
}
