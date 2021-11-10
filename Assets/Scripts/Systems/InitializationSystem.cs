using Leopotam.Ecs;

namespace Client
{
    sealed class InitializationSystem : IEcsInitSystem
    {
        readonly EcsWorld _world = null;
        private ObjectPool _pool = null;
        private SceneData _sceneData = null;
        private Configuration _configuration = null;

        public void Init()
        {
            _pool.AddObjects(_configuration.ValuePool, _sceneData.BallPrefab);

            var TimerEntity = _world.NewEntity();
            var timer =TimerEntity.Get<Timer>();
            _configuration.BallSpeedModifier = 1f;
            timer.TimeIncrease = _configuration.TimeIncrease;
        }
    }
}