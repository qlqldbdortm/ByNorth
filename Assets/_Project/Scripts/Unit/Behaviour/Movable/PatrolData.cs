using System;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.Unit.Behaviour.Movable
{
    [Serializable]
    public class PatrolData
    {
        [SerializeField] public List<Transform> patrolPoints;
        
        
        public bool HasPatrolDestination => patrolPoints.Count > 0;


        public Transform NextDestination()
        {
            Transform result = patrolPoints[0];
            patrolPoints.RemoveAt(0);
            patrolPoints.Add(result);

            return result;
        }
    }
}