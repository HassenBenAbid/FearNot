using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private const float TIME_BEFORE_DISABLE = 0.5f;

    [SerializeField] private GameObject blood;

    private BoxCollider2D bCollider;
    private bool playerHit = false;

    private void Start()
    {
        bCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(disable());
        playerHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (!playerHit && p)
        {
            p.hit();
            playerHit = true;
            Instantiate(blood, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator disable()
    {
        yield return new WaitForSeconds(TIME_BEFORE_DISABLE);
        gameObject.SetActive(false);
    }
}
