using ByNorth.LifeCycle;
using ByNorth.Unit.Behaviour.Movable;
using ByNorth.Unit.Behaviour.Movable.State.NonPlayer;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace ByNorth.Unit.DieEvent {
    public class SpawnUnitOnDie : MonoBehaviour, IRelease<Unit> {
        [Header("첫번째 NPC가 스폰될때까지 걸리는 시간")]
        public float firstSpawnTime = 0.5f;

        [Header("첫번째 NPC이후 스폰되는 딜레이 시간")]
        public float spawnDelay = 0.3f;

        public List<UnitData> CreateUnits { get; set; } = new();


        public void OnRelease(Unit unit)
        {
            _ = SpawnNpcUnit();
        }
        private async UniTask SpawnNpcUnit()
        {
            await UniTask.WaitForSeconds(firstSpawnTime);

            foreach (var unitData in CreateUnits)
            {
                Unit npc = Unit.Spawn(unitData);
                await UniTask.Yield();
                npc.transform.position = transform.transform.position;
                npc.GetComponent<NonPlayerHandler>().ChangeState(StateType.Runaway);
                await UniTask.WaitForSeconds(spawnDelay);
            }
        }
    }
}
