namespace Assets.Scripts.Enemy.EnemyStates
{
    public class ChasingState : IAiState
    {
        EnemyController _enemyController;

        public IAiState DoState(EnemyController enemyController)
        {
            _enemyController = enemyController;
            if (_enemyController.playerInSightRange && !_enemyController.playerInAttachRange && !_enemyController.LowInhealth)
            {

                ChasePlayer();
                return _enemyController.chasingState;
            }
            return _enemyController.attackingState;
        }

        private void ChasePlayer()
        {
            _enemyController.agent.SetDestination(_enemyController.players.position);
        }


    }
}
