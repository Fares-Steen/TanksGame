using UnityEngine;

namespace Assets.Scripts.Enemy.EnemyStates
{
    public class PatrolingState : IAiState
    {
        EnemyController _enemyController;
        public IAiState DoState(EnemyController enemyController)
        {
            _enemyController = enemyController;

            if (!_enemyController.playerInAttachRange && !_enemyController.playerInSightRange)
            {
                Patroling();
                return _enemyController.patrolingState;
            }
            return _enemyController.chasingState;
        }

        private void Patroling()
        {
            if (!_enemyController.walkPointsSet)
            {
                SearchWalkPoint();
            }
            else
            {
                _enemyController.agent.SetDestination(_enemyController.walkPoint);
            }

            Vector3 distanceToWalkPoint = _enemyController.transform.position - _enemyController.walkPoint;

            //WalkPoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                _enemyController.walkPointsSet = false;
        }

        private void SearchWalkPoint()
        {
            //Calculate random point in range
            float randomZ = Random.Range(-_enemyController.walkPointRange, _enemyController.walkPointRange);
            float randomX = Random.Range(-_enemyController.walkPointRange, _enemyController.walkPointRange);

            _enemyController.walkPoint = new Vector3(_enemyController.firstPosition.x + randomX, _enemyController.firstPosition.y, _enemyController.firstPosition.z + randomZ);

            if (Physics.Raycast(_enemyController.walkPoint, -_enemyController.transform.up, 2f, _enemyController.whatIsGround))
            {
                _enemyController.walkPointsSet = true;
            }
        }
    }
}
