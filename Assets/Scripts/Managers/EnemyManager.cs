using System;
using UnityEngine;



[Serializable]
public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    public Transform spwanPoint;

    [HideInInspector]
    public Color PlayerColor;

    [HideInInspector]
    public int playerNumber;

    [HideInInspector]
    public string coloredPlayerText;

    [HideInInspector]
    public GameObject instance;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
