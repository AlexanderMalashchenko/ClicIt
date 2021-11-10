using Leopotam.Ecs;
using UnityEngine;
using System.Collections;

namespace Client
{
    public class SpawnEnSystem : IEcsRunSystem
    {

        private EcsFilter<EntityInit> _ballInit = null;

        public void Run()
        {
            foreach (var item in _ballInit)
            {
                ref var ball = ref _ballInit.Get1(item);
                var entityRefComponent = ball.Ball.GetComponent<EntityRef>();
                var entity = entityRefComponent.Entity;
                entity.Del<OffFlag>();
                ref var entityBallInit = ref _ballInit.GetEntity(item);
                entityBallInit.Del<EntityInit>();
            }
        }
    }
}