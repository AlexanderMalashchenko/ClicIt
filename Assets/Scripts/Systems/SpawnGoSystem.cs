using Leopotam.Ecs;
using UnityEngine;
using System.Collections;

namespace Client
{
    public class SpawnGoSystem : IEcsRunSystem
    {
        private GameState _gameState = null;
        private ObjectPool _pool = null;
        private SceneData _sceneData = null;
        private Configuration _configuration = null;
        float Timer = -1f;
        readonly EcsWorld _world = null;
        public void Run()
        {
            var startPoint = _configuration.StartPoint;
            if (_gameState.State == State.Game)
            {
                if (Timer > 0)
                {
                    Timer -= UnityEngine.Time.deltaTime;
                }
                else
                {
                    var ball = _pool.Get(_sceneData.BallPrefab);
                    ball.transform.position = startPoint;
                    ball.SetActive(true);
                    var EntityInit = _world.NewEntity();
                    ref var entityInitComponent = ref  EntityInit.Get<EntityInit>();
                    entityInitComponent.Ball = ball;
                    Timer = _configuration.SpawnTime;
                }
            }
        }
    }
}