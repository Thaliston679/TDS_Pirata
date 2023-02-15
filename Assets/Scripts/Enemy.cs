using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("1 = Chaser / 2 = Shooter")]
    [SerializeField] int enemyType;

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float rotationSpeed = 1;
    float lowRotation;
    float totalRotation;
    Rigidbody2D rb;
    GameObject player;

    [SerializeField] float reloadTime = 0.75f;
    float reload;
    [SerializeField] GameObject cannonBullet;
    [SerializeField] Transform[] cannonL;
    [SerializeField] float viewDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (enemyType == 1)
        {
            lowRotation = rotationSpeed / 6;
            totalRotation = rotationSpeed;
        }

    }

    void Update()
    {
        if(enemyType == 1)
        {
            Movement();
        }
        else
        {
            if(Vector2.Distance(transform.position, player.transform.position) < viewDistance)
            {
                Movement();
            }
            else
            {
                rb.velocity = -transform.up * moveSpeed;
            }
        }
        

        if (enemyType == 2 && Vector2.Distance(transform.position, player.transform.position) < viewDistance)
        {
            if (reload >= reloadTime) Shoot();
        }

        if (reload < reloadTime)
        {
            reload += Time.deltaTime;
        }

        if (enemyType == 1)
        {
            if(Vector2.Distance(transform.position, player.transform.position) < viewDistance)
            {
                rotationSpeed = lowRotation;
            }
            else
            {
                rotationSpeed = totalRotation;
            }
        }
    }

    void Shoot()
    {
        for (int i = 0; i < cannonL.Length; i++)
        {
            Instantiate(cannonBullet, cannonL[i].position, cannonL[i].rotation);
        }
        reload = 0;
    }

    void Movement()
    {
        rb.velocity = -transform.up * moveSpeed;
        float typeRot = enemyType == 1 ? 90 : 0;
        
        var shipDirection = player.transform.position - transform.position;
        var desiredAngle = Mathf.Atan2(shipDirection.y, shipDirection.x) * Mathf.Rad2Deg;
        var rotationShip = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, desiredAngle + typeRot), rotationShip);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
