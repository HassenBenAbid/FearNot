using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private const float HALF_BG_WIDTH = 20.0f;

    [SerializeField] private List<GameObject> backgrounds;

    private Vector2 cameraStartPosition;

    private void Start()
    {
        cameraStartPosition = Camera.main.transform.position;
    }

    private void Update()
    {
        moveBg();
    }

    private void moveBg()
    {
        if (Camera.main.transform.position.x >= cameraStartPosition.x + HALF_BG_WIDTH)
        {
            backgrounds[0].transform.position = new Vector2(backgrounds[backgrounds.Count - 1].transform.position.x + HALF_BG_WIDTH, backgrounds[backgrounds.Count - 1].transform.position.y);
            firstToLast(backgrounds);
            cameraStartPosition = Camera.main.transform.position;
        }else if (Camera.main.transform.position.x <= cameraStartPosition.x - HALF_BG_WIDTH)
        {
            backgrounds[backgrounds.Count - 1].transform.position = new Vector2(backgrounds[0].transform.position.x - HALF_BG_WIDTH, backgrounds[0].transform.position.y);
            firstToLast(backgrounds, false);
            cameraStartPosition = Camera.main.transform.position;
        }
    }

    private void firstToLast(List<GameObject> currentList, bool positiveDirection = true)
    {
        if (positiveDirection)
        {
            currentList.Add(currentList[0]);
            currentList.RemoveAt(0);
        }else
        {
            GameObject currentObject = currentList[currentList.Count - 1];

            for(int i = currentList.Count - 2; i >= 0; i--)
            {
                currentList[i + 1] = currentList[i];
            }

            currentList[0] = currentObject;
        }
    }
}
