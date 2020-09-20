using System;
using UnityEngine;

[Serializable]
public class TankManager
{
    [SerializeField]
    private Color PlayerColor;

    [SerializeField]
    public Transform spwanPoint;

    [HideInInspector]
    public int playerNumber;

    [HideInInspector]
    public string coloredPlayerText;

    [HideInInspector]
    public GameObject instance;

    [HideInInspector]
    public int wins;

    private TankMovement movement;
    private TankShooting shooting;
    private TankHealth health;
    private GameObject canvasGameObject;

    public void Setup(bool godMod = false)
    {
        movement = instance.GetComponent<TankMovement>();
        shooting = instance.GetComponent<TankShooting>();
        health = instance.GetComponent<TankHealth>();

        SetGodMod(godMod);
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        movement.playerNumber = playerNumber;
        shooting.playerNumber = playerNumber;

        coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(PlayerColor) + ">PLAYER " + playerNumber + "</color>";

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.material.color = PlayerColor;
        }
    }

    private void SetGodMod(bool godMod)
    {
        health.SetGodMod(godMod);
    }

    public void DisableControl()
    {
        movement.enabled = false;
        shooting.enabled = false;

        canvasGameObject.SetActive(false);
    }
    public void EnableControl()
    {
        movement.enabled = true;
        shooting.enabled = true;

        canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = spwanPoint.position;
        instance.transform.rotation = spwanPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
