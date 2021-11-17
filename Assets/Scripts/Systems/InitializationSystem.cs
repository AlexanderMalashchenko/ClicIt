using Leopotam.Ecs;

namespace Client
{
    sealed class InitializationSystem : IEcsInitSystem
    {
        readonly EcsWorld _world = null;
        private Configuration _configuration = null;
        public void Init()
        {
            var TimerEntity = _world.NewEntity();
            var timer = TimerEntity.Get<TimerComponent>();
            _configuration.BallSpeedModifier = 1f;
            timer.TimeIncrease = _configuration.TimeIncrease;
        }
    }
}