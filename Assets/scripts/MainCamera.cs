using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform backLimit, frontLimit, topLimit, botLimit;
    [SerializeField] private float timeSmooth = 1.5f;
    [SerializeField] private Transform startLimit, fallLimit, jumpLimit;

    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPos;
    private Camera mainCamera;
    private bool followP = true;

    private void Start()
    {
        targetPos = transform.position;
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (followP)
        {
            follow();
        }
    }

    public void stopFollow()
    {
        followP = false;
    }

    public void startFollow()
    {
        followP = true;
    }

    private void follow()
    {
        if ((transform.position.x - getHalfWidth() <= startLimit.position.x) || (transform.position.y - getHalfHeight() <= fallLimit.position.y) || (transform.position.y + getHalfHeight() >= jumpLimit.position.y))
        {
            if (targetPos.x - getHalfWidth() <= startLimit.position.x) 
            {
                transform.position = new Vector3(startLimit.position.x + getHalfWidth(), transform.position.y, transform.position.z);
            }
            else if (targetPos.y - getHalfHeight() <= fallLimit.position.y) {
                transform.position = new Vector3(transform.position.x, fallLimit.position.y + getHalfHeight(), transform.position.z);
            }
            else if (targetPos.y + getHalfHeight() >= jumpLimit.position.y)
            {
                transform.position = new Vector3(transform.position.x, jumpLimit.position.y - getHalfHeight(), transform.position.z);
            }
        }

        if ((target.localScale.x > 0 && target.position.x >= frontLimit.position.x) || (target.localScale.x < 0 && target.position.x <= backLimit.position.x) || (target.position.y >= topLimit.position.y) || (target.position.y <= botLimit.position.y))
        {
            targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity,timeSmooth);

    }

    private float getHalfHeight()
    {
        return mainCamera.orthographicSize;
    }

    private float getHalfWidth()
    {
        return getHalfHeight() * mainCamera.aspect;
    } 
}
