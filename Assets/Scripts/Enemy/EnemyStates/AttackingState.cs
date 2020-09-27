namespace Assets.Scripts.Enemy.EnemyStates
{
    public class AttackingState : IAiState
    {
        EnemyController _enemyController;

        public IAiState DoState(EnemyController enemyController)
        {
            _enemyController = enemyController;
            if (_enemyController.playerInAttachRange && !_enemyController.LowInhealth)
            {

                AttackPlayer();
                return _enemyController.attackingState;
            }
            return _enemyController.fleeingState;
        }

        private void AttackPlayer()
        {
            _enemyController.agent.SetDestination(_enemyController.transform.position);
            _enemyController.transform.LookAt(_enemyController.players);

            if (!_enemyController.alreadyAttacked)
            {
                _enemyController.GetComponent<EnemyShoot>().Fire(_enemyController.players.position);

                _enemyController.alreadyAttacked = true;
                _enemyController.Invoke(nameof(ResetAttack), _enemyController.timeBetweenAttacks);
            }


        }


        private void ResetAttack()
        {

            _enemyController.alreadyAttacked = false;
        }
    }
}
