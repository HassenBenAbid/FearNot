using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private List<Rigidbody2D> RBodies;
    [SerializeField] private List<HingeJoint2D> hJoints;
    [SerializeField] private List<BoxCollider2D> bCollider;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        deactiveRigdoll();
    }

    private void Update()
    {
        if (!anim.enabled)
        {
            activeRigdoll();
        }
    }

    private void activeRigdoll()
    {
        for (int i = 0; i < RBodies.Count; i++)
        {
            RBodies[i].isKinematic = false;
            hJoints[i].enabled = true;
            bCollider[i].enabled = true;
        }
    }


    private void deactiveRigdoll()
    {
        for (int i=0; i < RBodies.Count; i++)
        {
            hJoints[i].enabled = false;
            RBodies[i].isKinematic = true;
            bCollider[i].enabled = false;
        }
    }
}
