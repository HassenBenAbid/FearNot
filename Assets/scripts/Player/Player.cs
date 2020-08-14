using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private const KeyCode JUMP_KEY = KeyCode.Space;
    private const KeyCode SLOW_MOTION_KEY = KeyCode.E;
    private const KeyCode FIRE_KEY = KeyCode.Mouse0;
    private const KeyCode PICK_KEY = KeyCode.E;
    private const KeyCode UNPICK_KEY = KeyCode.G;
    private const float SLOW_TIME = 0.6f;
    private const int MAX_HEALTH = 4;
    private const float INVINCIBILITY_TIMER = 0.2f;

    private const int SPECIAL_INDEX_HEALTH = 0;
    private const int SPECIAL_INDEX_TIME = 1;


    [SerializeField] private Transform armWithWeapon, voicesPos;
    [SerializeField] private float speed = 50.0f, jumpForce = 15.0f, slowMotionDuration = 1.5f;
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private int health = 3;
    [SerializeField] private WeaponsUI gunsUI;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private TextMeshPro voicesUI;
    [SerializeField] private SpriteRenderer head;

    private Rigidbody2D rg;
    private Animator anim;
    private bool grounded = true, gunPicked = false, timeSlowed = false, dead = false, canPick = false, invincible = false;
    private bool invincibilityStarted = false;
    private Pickable currentPickable;
    private Weapon currentWeapon;
    private float slowMotionTimer = 0, originalGravityScale;
    private int currentWeaponIndex = 0;
    private int maxHealth;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        originalGravityScale = rg.gravityScale;

        gunPicked = true;
    }

    private void OnEnable()
    {
        maxHealth = MAX_HEALTH;
        currentWeaponIndex = UserUnlockable.getActiveWeapon();

        equipeGun();
        specialEffect();

        healthUI.setHearts(health);
    }

    private void Update()
    {

        if (!dead && LevelManager.Instance.canPlay())
        {

            jump();

            fire();

            slowMotion();

            //putWeaponDown();

            //giveMeGun();

            ammoUI();

            updateVoicesPos();
        }
        else if (dead)
        {
            isDead();
        }
    }

    private void FixedUpdate()
    {
        if (!dead && LevelManager.Instance.canPlay())
        {

            move();

            slowMotion();

            armFollowMouse();

        }

        Debug.Log(currentWeaponIndex);

    }

    private void equipeGun()
    {
        gunsUI.gameObject.SetActive(true);

        for(int i=0; i<weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        weapons[currentWeaponIndex].gameObject.SetActive(true);
        currentWeapon = weapons[currentWeaponIndex];
        gunsUI.setWeapon(currentWeaponIndex, currentWeapon.getCurrentAmmo());
    }

    private void updateVoicesPos()
    {

       voicesUI.transform.position = (Vector2)voicesPos.position + new Vector2(8.5f, -1.1f);
    }

    private void move()
    {
        float delta = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        Vector2 mv = new Vector2(delta, rg.velocity.y);
        rg.velocity = new Vector2(delta, rg.velocity.y); ;
        if (delta != 0 && Mathf.Sign(delta) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
        anim.SetFloat("speed", Mathf.Abs(delta));
    }

    private void jump()
    {
        if (grounded && Input.GetKeyDown(JUMP_KEY))
        {
            grounded = false;
            rg.gravityScale = 1;
            rg.AddForce(new Vector2(0, jumpForce));
            anim.SetBool("grounded", grounded);
        }

        if (grounded && rg.gravityScale == 1)
        {
            rg.gravityScale = originalGravityScale;
        }
    }

    private void fire()
    {
        
        if (gunPicked)
        {
            if (currentWeaponIndex == 1)
            {
                if (Input.GetKey(FIRE_KEY))
                {
                    currentWeapon.fire((int)Mathf.Sign(transform.localScale.x));
                }
            }
            else
            {
                if (Input.GetKeyDown(FIRE_KEY))
                {
                    currentWeapon.fire((int)Mathf.Sign(transform.localScale.x));
                }
            }
        }

    }

    private void ammoUI()
    {
        if (gunPicked)
        {
            gunsUI.refreshAmmo(currentWeapon.getCurrentAmmo());
        }
    }

    private void armFollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 armPos = Camera.main.WorldToScreenPoint(armWithWeapon.position);
        Vector2 distance = mousePos - armPos;
        float angle = Mathf.Atan2(distance.x, distance.y * -1) * Mathf.Rad2Deg;
        armWithWeapon.rotation = Quaternion.Euler(new Vector3(0, 0, (angle - (30.0f * Mathf.Sign(transform.localScale.x)))));
    }

    private void slowMotion()
    {
        if (Input.GetKeyDown(SLOW_MOTION_KEY))
        {
            if (!grounded && !timeSlowed && slowMotionTimer <= 0)
            {
                Time.timeScale = SLOW_TIME;
                timeSlowed = true;
                slowMotionTimer = slowMotionDuration * SLOW_TIME;
            }
        }

        if (slowMotionTimer <= 0 || grounded)
        {
            Time.timeScale = 1.0f;
            if (grounded)
            {
                timeSlowed = false;
            }
        }

        slowMotionTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            grounded = true;
            anim.SetBool("grounded", grounded);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "weapon")
        {
            canPick = true;
            currentPickable = collision.GetComponent<Pickable>();
        }

        if (collision.tag == "Item")
        {
            Items currentItem = collision.GetComponent<Items>();

            if (currentItem.GetType() == typeof(Health))
            {
                if(health < maxHealth)
                {
                    currentItem.startEffect();
                }
            }else
            {
                currentItem.startEffect();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "weapon")
        {
            canPick = false;
            currentPickable = null;
        }
    }

    //private void giveMeGun()
    //{
    //    if (canPick)
    //    {
    //        if (Input.GetKeyDown(PICK_KEY))
    //        {
    //            if (gunPicked)
    //            {
    //                unpickGun();
    //            }
    //
    //            pickGun(currentPickable);
    //        }
    //    }
    //}

    //private void pickGun(Pickable fakeGun)
    //{
    //    currentWeaponIndex = fakeGun.getPickType();
    //    currentWeapon = weapons[currentWeaponIndex].GetComponent<Weapon>();
    //    weapons[currentWeaponIndex].SetActive(true);
    //    gunPicked = true;
    //    fakeGun.picked();
    //    gunsUI.setWeapon(currentWeaponIndex, currentWeapon.getCurrentAmmo());
    //}
    //
    //private void unpickGun()
    //{
    //    weapons[currentWeaponIndex].SetActive(false);
    //    currentWeaponIndex = -1;
    //    currentWeapon.spawnFake(transform.position);
    //    currentWeapon = null;
    //    gunPicked = false;
    //    gunsUI.setWeapon(currentWeaponIndex);
    //}
    //
    //private void putWeaponDown()
    //{
    //    if (Input.GetKeyDown(UNPICK_KEY) && currentWeapon != null)
    //    {
    //        unpickGun();
    //    }
    //}

    private void isDead()
    {
        anim.enabled = false;
        LevelManager.Instance.gameOver();
    }

    public void oneShotKill()
    {
        dead = true;
        health = 0;
        isDead();
    }

    public void hit()
    {
        if (!dead)
        {

            if (health > 0 && !invincible)
            {
                health--;
                healthUI.heartHit(1);

                if (health <= 0)
                {
                    dead = true;
                }else
                {
                    if (!invincibilityStarted)
                    {
                        StartCoroutine(hitInvincibility());
                    }
                }
            }
        }
    }

    public void stop()
    {
        rg.velocity = Vector2.zero;
        anim.SetFloat("speed", 0);
    }

    public void addHealth()
    {
        if (health < maxHealth )
        {
            health++;
            healthUI.addHeart();
        }
    }

    public bool isPlayerDead()
    {
        return dead;
    }

    private IEnumerator hitInvincibility()
    {
        invincibilityStarted = true;
        yield return new WaitForSeconds(0.1f);
        invincible = true;  
        for(int i = 0; i<4; i++)
        {
            head.enabled = false;
            yield return new WaitForSeconds(INVINCIBILITY_TIMER);
            head.enabled = true;
            yield return new WaitForSeconds(INVINCIBILITY_TIMER);
        }
        invincibilityStarted = false;
        invincible = false;
    }

    //special effect

    private void specialEffect()
    {
        if (UserUnlockable.isItemActive(SPECIAL_INDEX_HEALTH))
        {
            fullHealth();
        }

        if (UserUnlockable.isItemActive(SPECIAL_INDEX_TIME))
        {
            moreTime();
        }

        UserUnlockable.disableAllItems();
    }

    private void fullHealth()
    {
        maxHealth++;
        health = maxHealth;

    }

    private void moreTime()
    {
        LevelManager.Instance.moreTime();
    }

}
