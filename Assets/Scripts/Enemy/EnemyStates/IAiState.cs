namespace Assets.Scripts.Enemy.EnemyStates
{
    public interface IAiState
    {
        IAiState DoState(EnemyController enemyController);
    }
}
