using Leopotam.Ecs;
using UnityEngine;
namespace Client
{
    internal class SpeedIncreaseSystem : IEcsRunSystem
    {

        private EcsFilter<TimerComponent> _timer = null;
        private Configuration _config = null;
        public void Run()
        {
            foreach (var item in _timer)
            {
                ref var timer = ref _timer.Get1(item);
                if (timer.TimeIncrease > 0)
                {
                    timer.TimeIncrease -= Time.deltaTime;
                }
                else
                {
                    _config.BallSpeedModifier = _config.BallSpeedModifier + 0.2f;
                    timer.TimeIncrease = _config.TimeIncrease;
                }
            }
        }
    }
}