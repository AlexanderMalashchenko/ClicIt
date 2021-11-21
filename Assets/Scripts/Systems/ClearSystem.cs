using Leopotam.Ecs;

namespace Client
{
    public class ClearSystem : IEcsRunSystem
    {
        private EcsFilter<BallComponent, ClearEvent> _ball = null;
        private ObjectPool _pool = null;
        private Configuration _configuration = null;

        public void Run()
        {
            foreach (var item in _ball)
            {
                ref var ball = ref _ball.Get1(item);
                var Entity = _ball.GetEntity(item);
                ball.Transform.position = _configuration.StartPoint;
                _pool.ReturnToPool(ball.Transform.gameObject);
                Entity.Get<NewBallFlag>();
                Entity.Get<OffFlag>();
            }
        }
    }
}