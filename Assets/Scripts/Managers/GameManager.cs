using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int numOfRoundsNeedToWin = 2;

    [SerializeField]
    private float startDelay = 3f;

    [SerializeField]
    private float endDelay = 3f;

    [SerializeField]
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

    void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

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
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundEnding()
    {
        DisableTankControl();
        roundWinner = null;
        roundWinner = GetRoundWinner();
        if (roundWinner != null)
        {
            roundWinner.wins++;
        }

        gameWinner = GetGameWinner();
        string message = EndMessage();
        messageText.text = message;
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

        while (!OneTankLeft())
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
        Transform[] targets = new Transform[tanks.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = tanks[i].instance.transform;
        }
        cameraControl.targetTanks = targets;
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
