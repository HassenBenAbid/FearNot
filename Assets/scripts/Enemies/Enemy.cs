using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rds;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    private const float TIME_BEFORE_DESTROY = 10.0f;
    private const float DROP_WEAPON_CHANCE = 0.25f;
    private const float DROP_HEALTH_CHANCE = 0.40f;
    private const float DROP_DRUG_CHANCE = 0.35f;

    [SerializeField] private int maxHealth = 2;
    [SerializeField] private float raycastDistance = 10.0f;
    [SerializeField] protected float attackCooldown = 1.5f, speed = 250.0f, movingDistance = 5.0f;
    [SerializeField] private LayerMask ownLayer;
    [SerializeField] protected float standingIntervalMin = 1.5f, standingIntervalMax = 3.0f;
    [SerializeField] private List<Items> dropables;

    protected bool playerDetected = false, dead = false, moving = false, playedDead = false;
    protected Transform targetTransform;
    private int health;
    protected float attackTimer = 0;
    protected Rigidbody2D rg;
    protected Animator anim;
    private float standingTimer = 0;
    private Ragdoll doll;
    private bool deathAnim = false;
    private RDSTable dropTable;
    protected Player target;
    private float standingInterval;
    protected int veryOldAttack, oldAttack;
    protected bool invincible;

    protected virtual void Start()
    {
        //target = GameObject.FindWithTag("Player").GetComponent<Player>();
        target = GameObject.Find("player").GetComponent<Player>();
        //targetTransform = GameObject.FindWithTag("Player").transform;
        targetTransform = GameObject.Find("player").transform;
        health = maxHealth;
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        doll = GetComponent<Ragdoll>();

        dropTable = new RDSTable();
        createDropTable();
        playedDead = false;
        attackTimer = attackCooldown/3.0f;
        standingInterval = Random.Range(standingIntervalMin, standingIntervalMax);

        veryOldAttack = -1;
        oldAttack = -1;
        invincible = false;
    }

    private void FixedUpdate()
    {
        if (!dead && LevelManager.Instance.canPlay())
        {
            notDead();
        }else if (dead && !playedDead)
        {
            isDead();
        }
    }

    virtual protected void notDead()
    {
        detectPlayer();
        move();
    }

    private void detectPlayer()
    {
        if (!playerDetected && !target.isPlayerDead())
        {
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, transform.right * Mathf.Sign(transform.localScale.x), raycastDistance, ~ownLayer);
            Debug.DrawRay(transform.position, transform.right * Mathf.Sign(transform.localScale.x) * raycastDistance, Color.red);
            if (hit && hit.collider.tag == "Player")
            {
                playerDetected = true;
            }

            if (health < maxHealth)
            {
                playerDetected = true;
            }
        }

        if (target.isPlayerDead() && playerDetected)
        {
            playerDetected = false;
        }
    }

    virtual public void hit()
    {
        if (health > 0 && !invincible)
        {
            health--;

            if (health <= 0)
            {
                dead = true;
            }
        }
    }

    protected virtual void isDead()
    {
        if (doll)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Animator>().enabled = false;
        }
        else if (!deathAnim)
        {
            anim.enabled = true;
            anim.SetTrigger("dead");
            deathAnim = true;
        }
       drop();
       Destroy(this.gameObject, TIME_BEFORE_DESTROY);
       playedDead = true;
    }

    virtual protected void move() {
        if (playerDetected)
        {
            float distance = transform.position.x - targetTransform.position.x;
            if ((distance > 0 && transform.localScale.x > 0) || (distance < 0 && transform.localScale.x < 0))
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }

            if (Mathf.Abs(distance) > movingDistance)
            {
                rg.velocity = new Vector2(speed * Time.deltaTime * -Mathf.Sign(distance), rg.velocity.y);
                moving = true;

            }
            else
            {
                rg.velocity = Vector2.zero;
                moving = false;
            }
        }
        else
        {
            if (standingTimer >= standingInterval)
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                standingTimer = 0;
                standingInterval = Random.Range(standingIntervalMin, standingIntervalMax);
            }
            else
            {
                standingTimer += Time.deltaTime;
            }
        }

        anim.SetBool("moving", moving);
    }


    virtual protected void attack() { }

    public bool isHeDead()
    {
        return dead;
    }

    private void drop()
    {

        if (dropTable != null)
        {
            IEnumerable<IRDSOBJECT> currentRDS = dropTable.rdsResult;
            foreach (IRDSOBJECT currentObject in currentRDS)
            {
                if (!(currentObject is RDSNull))
                {
                    GameObject currentDrop = ((Items)currentObject).gameObject;
                    Instantiate(currentDrop, transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void createDropTable()
    {
        dropTable.clearContent();

        foreach(Items currentItem in dropables)
        {
            dropTable.addEntry(currentItem);
        }

        RDSNull noDrop = new RDSNull(40);
        dropTable.addEntry(noDrop);

    }


    protected T useObject<T>(List<T> bulletList)
    {
        T ob = bulletList[0];
        bulletList.Add(ob);
        bulletList.Remove(ob);

        return ob;
    }

    protected int chooseAttack(int maxAttack)
    {
        int attackIndex = -1;

        do
        {
            attackIndex = Random.Range(0, maxAttack);
        } while (attackIndex == oldAttack && attackIndex == veryOldAttack);

        veryOldAttack = oldAttack;
        oldAttack = attackIndex;

        return attackIndex;
    }

}
