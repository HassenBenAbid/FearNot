using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private const KeyCode PASS_SENTENCE_KEY = KeyCode.Space;

    [SerializeField] public List<Section> sections;
    [SerializeField] private TextMeshProUGUI nameField, sentenceField;

    private int sectionIndex = 0;
    private Section currentSection;
    private bool dialogueStarted = false;

    private void Start()
    {
        //gameObject.SetActive(false);
    }

    public void begin()
    {
        gameObject.SetActive(true);
        dialogueStarted = true;
        displayNextSentence();
    }

    private void end()
    {
        dialogueStarted = false;
        LevelManager.Instance.DialogueFinished();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (dialogueStarted && Input.GetKeyDown(PASS_SENTENCE_KEY))
        {
            displayNextSentence();
        }
    }

    public void displayNextSentence()
    {
        if (sectionIndex >= sections.Count)
        {
            end();
            return;
        }

        currentSection = sections[sectionIndex];

        nameField.text = currentSection.name;
        sentenceField.text = currentSection.sentence;
        sectionIndex++;
        StopAllCoroutines();
        StartCoroutine(typeSentence(currentSection.sentence));
    }

    IEnumerator typeSentence(string sentence)
    {
        sentenceField.text = "";
        foreach (char letter in sentence)
        {
            sentenceField.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
