using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using rds;

public class LevelGenerator : MonoBehaviour
{
    private const float DISTANCE_BTW_PLATFORMS_X = 2.0f;
    private const float DISTANCE_BTW_PLATFORMS_Y = 1.8f;
    private const float TIME_BTW_BOSSES = 30.0f;
    private const int SPIDER_BOSS_INDEX = 0;
    private const int SKULLHEAD_BOSS_INDEX = 1;

    [SerializeField] private List<GameObject> platformsType;
    [SerializeField] private List<RDSUnityObject> enemiesType;
    [SerializeField] private List<GameObject> bossesPlatforms;
    [SerializeField] private int platformsGeneratedNumber = 5;
    [SerializeField] private Transform startPosition, playerPosition, littleSkullSpawner;
    [SerializeField] private Vector2 timeBtwBossEnemies;

    private Vector2 lastPosition;
    private float cameraWidth, cameraHeight, spawnBETimer;
    private bool firstGeneration = true;
    private RDSTable enemiesTable;
    private int currentBossIndex;
    private static List<GameObject> bossEnemies;
    private static bool canSpawnBE = true;
    public static bool bossSpawned = false;

    private void Start()
    {
        createEnemiesTable();
        lastPosition = startPosition.position;
        cameraHeight = Camera.main.orthographicSize * 2.0f;
        cameraWidth = cameraHeight * Camera.main.aspect;

        generate();
        firstGeneration = false;
        currentBossIndex = 0;

        bossEnemies = new List<GameObject>();
        spawnBETimer = Random.Range(timeBtwBossEnemies.x, timeBtwBossEnemies.y);
    }

    private void generate()
    {
        for(int i=0; i<platformsGeneratedNumber; i++)
        {
            int platIndex = Random.Range(0, platformsType.Count);

            if (!firstGeneration || i != 0)
            {
                lastPosition.x += (platformsType[platIndex].GetComponent<SpriteRenderer>().size.x / 2.0f *platformsType[platIndex].transform.localScale.x) - 1.2f  ;
            }

            Vector2 currentPosition = new Vector2(Random.Range(lastPosition.x + 0.2f, lastPosition.x + DISTANCE_BTW_PLATFORMS_X), Random.Range(startPosition.position.y - DISTANCE_BTW_PLATFORMS_Y, startPosition.position.y+ DISTANCE_BTW_PLATFORMS_Y));
            
            GameObject currentPlat = Instantiate(platformsType[platIndex], currentPosition, Quaternion.identity);
            lastPosition = currentPosition;
            lastPosition.x += currentPlat.GetComponent<SpriteRenderer>().size.x/2.0f * currentPlat.transform.localScale.x ;

            generateEnemies(currentPlat.GetComponent<Platform>().enemiesPosition);
        }
    }

    private void generateBoss()
    {
        lastPosition.x += (bossesPlatforms[currentBossIndex].GetComponent<SpriteRenderer>().size.x / 2.0f * bossesPlatforms[currentBossIndex].transform.localScale.x) - 1.2f;

        Vector2 currentPosition = new Vector2(Random.Range(lastPosition.x + 0.2f, lastPosition.x + DISTANCE_BTW_PLATFORMS_X), Random.Range(startPosition.position.y - DISTANCE_BTW_PLATFORMS_Y, startPosition.position.y + DISTANCE_BTW_PLATFORMS_Y));

        GameObject currentPlat = Instantiate(bossesPlatforms[currentBossIndex], currentPosition, Quaternion.identity);

        lastPosition = currentPosition;
        lastPosition.x += currentPlat.GetComponent<SpriteRenderer>().size.x / 2.0f * currentPlat.transform.localScale.x;

        currentBossIndex++;
    }

    private void detectCameraPos()
    {
        if (Camera.main.transform.position.x + (cameraWidth) >= lastPosition.x)
        {
            if  (!bossSpawned && currentBossIndex < bossesPlatforms.Count && (TIME_BTW_BOSSES <= LevelManager.Instance.currentBossTimer()))
            {
                LevelManager.Instance.stopBossTimer();
                bossSpawned = true;
                generateBoss();
            }else
            {
                generate();
            }
        }
    }

    private void Update()
    {
        detectCameraPos();

        if (canSpawnBE)
        {
            generateBossEnemies();
        }
    }

    private void generateEnemies(List<Transform> enemiesPosition)
    {

        for(int i=0; i<enemiesPosition.Count; i++)
        {
            GameObject currentEnemy = ((RDSUnityObject)enemiesTable.rdsResult.ElementAt(0)).gameObject;
            currentEnemy = Instantiate(currentEnemy, enemiesPosition[i].position, Quaternion.identity);
            int enemyLook = 0;
            do
            {
                enemyLook = Random.Range(-1, 2);
            } while (enemyLook == 0);
                
            currentEnemy.transform.localScale = new Vector2(currentEnemy.transform.localScale.x * enemyLook, currentEnemy.transform.localScale.y); 
        }
    }

    private void generateBossEnemies()
    {
        if (bossEnemies.Count > 0)
        {
            if (spawnBETimer <= 0)
            {
                int chooseSpawner = Random.Range(0, bossEnemies.Count);

                if(chooseSpawner == SPIDER_BOSS_INDEX)
                {
                    spawnSpiders();
                }
                else if (chooseSpawner == SKULLHEAD_BOSS_INDEX)
                {
                    spawnSkulls();
                }

                spawnBETimer = Random.Range(timeBtwBossEnemies.x, timeBtwBossEnemies.y);
            }
            else
            {
                spawnBETimer -= Time.deltaTime;
            }
        }
    }

    private void createEnemiesTable()
    {
        enemiesTable = new RDSTable();

        for(int i=0; i<enemiesType.Count; i++)
        {
            enemiesTable.addEntry(enemiesType[i]);
        }
    }

    private void spawnSpiders()
    {
        Vector2 enemyPosition = new Vector2(playerPosition.position.x, cameraHeight);
        GameObject currentObject = Instantiate(bossEnemies[SPIDER_BOSS_INDEX], enemyPosition, Quaternion.identity);
    }

    private void spawnSkulls()
    {
        Vector2 spawnPosition = new Vector2(littleSkullSpawner.position.x, playerPosition.position.y);
        GameObject currentObject = Instantiate(bossEnemies[SKULLHEAD_BOSS_INDEX], spawnPosition, Quaternion.identity);
    }

    public int getCurrentBoss()
    {
        return currentBossIndex;
    }

    public static void addBossEnemy(GameObject theEnemy)
    {
        bossEnemies.Add(theEnemy);
    }

    public static void SpawningBE(bool answer)
    {
        canSpawnBE = answer;
    }

}
