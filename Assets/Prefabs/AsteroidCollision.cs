using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    public bool collided;
    public PlayerCollision pc;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        pc = GameObject.FindObjectOfType<PlayerCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        collided = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet" || collision.tag == "Player")
        {
            if(!pc.invincible && collision.tag == "Player")
            {
                collided = true;
            }
            if(collision.tag == "Bullet")
            {
                collided = true;
            }
        }
    }




}
