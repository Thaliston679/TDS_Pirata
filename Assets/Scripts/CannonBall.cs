using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    public float speed = 5;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.up * speed;

        speed -= Time.deltaTime * 2;

        if(speed <= 1)
        {
            DestroyBall();
        }
    }

    void DestroyBall()
    {
        GameObject vfxExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(vfxExplosion, 0.25f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DestroyBall();

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage();
        }
    }
}
