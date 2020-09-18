using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [HideInInspector]
    public Transform spwanPoint;

    [HideInInspector]
    public Color enemyColor;

    [HideInInspector]
    public int playerNumber;


    [HideInInspector]
    public GameObject instance;


    public void Setup()
    {

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.material.color = enemyColor;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
