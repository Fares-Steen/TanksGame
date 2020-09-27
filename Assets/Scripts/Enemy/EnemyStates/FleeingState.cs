
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Enemy.EnemyStates
{
    public class FleeingState : IAiState
    {
        EnemyController _enemyController;
        public IAiState DoState(EnemyController enemyController)
        {
            _enemyController = enemyController;

            if (_enemyController.agent)
            {
                _enemyController.agent = _enemyController.GetComponent<NavMeshAgent>();
            }
            if (_enemyController.playerInSightRange && _enemyController.LowInhealth)
            {

                RunAwayFromPlayer();
                return _enemyController.fleeingState;
            }
            return _enemyController.patrolingState;
        }

        private void RunAwayFromPlayer()
        {
            if (!_enemyController.walkPointsAwayFromPlayerSet && _enemyController.playerInAttachRange)
            {
                SearchWalkPointAwayFromPLayer();
            }
            else
            {
                _enemyController.agent.SetDestination(_enemyController.walkPoint);
            }

            Vector3 distanceToWalkPoint = _enemyController.transform.position - _enemyController.walkPoint;

            //WalkPoint reached
            if (distanceToWalkPoint.magnitude < 1f)
                _enemyController.walkPointsAwayFromPlayerSet = false;

        }

        private void SearchWalkPointAwayFromPLayer()
        {
            //Calculate random point in range
            float rangez = _enemyController.players.position.z - _enemyController.transform.position.z > 0 ? _enemyController.transform.position.z + 5 : _enemyController.transform.position.z - 5;
            float rangex = _enemyController.players.position.x - _enemyController.transform.position.x > 0 ? _enemyController.transform.position.x + 5 : _enemyController.transform.position.x - 5;
            float randomZ = Random.Range(-_enemyController.players.position.z, _enemyController.players.position.z);
            float randomX = Random.Range(-_enemyController.players.position.x + 5, _enemyController.players.position.x + 5);


            _enemyController.walkPoint = new Vector3(_enemyController.firstPosition.x + rangex, _enemyController.firstPosition.y, _enemyController.firstPosition.z + rangez);



            if (Physics.Raycast(_enemyController.walkPoint, -_enemyController.transform.up, 2f, _enemyController.whatIsGround))
            {
                _enemyController.walkPointsAwayFromPlayerSet = true;
            }
        }
    }
}
