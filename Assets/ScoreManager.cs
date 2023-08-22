using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public AsteroidManager am;
    public int points;
    private void OnEnable()
    {
        am.OnDestroyedLargeAsteroid += AddTenPoints;
        am.OnDestroyedSmallAsteroid += AddTwoPoints;
        am.OnAsteroidsCleared += AddFivePoints;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTenPoints()
    {
        points += 10;
    }
    public void AddFivePoints()
    {
        points += 5;
    }
    public void AddTwoPoints()
    {
        points += 2;
    }
        
}

