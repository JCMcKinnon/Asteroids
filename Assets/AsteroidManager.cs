using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public GameObject particleSystemObject;
    public class LittleAsteroidData
    {
        public GameObject gameObject { get; set; }
        public Vector3 direction { get; set; }
        public bool collided;
        public float timer;
        public AsteroidCollision asteroidCollision;
        public LittleAsteroidData(GameObject gameObject, Vector3 direction, float timer)
        {
            this.gameObject = gameObject;
            this.direction = direction;
            this.timer = timer;
            asteroidCollision = gameObject.GetComponent<AsteroidCollision>();
            collided = asteroidCollision.collided;
        }

    }
    public class BigAsteroidData
    {
        public GameObject gameObject { get; set; }
        public Vector3 direction { get; set; }
        public LittleAsteroidData[] LittleAsteroids;
        public float timer;
        public AsteroidCollision asteroidCollision;
        public bool collided;

        public BigAsteroidData(GameObject gameObject, Vector3 direction, float timer)
        {
            this.gameObject = gameObject;
            this.direction = direction;
            LittleAsteroids = new LittleAsteroidData[4];
            this.timer = timer;
            asteroidCollision = gameObject.GetComponent<AsteroidCollision>();
            collided = asteroidCollision.collided;
        }
    }



    //JUST MAKE THE ASTEROIDS A FUCKING CLASS


    /// <summary>
    /// prefabs to use for pool
    /// </summary>
    //-------------------------------------------------------------------------------------------

    //enemies
    public GameObject BigAsteroid;

    public GameObject smallAsteroidBotLeft;
    public GameObject smallAsteroidBotRight;
    public GameObject smallAsteroidTopLeft;
    public GameObject smallAsteroidTopRight;

    //non enemies
    public GameObject brokenAsteroidTopLeft1;
    public GameObject brokenAsteroidTopLeft2;
    public GameObject brokenAsteroidTopLeft3;
    public GameObject brokenAsteroidTopLeft4;
    public GameObject brokenAsteroidTopRight1;
    public GameObject brokenAsteroidTopRight2;
    public GameObject brokenAsteroidTopRight3;
    public GameObject brokenAsteroidTopRight4;
    public GameObject brokenAsteroidBotLeft1;
    public GameObject brokenAsteroidBotLeft2;
    public GameObject brokenAsteroidBotLeft3;
    public GameObject brokenAsteroidBotLeft4;
    public GameObject brokenAsteroidBotRight1;
    public GameObject brokenAsteroidBotRight2;
    public GameObject brokenAsteroidBotRight3;
    public GameObject brokenAsteroidBotRight4;
    //----------------------------------------------------------------------------------------

    /// <summary>
    /// variable settings
    /// </summary>
    //----------------------------------------------------------------------------------------
    public int MaxAsteroidCount;
    public int MinAsteroidCount;
    public int maxLittleAsteroidCount;
    public int AsteroidSpeed;
    public Rect bounds;
    public float wraparoundOffset;
    //----------------------------------------------------------------------------------------


    /// <summary>
    /// Containers
    /// </summary>
    //----------------------------------------------------------------------------------------
    public List<BigAsteroidData> bigAsteroidPool;
    public List<LittleAsteroidData> littleAsteroidPool;

    public Vector3[] bigAsteroidDirections; //the chosen directions of travel for a given asteroid, indicated by array index
    public GameObject[] littleAsteroidObjects;

    public List<float> smallAsteroidTimer;
    public bool[] smallAsteroidResetTimer;
    public float[] bigAsteroidTimer; //the time that an asteroid can no longer be teleported, since asteroids were stuck in teleport loop
    public bool[] bigAsteroidResetTimer; //boolean flags to determine if a tp timer should be reset.

    public AsteroidCollision[] bigAsteroidCollisions; //script to access collision info from individual asteroids
    public List<AsteroidCollision> littleAsteroidCollisions;

    public List<GameObject> asteroidsInPlay;
    //----------------------------------------------------------------------------------------


    /// <summary>
    /// Init
    /// </summary>
    /// 
    //----------------------------------------------------------------------------------------
    private Camera cam;
    //----------------------------------------------------------------------------------------

    /// <summary>
    /// Constants
    /// </summary>
    //----------------------------------------------------------------------------------------


    private Vector3 up;
    private Vector3 down;
    private Vector3 left;
    private Vector3 right;
    private List<Vector3> directions; 

    //----------------------------------------------------------------------------------------

    private void Awake()
    {
        asteroidsInPlay = new List<GameObject>();
        littleAsteroidPool = new List<LittleAsteroidData>();

        directions = new List<Vector3>();
        directions.Add(new Vector3(0, 1, 0)); //up
        directions.Add(new Vector3(1, 1, 0));//upright
        directions.Add(new Vector3(1, 0, 0));//right
        directions.Add(new Vector3(1, -1, 0));//downright
        directions.Add(new Vector3(0, -1, 0));//down
        directions.Add(new Vector3(-1, -1, 0));//downleft
        directions.Add(new Vector3(-1, 0, 0));//left
        directions.Add(new Vector3(-1, 1, 0));//upleft








        bigAsteroidPool = new List<BigAsteroidData>();
        PopulateObjectPools();
        InitializeCameraAndPlayerBoarders();


    }
    void Start()
    {
        littleAsteroidObjects = new GameObject[4];
        littleAsteroidObjects[0] = smallAsteroidBotLeft;
        littleAsteroidObjects[1] = smallAsteroidBotRight;
        littleAsteroidObjects[2] = smallAsteroidTopLeft;
        littleAsteroidObjects[3] = smallAsteroidTopRight;

        


        //TODO: Set small asteroids directions and use in line 137
    }

    // Update is called once per frame
    void Update()
    {
       
        //move asteroids, and and teleport them if needed.
        for (int i = 0; i < bigAsteroidPool.Count ; i++)
        {
            if (bigAsteroidPool[i].gameObject.activeSelf)
            {
                MoveAsteroid(bigAsteroidPool[i].gameObject, bigAsteroidPool[i].direction);

                if (bigAsteroidPool[i].gameObject.transform.position.y >= up.y + wraparoundOffset)
                {
                    if (bigAsteroidPool[i].timer < 0)
                    {
                        bigAsteroidPool[i].gameObject.transform.position = new Vector3(bigAsteroidPool[i].gameObject.transform.position.x, down.y - wraparoundOffset + 1, 0);
                        bigAsteroidPool[i].timer = 3f;

                    }
                }
                if (bigAsteroidPool[i].gameObject.transform.position.y <= down.y - wraparoundOffset)
                {
                    if (bigAsteroidPool[i].timer < 0)
                    {
                        bigAsteroidPool[i].gameObject.transform.position = new Vector3(bigAsteroidPool[i].gameObject.transform.position.x, up.y + wraparoundOffset - 1, 0);
                        bigAsteroidPool[i].timer = 3f;

                    }
                }
                if (bigAsteroidPool[i].gameObject.transform.position.x >= right.x + wraparoundOffset)
                {
                    //if pos is on the right

                    if (bigAsteroidPool[i].timer < 0)
                    {
                        bigAsteroidPool[i].gameObject.transform.position = new Vector3(left.x - (wraparoundOffset - 1), bigAsteroidPool[i].gameObject.transform.position.y, 0);
                        bigAsteroidPool[i].timer = 3f;

                    }

                }
                if (bigAsteroidPool[i].gameObject.transform.position.x <= left.x - wraparoundOffset)
                {
                    //if pos is on the left
                    if (bigAsteroidPool[i].timer < 0)
                    {
                        bigAsteroidPool[i].gameObject.transform.position = new Vector3(right.x + (wraparoundOffset + 1), bigAsteroidPool[i].gameObject.transform.position.y, 0);
                        bigAsteroidPool[i].timer = 3f;

                    }
                }
                bigAsteroidPool[i].timer -= Time.deltaTime;
            }
        }
        
        for (int i = 0; i < bigAsteroidPool.Count; i++)
        {
            for (int x = 0; x < bigAsteroidPool[i].LittleAsteroids.Length ; x++)
            {
                if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.activeSelf)
                {
                    MoveAsteroid(bigAsteroidPool[i].LittleAsteroids[x].gameObject, bigAsteroidPool[i].LittleAsteroids[x].direction);

                    if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.y >= up.y + wraparoundOffset)
                    {
                        if (bigAsteroidPool[i].LittleAsteroids[x].timer < 0)
                        {
                            bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = new Vector3(bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.x, down.y - wraparoundOffset + 1, 0);
                            bigAsteroidPool[i].LittleAsteroids[x].timer = 3f;

                        }
                    }
                    if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.y <= down.y - wraparoundOffset)
                    {
                        if (bigAsteroidPool[i].LittleAsteroids[x].timer < 0)
                        {
                            bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = new Vector3(bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.x, up.y + wraparoundOffset - 1, 0);
                            bigAsteroidPool[i].LittleAsteroids[x].timer = 3f;

                        }
                    }
                    if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.x >= right.x + wraparoundOffset)
                    {
                        //if pos is on the right

                        if (bigAsteroidPool[i].LittleAsteroids[x].timer < 0)
                        {
                            bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = new Vector3(left.x - (wraparoundOffset - 1), bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.y, 0);
                            bigAsteroidPool[i].LittleAsteroids[x].timer = 3f;

                        }

                    }
                    if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.x <= left.x - wraparoundOffset)
                    {
                        //if pos is on the left
                        if (bigAsteroidPool[i].timer < 0)
                        {
                            bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = new Vector3(right.x + (wraparoundOffset + 1), bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position.y, 0);
                            bigAsteroidPool[i].LittleAsteroids[x].timer = 3f;

                        }
                    }
                    bigAsteroidPool[i].LittleAsteroids[x].timer -= Time.deltaTime;
                }
            }
            
            
           
        }



        for (int i = 0; i < bigAsteroidPool.Count ; i++)
        {
            if (bigAsteroidPool[i].asteroidCollision.collided)
            {
                for (int x = 0; x < bigAsteroidPool[i].LittleAsteroids.Length  ; x++)
                {
                    bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = bigAsteroidPool[i].gameObject.transform.position;
                    print(bigAsteroidPool[i].asteroidCollision.collided);
                    bigAsteroidPool[i].LittleAsteroids[x].gameObject.SetActive(true);
                    bigAsteroidPool[i].asteroidCollision.collided = false;
                }
                bigAsteroidPool[i].gameObject.SetActive(false);
            }

           



        }
        for (int x = 0; x < bigAsteroidPool.Count; x++)
        {
            for (int w = 0; w < bigAsteroidPool[x].LittleAsteroids.Length; w++)
            {
                if (bigAsteroidPool[x].LittleAsteroids[w].asteroidCollision.collided && bigAsteroidPool[x].LittleAsteroids[w].gameObject.activeSelf)
                {
                    SpawnParticlesInPlace(bigAsteroidPool[x].LittleAsteroids[w].gameObject.transform.position);
                    bigAsteroidPool[x].LittleAsteroids[w].gameObject.SetActive(false);
                }
            }
        }
    }
    private void SpawnParticlesInPlace(Vector3 pos)
    {
        Instantiate(particleSystemObject, pos, Quaternion.identity);
    }


        private void MoveAsteroid(GameObject asteroid, Vector3 direction)
        {
            asteroid.transform.position += direction * Time.deltaTime * 2;
        }
        public void InitializeCameraAndPlayerBoarders()
        {
            cam = Camera.main;
            up = cam.ViewportToWorldPoint(Vector3.up);
            down = cam.ViewportToWorldPoint(Vector3.zero);
            left = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            right = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));
        }
        public void PopulateObjectPools()
        {

            for (int i = 0; i < MaxAsteroidCount; i++)
            {
                var instance = Instantiate(BigAsteroid);

                var randomDir1 = directions[Random.Range(0, directions.Count)];
                randomDir1.x += Random.Range(-1f, 1f);
                print(randomDir1);
                bigAsteroidPool.Add(new BigAsteroidData(instance, randomDir1.normalized, 4f));
                
                   
            }
            for (int i = 0; i < bigAsteroidPool.Count ; i++)
            {
                bigAsteroidPool[i].collided = bigAsteroidPool[i].asteroidCollision.collided;

                var randomDir2 = directions[Random.Range(0, directions.Count)];
               // randomDir2.x += Random.Range(-1f, 1f);
                var randomDir3 = directions[Random.Range(0, directions.Count)];
               // randomDir3.x += Random.Range(-1f, 1f);
                var randomDir4 = directions[Random.Range(0, directions.Count)];
               // randomDir4.x += Random.Range(-1f, 1f);
                var randomDir5 = directions[Random.Range(0, directions.Count)];
               // randomDir5.x += Random.Range(-1f, 1f);

                var littleAsteroidInstance1 = Instantiate(smallAsteroidBotLeft, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance2 = Instantiate(smallAsteroidBotRight, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance3 = Instantiate(smallAsteroidTopLeft, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance4 = Instantiate(smallAsteroidTopRight, Vector3.zero, Quaternion.identity);
                bigAsteroidPool[i].LittleAsteroids[0] = new LittleAsteroidData(littleAsteroidInstance1, randomDir2.normalized,3);
                bigAsteroidPool[i].LittleAsteroids[1] = new LittleAsteroidData(littleAsteroidInstance2, randomDir3.normalized,3);
                bigAsteroidPool[i].LittleAsteroids[2] = new LittleAsteroidData(littleAsteroidInstance3, randomDir4.normalized, 3);
                bigAsteroidPool[i].LittleAsteroids[3] = new LittleAsteroidData(littleAsteroidInstance4, randomDir5.normalized, 3);

                littleAsteroidPool.Add(bigAsteroidPool[i].LittleAsteroids[0]);
                littleAsteroidPool.Add(bigAsteroidPool[i].LittleAsteroids[1]);
                littleAsteroidPool.Add(bigAsteroidPool[i].LittleAsteroids[2]);
                littleAsteroidPool.Add(bigAsteroidPool[i].LittleAsteroids[3]);

                for (int x = 0; x < 4; x++)
                {
                    bigAsteroidPool[i].LittleAsteroids[x].gameObject.SetActive(false);
                }
            }

        }    
}

