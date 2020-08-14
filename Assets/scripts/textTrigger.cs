using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textTrigger : MonoBehaviour
{
    private const float TIME_BTW_SENTENCES = 3.0f;

    [SerializeField] private List<string> sentences;

    private TextMeshPro text;
    private bool triggered = false;

    private void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.enabled = false;
    }

    private void startText()
    {
        text.enabled = true;
        StartCoroutine(printText());
    }

    private IEnumerator printText()
    {
        foreach (string sentence in sentences)
        {
            text.text = sentence;
            yield return new WaitForSeconds(TIME_BTW_SENTENCES);
        }
        text.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !triggered)
        {
            startText();
            triggered = true;
        }
    }

}
