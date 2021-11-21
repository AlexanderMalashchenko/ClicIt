using Leopotam.Ecs;
using UnityEngine;
using System.Collections;

namespace Client
{
    public class SpawnEnSystem : IEcsRunSystem
    {
        private EcsFilter<BallComponent, NewBallFlag, OffFlag> _ball = null;

        public void Run()
        {
            foreach (var item in _ball)
            {
                var entity = _ball.GetEntity(item);
                var ballTransform = entity.Get<BallComponent>();
                if (ballTransform.Transform.gameObject.activeSelf)
                    entity.Del<OffFlag>();
            }
        }
    }
}