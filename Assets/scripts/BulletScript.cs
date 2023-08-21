using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject player;
    public float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        timer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * Time.deltaTime * 8;
        timer-=Time.deltaTime;
        if(timer < 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
