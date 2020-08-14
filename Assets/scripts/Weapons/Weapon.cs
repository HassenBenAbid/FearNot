using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Weapon : MonoBehaviour
{
    private const int BULLET_LIST_SIZE = 40;
    private const float DEFAULT_GRAVITY = 1.0f;
    private const float FLASH_DURATION = 0.15f;

    [SerializeField] protected Transform firePos;
    [SerializeField] protected GameObject bullet, fakeOne;
    [SerializeField] protected float fireCooldown = 0.5f, reloadTime = 1.0f, bulletLifeTime = 2.0f;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private int maxClipAmmo = 30;
    [SerializeField] private bool weaponReload = true;
    [SerializeField] private int weaponIndex;

    protected int currentClipAmmo;
    protected bool isReloading = false;
    protected float reloadingTimer;

    protected static List<GameObject> bulletList;
    protected float fireTimer, flashTimer = 0;
    protected Rigidbody2D rg;
    private Vector2 unpickPush = new Vector2(50.0f, 100.0f);
    protected AudioSource fireSound;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        if (bulletList == null)
        {
            bulletList = new List<GameObject>();
            for (int i = 0; i < BULLET_LIST_SIZE; i++)
            {
                bulletList.Add(Instantiate(bullet));
                bulletList[i].SetActive(false);
                DontDestroyOnLoad(bulletList[i]);
            }
        }

        fireSound = GetComponent<AudioSource>();
    }

    /*private void OnLevelWasLoaded(int level)
    {
        bulletList = null;
    }*/

    protected virtual void OnEnable()
    {
        muzzleFlash.SetActive(false);
        currentClipAmmo = maxClipAmmo;
    }

    protected virtual void FixedUpdate()
    {
        flashTimer -= Time.deltaTime;
        fireTimer += Time.deltaTime;
        if (flashTimer <= 0 && muzzleFlash.activeSelf)
        {
            muzzleFlash.SetActive(false);
        }

        if (weaponReload)
        {
            reload();
        }
    }

    public void unpick()
    {
        GameObject go = Instantiate(fakeOne, transform);
        go.GetComponent<Rigidbody2D>().AddForce(unpickPush);
    }

    protected GameObject useObject()
    {
      GameObject ob = bulletList[0];
      bulletList.Add(ob);
      bulletList.Remove(ob);
    
      return ob;
    }

    public virtual void fire(int playerDirection, bool isPlayer=true) {
        flash();
    }

    private void flash()
    {
        muzzleFlash.SetActive(true);
        flashTimer = FLASH_DURATION;
    }

    protected void reload()
    {
        if (isReloading && reloadingTimer <= 0)
        {
            isReloading = false;
            currentClipAmmo = maxClipAmmo;
        }
        else if (isReloading && reloadingTimer > 0)
        {
            reloadingTimer -= Time.deltaTime;
        }
    }

    public void spawnFake(Vector2 position)
    {
        Instantiate(fakeOne, position, Quaternion.identity);
    }

    public int getCurrentAmmo()
    {
        return currentClipAmmo;
    }

    public int getWeaponIndex()
    {
        return weaponIndex;
    }
}
