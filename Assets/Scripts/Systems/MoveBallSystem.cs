using Leopotam.Ecs;
using UnityEngine;

namespace Client
{
    sealed class MoveBallSystem : IEcsRunSystem
    {

        private EcsFilter<BallComponent>.Exclude<NewBallFlag, OffFlag> _ball = null;
        private Configuration _config = null;
        private GameState _gameState = null;

        void IEcsRunSystem.Run()

        {
            if (_gameState.State == State.Game)
            {
                foreach (var item in _ball)
                {
                    ref var ball = ref _ball.Get1(item);
                    ref var ballTransform = ref ball.Transform;
                    ballTransform.position += Vector3.down * UnityEngine.Time.deltaTime * ball.Speed * _config.BallSpeedModifier;
                }

            }
        }
    }
}