using UnityEngine;


public class EnemyManager
{
    [HideInInspector]
    public Transform spwanPoint;

    [HideInInspector]
    public Color enemyColor;

    [HideInInspector]
    public int playerNumber;


    [HideInInspector]
    public GameObject instance;

    private EnemyShoot enemyShoot;
    private EnemyController enemyController;


    public void Setup(Difficulty difficulty)
    {
        enemyShoot = instance.GetComponent<EnemyShoot>();
        enemyController = instance.GetComponent<EnemyController>();

        MeshRenderer[] renderers = instance.GetComponentsInChildren<MeshRenderer>();

        foreach (var renderer in renderers)
        {
            renderer.material.color = enemyColor;
        }
        SetDifficulty(difficulty);
    }

    public void DisableControl()
    {
        enemyController.enabled = false;
        enemyShoot.enabled = false;

    }
    public void EnableControl()
    {
        enemyController.enabled = true;
        enemyShoot.enabled = true;

    }

    void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                break;
            case Difficulty.Normal:
                SetNormalEnemies();
                break;
            case Difficulty.Hard:
                SetHardEnemies();
                break;
            case Difficulty.Professional:
                SetProfessionalEnemies();
                break;
            case Difficulty.God:
                SetGodEnemies();
                break;
            default:
                break;
        }
    }

    void SetNormalEnemies()
    {
        enemyController.SetSightRange(25f);
        enemyController.SetAttackRange(18f);
        enemyController.SetTimeBetweenAttacks(2f);

        enemyShoot.SetMaxLaunchForce(30f);
    }
    void SetHardEnemies()
    {
        enemyController.SetSightRange(35f);
        enemyController.SetAttackRange(25f);
        enemyController.SetTimeBetweenAttacks(1.25f);

        enemyShoot.SetMaxLaunchForce(50f);
    }
    void SetProfessionalEnemies()
    {
        enemyController.SetSightRange(50f);
        enemyController.SetAttackRange(35f);
        enemyController.SetTimeBetweenAttacks(0.5f);

        enemyShoot.SetMaxLaunchForce(65f);
    }

    void SetGodEnemies()
    {
        enemyController.SetSightRange(60f);
        enemyController.SetAttackRange(50f);
        enemyController.SetTimeBetweenAttacks(0.4f);
        enemyController.SetEnemySpeed(20f);

        enemyShoot.SetMaxLaunchForce(85f);
    }

}
