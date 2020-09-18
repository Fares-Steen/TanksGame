using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    private int numOfRoundsNeedToWin = 2;


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

    void Awake()
    {
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

        if (gameOver)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();

        SetGameOver();
        if (numOfRoundsNeedToWin == roundNumber)
        {
            levelWins = true;
            roundNumber = 0;
        }
        //if (roundWinner != null)
        //{
        //    roundWinner.wins++;
        //}

        //gameWinner = GetGameWinner();
        //string message = EndMessage();
        //messageText.text = message;
        yield return endWait;
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (roundWinner != null)
        {
            message = roundWinner.coloredPlayerText + " WINS THE ROUND!";
        }

        message += "\n\n\n\n";

        foreach (var tank in tanks)
        {
            message += tank.coloredPlayerText + ": " + tank.wins + " WINS\n";

        }

        //if (gameWinner != null)
        //{
        //    message = gameWinner.coloredPlayerText + " WINS THE GAME!";
        //}
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
    }

    private IEnumerator RoundStarting()
    {
        if (levelWins || levelNumber == 0)
        {
            levelNumber++;
        }

        ResetAllTanks();
        RemoveAllEnemies();

        DisableTankControl();
        SpawnAllEnemies();

        SetCameraTargets();
        cameraControl.SetStartPositionAndSize();

        roundNumber++;
        messageText.text = "Level " + levelNumber;
        messageText.text += "\n\n\n\n";
        messageText.text += "Round " + roundNumber;
        levelWins = false;
        yield return startWait;
    }

    private void DisableTankControl()
    {
        foreach (var tank in tanks)
        {
            tank.DisableControl();
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
            tanks[i].Setup();
        }
    }

    private void SpawnAllEnemies()
    {
        for (int i = 0; i < roundNumber + 1; i++)
        {
            var spawnPoint = GameObject.FindGameObjectWithTag("EnemySpawnPoint" + (i + 1)).transform;
            enemies.Add(new EnemyManager()
            {
                enemyColor = Color.yellow,
                instance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject
            });

        }

        foreach (var enemy in enemies)
        {
            enemy.Setup();
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

}
