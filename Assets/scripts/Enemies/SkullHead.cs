using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullHead :Enemy
{
    private const int BULLET_NUMBER = 4;
    private const int RAIN_BULLET_NUMBER = 15;
    private const float RUSH_SPEED = 9;
    private const float TIME_BTW_RAIN = 0.3f;
    private const float TIME_BEFORE_START = 1.5f;
    private const float PAUSE_BEFORE_RUSHING = 0.2f;

    [SerializeField] private DestinationBullet eyeBullet;
    [SerializeField] private Transform minFieldLimit, maxFieldLimit;
    [SerializeField] private List<Transform> firePosition;
    [SerializeField] private Vector2 timeBtwRain;
    [SerializeField] private GameObject littleChild;

    private List<GameObject> bulletList;
    private List<DestinationBullet> rainBulletList;
    private bool rushing = false, towardMoving = false, fightStarted = false, raining = false, startRushing = false;
    private Vector2 rushDirection, beforeRushPosition;
    private float rainTimer;

    protected override void Start()
    {
        base.Start();

        bulletList = new List<GameObject>();
        for(int i = 0; i< BULLET_NUMBER ; i++)
        {
            GameObject currentObject = Instantiate(eyeBullet.gameObject, transform.position, Quaternion.identity);
            bulletList.Add(currentObject);
            bulletList[i].SetActive(false);
        }

        rainBulletList = new List<DestinationBullet>();
        for(int i = 0; i<RAIN_BULLET_NUMBER; i++)
        {
            DestinationBullet currentObject = Instantiate(eyeBullet, transform.position, Quaternion.Euler(0, 0, 90.0f));
            currentObject.gameObject.SetActive(false);
            currentObject.goToPlayer(false, Vector2.down);
            currentObject.transform.rotation = Quaternion.Euler(0, 0, 90.0f);
            rainBulletList.Add(currentObject);
        }

        attackTimer = Random.Range(standingIntervalMin, standingIntervalMax);
        rainTimer = Random.Range(timeBtwRain.x, timeBtwRain.y);

        StartCoroutine(startingFight());
    }

    protected override void notDead()
    {
        if (fightStarted)
        {
            base.notDead();

            if (rushing)
            {
                rushAttack();
            }else
            {
                attack();
            }

            rainAttack();
        }

    }


    protected override void move()
    {
        if (!rushing)
        {
            Vector2 destination = Vector2.zero;
            if (!towardMoving) {
                destination = new Vector2(minFieldLimit.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                if ((Vector2)transform.position == destination)
                {
                    towardMoving = true;
                }
            }else
            {
                destination = new Vector2(maxFieldLimit.position.x, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                if ((Vector2)transform.position == destination)
                {
                    towardMoving = false;
                }
            }
        }
    }

    protected override void attack()
    {
        if (attackTimer <= 0)
        {
            int attackIndex = chooseAttack(2);

            if (attackIndex == 0)
            {
                eyeAttack();

            }else if (attackIndex == 1)
            {
                startingRushAttack();
            }

            attackTimer = Random.Range(standingIntervalMin, standingIntervalMax);
        }else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void eyeAttack()
    {
        spawnEyeBullet(firePosition[0].position); 
        spawnEyeBullet(firePosition[1].position); 
    }

    private void spawnEyeBullet(Vector2 position)
    {
        GameObject currentObject = useObject(bulletList);
        currentObject.transform.position = position;
        currentObject.SetActive(true);
    }

    private void startingRushAttack()
    {
        if (!rushing)
        {
            StartCoroutine(pauseBeforeRush());
            rushDirection = targetTransform.position;
            beforeRushPosition = transform.position;
            rushing = true;
        }
    }

    private void rushAttack()
    {
        if (startRushing)
        {
            bool arrived = startRush(rushDirection);

            if (arrived && rushDirection == beforeRushPosition)
            {
                rushing = false;
                startRushing = false;
            }

            if (arrived && rushDirection != beforeRushPosition)
            {
                rushDirection = beforeRushPosition;
            }
        }

    }

    private void rainAttack()
    {
        if (!raining)
        {
            if (rainTimer <= 0)
            {
                StartCoroutine(rain());
                rainTimer = Random.Range(timeBtwRain.x, timeBtwRain.y);
            }
            else
            {
                rainTimer -= Time.deltaTime;
            }
        }

    }

    private bool startRush(Vector2 position)
    {
        transform.position = Vector2.MoveTowards(transform.position, position, RUSH_SPEED * Time.deltaTime);

        if((Vector2)transform.position == position)
        {
            return true;
        }else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target.hit();
        }
    }

    private IEnumerator rain()
    {
        int chooseDirection = Random.Range(0, 2);
        Debug.Log(chooseDirection);
        Vector2 startPosition = Vector2.zero;
        Vector2 endPosition = Vector2.zero;
        raining = true;

        if (chooseDirection == 0)
        {
            startPosition = minFieldLimit.position;
            endPosition = maxFieldLimit.position;

            for (float i = startPosition.x; i <= endPosition.x; i += -Mathf.Sign(startPosition.x) * 2.2f)
            {
                DestinationBullet currentObejct = useObject(rainBulletList);
                currentObejct.transform.position = new Vector2(i, startPosition.y);
                currentObejct.gameObject.SetActive(true);

                yield return new WaitForSeconds(TIME_BTW_RAIN);
            }
        }
        else
        {
            startPosition = maxFieldLimit.position;
            endPosition = minFieldLimit.position;

            for (float i = startPosition.x; i >= endPosition.x; i += -Mathf.Sign(startPosition.x) * 2.2f)
            {
                DestinationBullet currentObejct = useObject(rainBulletList);
                currentObejct.transform.position = new Vector2(i, startPosition.y);
                currentObejct.gameObject.SetActive(true);

                yield return new WaitForSeconds(TIME_BTW_RAIN);
            }
        }

        raining = false;

    }

    private IEnumerator startingFight()
    {
        invincible = true;
        yield return new WaitForSeconds(TIME_BEFORE_START);
        invincible = false;
        fightStarted = true;
        anim.enabled = false;
    }

    private IEnumerator pauseBeforeRush()
    {
        yield return new WaitForSeconds(PAUSE_BEFORE_RUSHING);

        startRushing = true;
    }

    protected override void isDead()
    {
        base.isDead();

        LevelGenerator.addBossEnemy(littleChild);
        LevelManager.Instance.restartBossTimer();
    }

}
