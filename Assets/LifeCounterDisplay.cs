using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasRenderer image1;
    public CanvasRenderer image2;
    public CanvasRenderer image3;

    public Queue<CanvasRenderer> lives;

    public PlayerCollision pc;
    public Action removeLife;
    void Start()
    {
        lives = new Queue<CanvasRenderer>();
        lives.Enqueue(image1);
        lives.Enqueue(image2);
        lives.Enqueue(image3);
       
    }
    private void OnEnable()
    {
        pc.OnPlayerCollided += RemoveLife;
    }
    private void OnDisable()
    {
        pc.OnPlayerCollided -= RemoveLife;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveLife()
    {
        lives.Dequeue().SetAlpha(0);
    }
    public void AddLife(CanvasRenderer cr)
    {
       
    }
}
