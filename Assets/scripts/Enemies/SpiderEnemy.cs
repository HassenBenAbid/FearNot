using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : Enemy
{
    private const int MISSLE_NUMBER_SPAWNED = 10;
    private const int ATTACK_NUMBER = 2;
    private const float MELEE_DISTANCE = 3.5f;
    private const float PREP_MELEE_DISTANCE = 3.5f;
    private const int CHILD_NUMBER = 4;
    private const float TIME_BTW_CHILD = 0.8f;

    [SerializeField] private GameObject missileAttack, meleeCollider, littleChildren;
    [SerializeField] private Transform firePos, minXPos, maxXPos;
    [SerializeField] private Transform anchorPos;
    [SerializeField] private float meleeSpeed = 800.0f;
    [SerializeField] private GameObject spiderWeb;

    private bool moveDown = false, moveUp = false, closeAttack = false, attackAnimation = false, attacking = false, startMelee = false;
    private Vector2 startPosition, stopPosition;
    private static List<GameObject> missilesList;
    private int attackCounter, currentAttack;
    private float waitTimer;
    private BoxCollider2D spiderCollider;

    private void OnEnable()
    {
        spiderWeb.SetActive(true);
    }

    protected override void Start()
    {
        base.Start();

        missilesList = new List<GameObject>();
        for (int i = 0; i < MISSLE_NUMBER_SPAWNED; i++)
        {
            missilesList.Add(Instantiate(missileAttack, transform.position, Quaternion.identity));
            missilesList[i].SetActive(false);
        }

        startPosition = transform.position;
        currentAttack = -1;
        moving = true;
        moveDown = true;
        spiderCollider = GetComponent<BoxCollider2D>();
        GetComponent<DistanceJoint2D>().enabled = true;
        rg.freezeRotation = true;
        GetComponent<DistanceJoint2D>().anchor = anchorPos.position;
        //waitTimer = Random.Range(standingIntervalMin, standingIntervalMax);
    }

    protected override void notDead()
    {
        base.notDead();

        if (!attacking && moving)
        {
            move();
            if (startMelee)
            {
                meleeAttack(stopPosition);
            }
        }

        if (!moving)
        {
            attack();
        }
    }

    protected override void attack()
    {
        if (attackCounter <= ATTACK_NUMBER && !attacking)
        {
            if (attackTimer <= 0)
            {
                attackTimer = attackCooldown;
                attacking = true;
    
                attackCounter++;
    
                if (attackCounter > ATTACK_NUMBER)
                {
                    moving = true;
                    moveUp = true;
                    attacking = false;
                    attackCounter = 0;
                }
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            if (attacking)
            {
                spitAttack();
            }
        }
    }

    private void spitAttack()
    {
        StartCoroutine(spitAttackWait());
    }

    protected override void move()
    {
        if (moveDown)
        {
            if (!spiderCollider.enabled)
            {
                spiderCollider.enabled = true;
            }
            movePosition(new Vector2(startPosition.x, startPosition.y - movingDistance), ref moveDown);
            if (!moveDown)
            {
                moving = false;
            }
        }else if (moveUp)
        {
            if (spiderCollider.enabled)
            {
                spiderCollider.enabled = false;
            }

            movePosition(new Vector2(startPosition.x, startPosition.y), ref moveUp);
            if (!moveUp)
            {
                int nextAction = chooseAttack(3);

                if (nextAction == 0)
                {
                    StartCoroutine(timeBtwAppearing());
                }else
                {
                    changingPosition();

                    if (nextAction == 1)
                    {
                        childAttack();
                    }else if (nextAction == 2)
                    {
                        stopPosition = transform.position;
                        startMelee = true;
                    }
                }
                
            }
        }
    }

    private void meleeAttack(Vector2 currentPosition)
    {
        if (!closeAttack)
        {
            movePosition(new Vector2(currentPosition.x, currentPosition.y - (MELEE_DISTANCE + PREP_MELEE_DISTANCE)), ref closeAttack, meleeSpeed);

            if (closeAttack)
            {
                StartCoroutine(hitMelee());
            }
        }else if (!attackAnimation)
        {
            movePosition(new Vector2(currentPosition.x, currentPosition.y), ref attacking);
            if (!attacking)
            {
                closeAttack = false;
                startMelee = false;
                moveDown = true;
            }
        }
    }

    private void movePosition(Vector2 newPosition, ref bool arrived, float currentSpeed = default)
    {
        if ((Vector2)transform.position != newPosition)
        {
            if (currentSpeed == default)
            {
                transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * currentSpeed);
            }

            if ((Vector2)transform.position == newPosition)
            {
                arrived = !arrived;
            }
        }
    }

    private void movePosition(Vector2 newPosition)
    {
        if ((Vector2)transform.position != newPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * speed);
        }
    }

    private void useBullet()
    {
        GameObject ob = missilesList[0];
        missilesList.Add(ob);
        missilesList.Remove(ob);

        ob.transform.position = (Vector2)firePos.position;
        if (Mathf.Sign(ob.transform.localScale.x) != Mathf.Sign(transform.localScale.x))
        {
            ob.transform.localScale = new Vector2(ob.transform.localScale.x * -1, ob.transform.localScale.y);
        }
        ob.SetActive(true);
    }

    private void changingPosition()
    {
        //float newXPos = Random.Range(minXPos.position.x, maxXPos.position.x);
        float newXPos = targetTransform.position.x;

        DistanceJoint2D distanceJo = GetComponent<DistanceJoint2D>();
        distanceJo.connectedAnchor = new Vector2(newXPos, distanceJo.connectedAnchor.y);
        transform.position = new Vector2(newXPos, transform.position.y);
        startPosition = transform.position;
    }

    private void childAttack()
    {
        StartCoroutine(spawnChildren());
    }

    protected override void isDead()
    {
        base.isDead();

        spiderWeb.SetActive(false);
        rg.freezeRotation = false;
        rg.constraints = RigidbodyConstraints2D.None;
        GetComponent<DistanceJoint2D>().enabled = false;
        LevelGenerator.addBossEnemy(littleChildren);
        LevelManager.Instance.restartBossTimer();
    }

    private IEnumerator hitMelee()
    {
        meleeCollider.SetActive(true);
        attackAnimation = true;
        anim.SetTrigger("melee");
        yield return new WaitForSeconds(1.0f);
        meleeCollider.SetActive(false);
        attackAnimation = false;
    }

    private IEnumerator timeBtwAppearing()
    {
        float timer = Random.Range(standingIntervalMin, standingIntervalMax);
        yield return new WaitForSeconds(timer);
        changingPosition();
        moveDown = true;
    }

    private IEnumerator spitAttackWait()
    {
        anim.SetTrigger("missile");
        attacking = false;
        yield return new WaitForSeconds(0.3f);
        useBullet();
    }

    private IEnumerator spawnChildren()
    {
        for(int i=0; i<CHILD_NUMBER; i++)
        {
            Vector2 childPos = new Vector2(Random.Range(minXPos.position.x, maxXPos.position.x), minXPos.position.y);
            Instantiate(littleChildren, childPos, Quaternion.identity);
            yield return new WaitForSeconds(TIME_BTW_CHILD);
        }

        spiderCollider.enabled = true;
        moveDown = true;
    }


}
