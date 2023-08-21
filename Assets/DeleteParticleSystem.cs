using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticleSystem : MonoBehaviour
{
    float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
            Destroy(gameObject);
        }
    }
}
