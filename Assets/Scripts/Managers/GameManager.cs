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
    public TankManager[] tanks;



    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;

    private TankManager roundWinner;
    private TankManager gameWinner;

    void Awake()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        cameraControl = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraControl>();
        SpawnAllTanks();

        SetCameraTargets();

        StartCoroutine(GameLoop());
    }



    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
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
        //DisableTankControl();
        //roundWinner = null;
        //roundWinner = GetRoundWinner();
        //if (roundWinner != null)
        //{
        //    roundWinner.wins++;
        //}

        //gameWinner = GetGameWinner();
        //string message = EndMessage();
        //messageText.text = message;
        yield return null;
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

        if (gameWinner != null)
        {
            message = gameWinner.coloredPlayerText + " WINS THE GAME!";
        }
        return message;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == numOfRoundsNeedToWin)
                return tanks[i];
        }
        return null;
    }

    private TankManager GetRoundWinner()
    {
        foreach (var tank in tanks)
        {
            if (tank.instance.activeSelf)
                return tank;
        }
        return null;
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
        int numOfTanksLeft = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                numOfTanksLeft++;
        }

        return numOfTanksLeft <= 1;
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
        ResetAllTanks();

        DisableTankControl();

        cameraControl.SetStartPositionAndSize();

        roundNumber++;
        messageText.text = "Round " + roundNumber;

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
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<Transform> targets = new List<Transform>();

        for (int i = 0; i < tanks.Length; i++)
        {
            targets.Add(tanks[i].instance.transform);
        }
        for (int i = 0; i < enemies.Length; i++)
        {
            targets.Add(enemies[i].transform);
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

}
