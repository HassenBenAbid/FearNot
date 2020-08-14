using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlatform : Platform
{
    private const float WAIT_BEFORE_START = 1.0f;

    [SerializeField] private LevelTrigger activationTrigger;
    [SerializeField] private Enemy boss;
    [SerializeField] private wallBlocker rightWall, leftWall;
    [SerializeField] private Transform cameraPos;

    private MainCamera currentCamera;
    private bool started, ended;

    private void Start()
    {
        currentCamera = Camera.main.GetComponent<MainCamera>();

        started = false;
        ended = false;
    }

    private void Update()
    {
        if (!started)
        {
            if (activationTrigger.playerInside())
            {
                startBoss();
            }
        }

        if (!ended && started)
        {
            if (boss.isHeDead())
            {
                endBoss();
            }
        }
    }

    private void startBoss()
    {
        StartCoroutine(starting());
    }
    
    private void endBoss()
    {
        rightWall.wallOut();
        leftWall.wallOut();

        currentCamera.startFollow();

        ended = true;

        LevelGenerator.SpawningBE(true);
    }

    private IEnumerator starting()
    {
        started = true;

        LevelGenerator.SpawningBE(false);
        currentCamera.stopFollow();
        currentCamera.transform.position = cameraPos.position;

        rightWall.gameObject.SetActive(true);
        leftWall.gameObject.SetActive(true);

        activationTrigger.gameObject.SetActive(false);

        yield return new WaitForSeconds(WAIT_BEFORE_START);

        boss.gameObject.SetActive(true);
    }

}
