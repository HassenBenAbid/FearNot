using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private int weaponNumber;

    Rigidbody2D rg;

    private void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    public int getPickType()
    {
        return weaponNumber;
    }

    public void picked()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            rg.gravityScale = 0;
            rg.velocity = Vector2.zero;
        }
    }
}
