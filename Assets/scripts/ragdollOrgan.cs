using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ragdollOrgan : MonoBehaviour
{
    [SerializeField] private bool cutOrgan = true;

    private HingeJoint2D hj;

    private void Start()
    {
        hj = GetComponent<HingeJoint2D>();
    }

    public void hit()
    {
        if (cutOrgan)
        {
            hj.enabled = false;
        }
    }


}
