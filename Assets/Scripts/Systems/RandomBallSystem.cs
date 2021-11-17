using Leopotam.Ecs;
using UnityEngine;
namespace Client
{
    sealed class RandomBallSystem : IEcsRunSystem
    {

        private EcsFilter<BallComponent, NewBallFlag>.Exclude<OffFlag> _ball = null;
        void IEcsRunSystem.Run()
        {
            foreach (var item in _ball)
            {
                ref var ball = ref _ball.Get1(item);
                var NewColor = new Color(Random.value, Random.value, Random.value, 1);
                ball.SpriteRenderer.color = NewColor;
                ball.Color = NewColor;
                var RandomValue = Random.Range(1.0f, 5.0f);
                ball.Speed = RandomValue;
                ball.Score = (int)RandomValue;
                ball.Damage = (int)RandomValue;
                ball.Transform.localScale = new Vector3 (RandomValue, RandomValue, 0);
                var Entity = _ball.GetEntity(item);
                Entity.Del<NewBallFlag>();
            }
        }
    }
}