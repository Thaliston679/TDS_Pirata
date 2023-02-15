using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameObject player;
    public GameObject[] enemyPrefab;
    public float spawnRadius = 10f;
    [SerializeField]private float timeUntilNextSpawn = 5f;

    public LayerMask whatIsObstacles;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        timeUntilNextSpawn -= Time.deltaTime;

        /*if (timeUntilNextSpawn <= 0)
        {
            Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius + (Vector2)player.transform.position;

            Collider2D[] obstacles = Physics2D.OverlapCircleAll(randomPos, 1, whatIsObstacles);

            if(obstacles == null)
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], randomPos, Quaternion.identity);
                timeUntilNextSpawn = 5f;
            }
            else
            {
                timeUntilNextSpawn = 0.01f;
            }
            
        }*/


        if (timeUntilNextSpawn <= 0 && player != null)
        {
            Vector2 randomPos = Random.insideUnitCircle.normalized * spawnRadius + (Vector2)player.transform.position;

            // Verifica se há colisões na área de spawn
            if (Physics2D.OverlapCircle(randomPos, 3, whatIsObstacles) == null)
            {
                Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], randomPos, Quaternion.identity);
                timeUntilNextSpawn = 5f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(player != null)Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
    }
}
