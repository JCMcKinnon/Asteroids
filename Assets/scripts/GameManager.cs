using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerCollision collision;
    public int lives;

    public event Action OnRespawn;

    private void Awake()
    {
        instance = this;

        lives = 3;
    }
    private void LostLife()
    {
        lives--;
        StartCoroutine(Respawn());
    }
    private void OnEnable()
    {
        collision.OnPlayerCollided += LostLife;
    }
    private void OnDisable()
    {
        collision.OnPlayerCollided -= LostLife;

    }
    // Start is called before the first frame update
    void Start()
    {
    }
   public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        OnRespawn?.Invoke();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
