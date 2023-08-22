using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AsteroidManager : MonoBehaviour
{
    public GameObject particleSystemObject;
    public interface IAsteroid
    {
        GameObject gameObject { get; set; }
        Vector3 direction { get; set; }
        bool collided { get; set; }
        float timer { get; set; }
        float speed { get; set; }
        AsteroidCollision asteroidCollision { get; set; }

    }
    public class LittleAsteroidData : IAsteroid
    {
        public LittleAsteroidData(GameObject gameObject, Vector3 direction, float timer, float speed)
        {
            this.gameObject = gameObject;
            this.direction = direction;
            this.timer = timer;
            asteroidCollision = gameObject.GetComponent<AsteroidCollision>();
            collided = asteroidCollision.collided;
            this.speed = speed;
        }
        public float speed { get; set; }
        public GameObject gameObject { get; set; }
        public Vector3 direction { get; set; }
        public bool collided { get; set; }
        public float timer { get; set; }
        public AsteroidCollision asteroidCollision { get; set; }
    }
    public class BigAsteroidData : IAsteroid
    {
        public LittleAsteroidData[] LittleAsteroids;
        public BigAsteroidData(GameObject gameObject, Vector3 direction, float timer, float speed)
        {
            this.gameObject = gameObject;
            this.direction = direction;
            LittleAsteroids = new LittleAsteroidData[4];
            this.timer = timer;
            asteroidCollision = gameObject.GetComponent<AsteroidCollision>();
            collided = asteroidCollision.collided;
            this.speed = speed;
        }
        public float speed { get; set; }

        public GameObject gameObject { get; set; }
        public Vector3 direction { get; set; }
        public bool collided { get; set; }
        public float timer { get; set; }
        public AsteroidCollision asteroidCollision { get; set; }
        public float spawnInvincibilityTimer;
    }



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

        public int asteroidsInPlay;
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


    /// <summary>
    /// events
    /// </summary>
    //-----------------------------------------------------------------------------------------
    public event Action OnAsteroidsCleared;
    public event Action OnDestroyedLargeAsteroid;
    public event Action OnDestroyedSmallAsteroid;

        private void Awake()
        {
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

        public void CheckForBounds(IAsteroid asteroid)
        {
            if (asteroid.gameObject.transform.position.y >= up.y + wraparoundOffset)
            {
                if(asteroid.timer < 0)
                {
                    Vector3 newPosition = new Vector3(asteroid.gameObject.transform.position.x, down.y - wraparoundOffset + 1, 0);
                    asteroid.gameObject.transform.position = newPosition;
                }
            }
            if (asteroid.gameObject.transform.position.y <= down.y - wraparoundOffset)
            {
                if (asteroid.timer < 0)
                {
                    asteroid.gameObject.transform.position = new Vector3(asteroid.gameObject.transform.position.x, up.y + wraparoundOffset - 1, 0);
                    asteroid.timer = 3f;
                }
            }
            if (asteroid.gameObject.transform.position.x >= right.x + wraparoundOffset)
            {
                //if pos is on the right

                if (asteroid.timer < 0)
                {
                    asteroid.gameObject.transform.position = new Vector3(left.x - (wraparoundOffset - 1), asteroid.gameObject.transform.position.y, 0);
                    asteroid.timer = 3f;
                }
            }
            if (asteroid.gameObject.transform.position.x <= left.x - wraparoundOffset)
            {
                //if pos is on the left
                if (asteroid.timer < 0)
                {
                     asteroid.gameObject.transform.position = new Vector3(right.x + (wraparoundOffset + 1), asteroid.gameObject.transform.position.y, 0);
                     asteroid.timer = 3f;
                }
            }
            asteroid.timer -= Time.deltaTime;
        }


        // Update is called once per frame
        void Update()
        {
            //check if asteroids cleared, if so spawn more 
            if(asteroidsInPlay <= 0)
            {
                OnAsteroidsCleared?.Invoke();
                PopulateObjectPools();

            for (int i = 0; i < bigAsteroidPool.Count; i++)
                {
                    asteroidsInPlay = MaxAsteroidCount;
                    bigAsteroidPool[i].gameObject.transform.position = Vector3.zero;
                    bigAsteroidPool[i].gameObject.SetActive(true);
                }
            }

            //move asteroids, and teleport them if needed.
            for (int i = 0; i < bigAsteroidPool.Count; i++)
            {
                if (bigAsteroidPool[i].gameObject.activeSelf)
                {
                if(bigAsteroidPool[i].spawnInvincibilityTimer > 0)
                {
                    bigAsteroidPool[i].spawnInvincibilityTimer -= Time.deltaTime;
                }
                MoveAsteroid(bigAsteroidPool[i].gameObject, bigAsteroidPool[i].direction, bigAsteroidPool[i].speed);
                    CheckForBounds(bigAsteroidPool[i]);            
                }
            }
            for (int i = 0; i < bigAsteroidPool.Count; i++)
            {
                for (int x = 0; x < bigAsteroidPool[i].LittleAsteroids.Length; x++)
                {
                    if (bigAsteroidPool[i].LittleAsteroids[x].gameObject.activeSelf)
                    {
                        MoveAsteroid(bigAsteroidPool[i].LittleAsteroids[x].gameObject, bigAsteroidPool[i].LittleAsteroids[x].direction, bigAsteroidPool[i].LittleAsteroids[x].speed);
                        CheckForBounds(bigAsteroidPool[i].LittleAsteroids[x]);                     
                    }
                }
            }



            for (int i = 0; i < bigAsteroidPool.Count; i++)
            {
                if (bigAsteroidPool[i].asteroidCollision.collided)
                {
                bigAsteroidPool[i].asteroidCollision.collided = false;

                    for (int x = 0; x < bigAsteroidPool[i].LittleAsteroids.Length; x++)
                    {
                        bigAsteroidPool[i].LittleAsteroids[x].gameObject.transform.position = bigAsteroidPool[i].gameObject.transform.position;
                        bigAsteroidPool[i].LittleAsteroids[x].gameObject.SetActive(true);
                        asteroidsInPlay++;

                    }
                OnDestroyedLargeAsteroid?.Invoke();
                    bigAsteroidPool[i].gameObject.SetActive(false);
                    SpawnParticlesInPlace(bigAsteroidPool[i].gameObject.transform.position);
                    cam.DOShakePosition(0.1f,0.5f,3,30);
                    asteroidsInPlay--;
                }





            }
            for (int x = 0; x < bigAsteroidPool.Count; x++)
            {
                for (int w = 0; w < bigAsteroidPool[x].LittleAsteroids.Length; w++)
                {
                    if (bigAsteroidPool[x].LittleAsteroids[w].asteroidCollision.collided && bigAsteroidPool[x].LittleAsteroids[w].gameObject.activeSelf)
                    {
                    OnDestroyedSmallAsteroid?.Invoke();
                        SpawnParticlesInPlace(bigAsteroidPool[x].LittleAsteroids[w].gameObject.transform.position);
                        bigAsteroidPool[x].LittleAsteroids[w].gameObject.SetActive(false);
                        asteroidsInPlay--;
                        cam.DOShakePosition(0.1f, 0.2f, 2, 10);

                    }
                }
            }
        }
        private void SpawnParticlesInPlace(Vector3 pos)
        {
            Instantiate(particleSystemObject, pos, Quaternion.identity);
        }


        private void MoveAsteroid(GameObject asteroid, Vector3 direction, float speed)
        {
            asteroid.transform.position += direction * Time.deltaTime * speed;
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
             bigAsteroidPool.Clear();
            for (int i = 0; i < MaxAsteroidCount; i++)
            {
                var instance = Instantiate(BigAsteroid);

                var randomDir1 = directions[UnityEngine.Random.Range(0, directions.Count)];
                randomDir1.x += UnityEngine.Random.Range(-1f, 1f);
                bigAsteroidPool.Add(new BigAsteroidData(instance, randomDir1.normalized, 4f, UnityEngine.Random.Range(1f,2.5f)));
               
                asteroidsInPlay++;
            }
            for (int i = 0; i < bigAsteroidPool.Count; i++)
            {
                bigAsteroidPool[i].collided = bigAsteroidPool[i].asteroidCollision.collided;
            bigAsteroidPool[i].spawnInvincibilityTimer = 2f;
                var randomDir2 = directions[UnityEngine.Random.Range(0, directions.Count)];
                // randomDir2.x += Random.Range(-1f, 1f);
                var randomDir3 = directions[UnityEngine.Random.Range(0, directions.Count)];
                // randomDir3.x += Random.Range(-1f, 1f);
                var randomDir4 = directions[UnityEngine.Random.Range(0, directions.Count)];
                // randomDir4.x += Random.Range(-1f, 1f);
                var randomDir5 = directions[UnityEngine.Random.Range(0, directions.Count)];
                // randomDir5.x += Random.Range(-1f, 1f);

                var littleAsteroidInstance1 = Instantiate(smallAsteroidBotLeft, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance2 = Instantiate(smallAsteroidBotRight, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance3 = Instantiate(smallAsteroidTopLeft, Vector3.zero, Quaternion.identity);
                var littleAsteroidInstance4 = Instantiate(smallAsteroidTopRight, Vector3.zero, Quaternion.identity);
                bigAsteroidPool[i].LittleAsteroids[0] = new LittleAsteroidData(littleAsteroidInstance1, randomDir2.normalized, 3, UnityEngine.Random.Range(1f, 2.5f));
                bigAsteroidPool[i].LittleAsteroids[1] = new LittleAsteroidData(littleAsteroidInstance2, randomDir3.normalized, 3, UnityEngine.Random.Range(1f, 2.5f));
                bigAsteroidPool[i].LittleAsteroids[2] = new LittleAsteroidData(littleAsteroidInstance3, randomDir4.normalized, 3, UnityEngine.Random.Range(1f, 2.5f));
                bigAsteroidPool[i].LittleAsteroids[3] = new LittleAsteroidData(littleAsteroidInstance4, randomDir5.normalized, 3, UnityEngine.Random.Range(1f, 2.5f));

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

