using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    private List<GameObject> bulletPool;
    public Queue<GameObject> bullets;

    private float shootTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new List<GameObject>();
        bullets = new Queue<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            var instance = Instantiate(bullet);
            bulletPool.Add(instance);

            instance.SetActive(false);
            bullets.Enqueue(instance);
            


            //instance
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && shootTimer < 0f)
        {
            //Instantiate();
        
            
            GameObject instance = bullets.Dequeue();
            instance.SetActive(true);
            instance.transform.position = transform.position + transform.up ;
            instance.transform.rotation = transform.rotation;
            print(bullets.Count);

            shootTimer = 0.6f;
        }
        if (bullets.Count < 1)
        {
            for (int i = 0; i < bulletPool.Count; i++)
            {
                bullets.Enqueue(bulletPool[i]);
            }
        }
        shootTimer -= Time.deltaTime;
    }
}
