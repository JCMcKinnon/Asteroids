using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCollision : MonoBehaviour
{
    public event Action OnPlayerCollided;
    public GameObject particles;
    public GameObject explosion;
    public GameManager gm;
    public SpriteRenderer sr;
    public BoxCollider2D bc;
    public AsteroidManager am;
    public Animator animator;

    public bool invincible;
    public float invincibleTimer;
    public float invincibleMaxTimer;

    public bool dead;
    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
        am = GameObject.FindObjectOfType<AsteroidManager>();
        animator = GetComponent<Animator>();
    }
    public void OnEnable()
    {
        invincible = true;
        invincibleTimer = invincibleMaxTimer;
        gm.OnRespawn += Respawn;
        am.OnAsteroidsCleared += SetInvincible;
        animator.SetBool("isImmortal", true);

    }
    public void OnDisable()
    {
        gm.OnRespawn -= Respawn;

    }
    private void Update()
    {
        if(invincibleTimer < 0)
        {
            invincible = false;
            animator.SetBool("isImmortal", false);

        }
        else
        {
            invincible = true;
            animator.SetBool("isImmortal", true);

        }
        invincibleTimer -= Time.deltaTime;

    }
    public void Collided()
    {
    }
    public void Respawn()
    {
        sr.enabled = true;
        bc.enabled = true;
        
        gameObject.transform.position = new Vector3(5, -5, 0);
        dead = false;
        invincibleTimer = invincibleMaxTimer;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Asteroid" && !invincible)
        {
            OnPlayerCollided?.Invoke();
            sr.enabled = false;
            bc.enabled = false;
            //Instantiate(explosion, transform.position, Quaternion.identity);
            dead = true;
            Instantiate(particles, transform.position, Quaternion.identity);

            
        }
    }
    private void SetInvincible()
    {
        invincibleTimer = invincibleMaxTimer;
    }
}
