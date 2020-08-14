using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelManager : Singleton<LevelManager>
{
    private const int ORTHOGRAPHIC_CAMERA_SIZE = 8;
    private const int ORTHOGRAPHIC_CAMERA_SIZE_DIALOGUE = 4;
    private const float TIME_BTW_SCENE = 1.0f;
    private const float VOICE_LINE_TIME = 2.5f;
    private const float TIME_BTW_LINES = 15.0f;
    private const float SPECIAL_TIME = 25.0f;

    [SerializeField] private Dialogue startDialogue, endDialogue, endSceneDialogue;
    [SerializeField] private GameObject gameoverScreen, marketScreen;
    [SerializeField] private Player player;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Transform playerEndScenePos, endScene;
    [SerializeField] private TextMeshProUGUI timerUI, moneyUI;
    [SerializeField] private float startingTime = 150.0f;
    [SerializeField] private List<string> voiceLines;
    [SerializeField] private TextMeshPro voicesUI;
    [SerializeField] private LevelGenerator generator;

    private bool playing = true, triggred = false;
    private Vector2 originalGravity;
    private bool levelEnded = false;
    private static bool intro ;
    private float gameTimer, endTimer, bossTimer;
    private MainCamera camera;
    private int currentLine;
    private bool timerStopped;

    private int currentMoney;

    private void Start()
    {
        camera = Camera.main.GetComponent<MainCamera>();
        currentLine = 0;

        gameTimer = 0;
        endTimer = startingTime;

        gameoverScreen.SetActive(false);
        startDialogue.gameObject.SetActive(false);
        
        if (!intro)
        {
            startGame();
        }

        marketScreen.SetActive(false);
        timerStopped = false;
    }

    private void Update()
    {
        if (levelEnded && playing)
        {
            simpleGameOver();
            globalLight.intensity = 0;
        }

        if (playing)
        {
            TimersUpdate();
            updateBossTimer();
            nextLine();
            if (player.isPlayerDead())
            {
                playing = false;
            }
        }
    }

    private void TimersUpdate()
    {
            endTimer -= Time.deltaTime;
            if (endTimer < 0)
            {
                endTimer = 0;
                gameOver();
            }
            timerUI.text = ((int)endTimer).ToString();

            gameTimer += Time.deltaTime;
    }

    private void updateBossTimer()
    {
        if (!timerStopped)
        {
            bossTimer += Time.deltaTime;
        }
    }

    public void stopBossTimer()
    {
        timerStopped = true;
    }

    public void restartBossTimer()
    {
        bossTimer = 0;
        timerStopped = false;
        LevelGenerator.bossSpawned = false;
    }

    public float currentBossTimer()
    {
        return bossTimer;
    }

    public void addToTimer(float seconds)
    {
        endTimer += seconds;
        timerUI.text = ((int)endTimer).ToString();
    }

    public float currentGameTimer()
    {
        return gameTimer;
    }

    private void startaDialogue(Dialogue aDialogue)
    {
        playing = false;
        camera.stopFollow();
        Camera.main.orthographicSize = ORTHOGRAPHIC_CAMERA_SIZE_DIALOGUE;
        aDialogue.begin();
        player.stop();
    }

    private void startGame()
    {
        startaDialogue(startDialogue);
        intro = true;
    }

    public void DialogueFinished()
    {
        Camera.main.orthographicSize = ORTHOGRAPHIC_CAMERA_SIZE;
        camera.startFollow();
        playing = true;
    }

    public bool canPlay()
    {
        return playing;
    }

    public void gameOver()
    {
        if (!levelEnded)
        {
            if (generator.getCurrentBoss() > 0)
            {
                gameOverPlus();
            }
            else
            {
                simpleGameOver();
            }

            levelEnded = true;
            playing = false;
            SaveSystem.save();
        }
    }

    private void simpleGameOver()
    {
        saveData();

        gameoverScreen.SetActive(true);
        gameoverScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "TIME SURVIVED : " + ((int)gameTimer).ToString() + "s";
        gameoverScreen.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = UserUnlockable.getMoney().ToString();
        camera.stopFollow();
    }

    private void gameOverPlus()
    {
        StartCoroutine(lastScene());
        playing = false;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void nextLine()
    {

        if (currentLine < voiceLines.Count)
        {
            if (gameTimer >= (currentLine) * TIME_BTW_LINES)
            {
                StartCoroutine(lineTimer());
            }
        }
    }

    public void addMoney(int value)
    {
        currentMoney += value;
        moneyUI.text = currentMoney.ToString();
    }

    private void saveData()
    {
        UserUnlockable.addMoney(currentMoney);
    }

    public void openMarket()
    {
        marketScreen.SetActive(true);
    }

    public void moreTime()
    {
        startingTime += SPECIAL_TIME;
    }

    public void exitGame()
    {
        Application.Quit();
    }

    private IEnumerator lineTimer()
    {
        voicesUI.gameObject.SetActive(true);
        voicesUI.text = voiceLines[currentLine];
        currentLine++;

        yield return new WaitForSeconds(VOICE_LINE_TIME);

        voicesUI.gameObject.SetActive(false);

    }

    //
    //private void endGame()
    //{
    //    if (lastSceneEnded && playing)
    //    {
    //        globalLight.SetActive(false);
    //        lastSceneEnded = false;
    //        playing = false;
    //    }
    //}

    private IEnumerator lastScene()
    {
        camera.stopFollow();
        camera.gameObject.transform.position = new Vector3(endScene.position.x, endScene.position.y, camera.gameObject.transform.position.z);
        globalLight.intensity = 0.8f;
        yield return new WaitForSeconds(TIME_BTW_SCENE);
        endSceneDialogue.begin();
    }
}
