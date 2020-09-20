using Assets.Enums;
using Assets.Scripts;
using Assets.Scripts.ScoorRepository;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    private int numOfRoundsNeedToWinTheLevel = 2;
    private int numOfLevelvsNeedToWinTheGame = 5;


    private float startDelay = 3f;


    private float endDelay = 3f;


    private CameraControl cameraControl;

    [SerializeField]
    private Text messageText;

    [SerializeField]
    private GameObject tankPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    public TankManager[] tanks;


    private List<EnemyManager> enemies;


    private int roundNumber;
    private int levelNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;

    private TankManager roundWinner;
    private bool gameOver;
    private bool levelWins;
    private bool finishedTheGame;
    private ScoreModel score;
    private bool godMod;

    void Awake()
    {
        score = new ScoreModel()
        {
            Name = UnitySingleton.Instance.playerName,
            Level = 1,
            Round = 1
        };
        godMod = UnitySingleton.Instance.godMod;
        enemies = new List<EnemyManager>();
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        cameraControl = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraControl>();
        SpawnAllTanks();



        StartCoroutine(GameLoop());
    }



    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameOver || finishedTheGame)
        {
            SaveScore();
            SceneManager.LoadScene((int)EScens.MainMenu);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private void SaveScore()
    {
        if (!string.IsNullOrEmpty(score.Name))
        {
            score.Round = roundNumber;
            score.Level = levelNumber;
            ScoreRepositoryAction scoreRepositoryAction = new ScoreRepositoryAction();
            scoreRepositoryAction.SaveNewScoor(score);
        }

    }

    private IEnumerator RoundEnding()
    {
        DisableTanksControl();

        SetGameOver();
        SetFinishedTheGame();
        if (numOfRoundsNeedToWinTheLevel == roundNumber && !gameOver)
        {
            levelWins = true;
            roundNumber = 0;
        }
        if (finishedTheGame)
        {
            string message = EndMessage(godMod);

            messageText.text = message;
            endWait = new WaitForSeconds(15f);

        }


        yield return endWait;
    }



    private string EndMessage(bool godMod)
    {
        string message = "Ohhhhhhh";

        message += "\n\n\n\n";

        if (godMod)
        {
            message += "<color=#2A64B2>The End of the game!!</color>";

        }
        else
        {
            message += "<color=#2A64B2>You have finished the Game!!</color>";

        }

        return message;
    }

    private void SetGameOver()
    {
        int numOfTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numOfTanksLeft++;
        }

        gameOver = numOfTanksLeft < 1;
    }

    private void SetFinishedTheGame()
    {
        if (numOfLevelvsNeedToWinTheGame == levelNumber && numOfRoundsNeedToWinTheLevel == roundNumber)
        {
            int numOfTanksLeft = 0;

            for (int i = 0; i < tanks.Length; i++)
            {
                if (tanks[i].instance.activeSelf)
                    numOfTanksLeft++;
            }

            finishedTheGame = numOfTanksLeft > 0;
        }
    }

    private IEnumerator RoundPlaying()
    {
        EnableTanksControl();
        messageText.text = string.Empty;

        while (OneTankLeft())
        {
            yield return null;

        }
    }

    private bool OneTankLeft()
    {
        int numOfEnemiesLeft = 0;
        int numOfTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numOfTanksLeft++;
        }

        foreach (var enemy in enemies)
        {
            if (enemy.instance.activeSelf)
                numOfEnemiesLeft++;
        }

        return numOfEnemiesLeft >= 1 && numOfTanksLeft >= 1;
    }

    private void EnableTanksControl()
    {
        foreach (var tank in tanks)
        {
            tank.EnableControl();
        }

        foreach (var enemy in enemies)
        {
            enemy.EnableControl();
        }
    }

    private IEnumerator RoundStarting()
    {
        if (levelWins || levelNumber == 0)
        {
            levelNumber++;
        }

        ResetAllTanks();
        RemoveAllEnemies();

        SpawnAllEnemies();
        DisableTanksControl();

        SetCameraTargets();
        cameraControl.SetStartPositionAndSize();

        roundNumber++;
        messageText.text = "Level " + levelNumber;
        messageText.text += "\n\n\n\n";
        messageText.text += "Round " + roundNumber;
        levelWins = false;
        yield return startWait;
    }

    private void DisableTanksControl()
    {
        foreach (var tank in tanks)
        {
            tank.DisableControl();
        }

        foreach (var enemy in enemies)
        {
            enemy.DisableControl();
        }
    }

    private void ResetAllTanks()
    {
        foreach (var tank in tanks)
        {
            tank.Reset();
        }


    }

    private void SetCameraTargets()
    {


        List<Transform> targets = new List<Transform>();

        for (int i = 0; i < tanks.Length; i++)
        {
            targets.Add(tanks[i].instance.transform);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            targets.Add(enemies[i].instance.transform);
        }

        cameraControl.targetTanks = targets.ToArray();
    }

    private void SpawnAllTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance = Instantiate(tankPrefab, tanks[i].spwanPoint.position, tanks[i].spwanPoint.rotation) as GameObject;
            tanks[i].playerNumber = i + 1;
            tanks[i].Setup(godMod);
        }
    }

    private void SpawnAllEnemies()
    {
        var numberofEnemies = roundNumber == 0 ? 2 : 4;
        for (int i = 0; i < numberofEnemies; i++)
        {
            var spawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint" + (i + 1)).transform;
            enemies.Add(new EnemyManager()
            {
                enemyColor = GetEnemiesColor(),
                instance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject
            });

        }

        foreach (var enemy in enemies)
        {
            enemy.Setup(GetEnemiesDifficlty());
        }
    }

    private void RemoveAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy.instance);
        }

        enemies = new List<EnemyManager>();
    }

    private Difficulty GetEnemiesDifficlty()
    {
        switch (levelNumber)
        {
            case 1:
                return Difficulty.Easy;
            case 2:
                return Difficulty.Normal;
            case 3:
                return Difficulty.Hard;
            case 4:
                return Difficulty.Professional;
            case 5:
                return Difficulty.God;
            default:
                return Difficulty.Easy;

        }
    }

    private Color GetEnemiesColor()
    {
        switch (levelNumber)
        {
            case 1:
                return Color.yellow;
            case 2:
                return Color.white;
            case 3:
                return Color.green;
            case 4:
                return Color.red;
            case 5:
                return Color.black;
            default:
                return Color.yellow;

        }
    }

}
