using Leopotam.Ecs;
using UnityEngine;

namespace Client
{

    public class BoundSystem : IEcsRunSystem
    {
        EcsWorld _world = null;
        private EcsFilter<BallComponent>.Exclude<NewBallFlag, OffFlag> _ball = null;
        private SceneData _sceneData = null;
        public void Run()
        {
            var camera = _sceneData.Camera;

            foreach (var item in _ball)
            {
                ref var ball = ref _ball.Get1(item);
                var Entity = _ball.GetEntity(item);
                var DamageValue = ball.Damage;
                var point = Vector2.zero;

                point = camera.WorldToViewportPoint(ball.Transform.position);

                if (point.y < 0f)
                {
                    EcsEntity TakeDamage = _world.NewEntity();
                    ref var takeDamage = ref TakeDamage.Get<Damage>();
                    takeDamage.DamageValue = DamageValue;

                    Entity.Get<ClearEvent>();
                }
            }

        }

    }
}