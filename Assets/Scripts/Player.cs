using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float rotateSpeed = 30;
    [SerializeField] float reloadTime = 0.5f;
    float reload;

    [SerializeField] GameObject cannonBullet;
    [SerializeField] Transform[] cannonR;
    [SerializeField] Transform[] cannonL;
    [SerializeField] Transform cannonF;

    [SerializeField]int hpMax;
    int hp;


    void Start()
    {
        hp = hpMax;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetInteger("Hp", hp);

        reload = reloadTime;
    }

    private void Update()
    {
        if(reload < reloadTime)
        {
            reload += Time.deltaTime;
        }

        if(reload >= reloadTime && hp > 0) Attack();
    }


    void FixedUpdate()
    {
        if(hp > 0)
        {
            rb.angularVelocity = -Input.GetAxis("Horizontal") * rotateSpeed;
            rb.velocity = -transform.up * Input.GetAxis("Vertical") * moveSpeed;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(cannonBullet, cannonF.position, cannonF.rotation);
            reload = 0;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int i = 0; i < cannonR.Length; i++)
            {
                Instantiate(cannonBullet, cannonR[i].position, cannonR[i].rotation);
            }

            reload = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < cannonL.Length; i++)
            {
                Instantiate(cannonBullet, cannonL[i].position, cannonL[i].rotation);
            }

            reload = 0;
        }
    }

    public void TakeDamage()
    {
        hp--;
        anim.SetInteger("Hp", hp);
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}
