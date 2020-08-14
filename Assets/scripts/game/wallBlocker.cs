using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBlocker : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void wallOut()
    {
        anim.SetTrigger("out");
    }
}
