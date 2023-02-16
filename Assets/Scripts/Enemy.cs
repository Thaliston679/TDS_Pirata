using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]float hpMax;
    float hp;
    public GameObject healthCanvas;
    public Image healthBar;

    Animator anim;

    public GameObject crew;
    public GameObject shipExplosion;

    bool chaserAtk = false;

    void Start()
    {
        hp = hpMax;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        anim.SetInteger("Hp", (int)hp);

        reload = reloadTime;

        if (enemyType == 1)
        {
            lowRotation = rotationSpeed / 6;
            totalRotation = rotationSpeed;
        }

    }

    void Update()
    {
        if(hp > 0 && player != null)
        {
            if (enemyType == 1)
            {
                Movement();
            }
            else
            {
                if (Vector2.Distance(transform.position, player.transform.position) < viewDistance)
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
                if (Vector2.Distance(transform.position, player.transform.position) < viewDistance)
                {
                    rotationSpeed = lowRotation;
                }
                else
                {
                    rotationSpeed = totalRotation;
                }
            }
        }

        if (hp <= 0) GetComponent<TrailRenderer>().enabled = false;
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

    public void TakeDamage()
    {
        hp--;
        anim.SetInteger("Hp", (int)hp);

        float barValue = 1 / (hpMax / hp);
        healthBar.fillAmount = barValue;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    public void Shipwreck()
    {
        Instantiate(crew, transform.position, Quaternion.identity);
        Destroy(healthCanvas);
        GameObject shipExp = Instantiate(shipExplosion, transform.position, transform.rotation);
        Destroy(shipExp, 0.25f);
        if (!chaserAtk) GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().AddEnemyDefeated();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyType == 1 && collision.gameObject.CompareTag("Player"))
        {
            chaserAtk = true;
            hp = 0;
            anim.SetInteger("Hp", (int)hp);
            collision.gameObject.GetComponent<Player>().TakeDamage();
        }
    }

    private void LateUpdate()
    {
        if(healthCanvas != null)
        {
            healthCanvas.transform.position = transform.position + new Vector3(0, 0.8f, 0);
            healthCanvas.transform.LookAt(Camera.main.transform);
            healthCanvas.transform.Rotate(0, 0, 0);
        }
    }
}
