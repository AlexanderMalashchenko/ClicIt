using Leopotam.Ecs;
using UnityEngine;
using Voody.UniLeo;
using LeoEcsPhysics;
using Leopotam.Ecs.Ui.Systems;
namespace Client
{
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] EcsUiEmitter _uiEmitter = null;
        public Configuration Configuration;
        public SceneData SceneData;
        public ObjectPool Pool;

        EcsWorld _world;
        EcsSystems _updateSystems;
        EcsSystems _fixedUpdateSystems;
        GameState _gameState;



        void Start()
        {
            _world = new EcsWorld();
            EcsPhysicsEvents.ecsWorld = _world;

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);
            _gameState = new GameState();
            _updateSystems

                        .Add(new InitializationSystem())

                        .Add(new SpawnGoSystem())
                        .ConvertScene()
                        .Add(new SpawnEnSystem())
                        .Add(new RandomBallSystem())

                        .Add(new MoveBallSystem())
                        .Add(new SpeedIncreaseSystem())
                        .Add(new InputSystem())
                        .Add(new BoundSystem())
                        .Add(new UiProcessingSystem())
                        .Add(new ClearSystem())
                        .Add(new EndGameSystem())
                         .OneFrame<Damage>()
                         .OneFrame<EndGameEvent>()
                         .OneFrame<ClearEvent>();



            AddInjections(_updateSystems);
            _updateSystems.ProcessInjects();

            _updateSystems.Init();


            _gameState.State = State.Start;

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
#endif
        }

        void AddInjections(EcsSystems systems0)
        {
            systems0
            .Inject(Configuration)
            .Inject(SceneData)
            .Inject(_gameState)
            .Inject(Pool)
            .InjectUi(_uiEmitter);
        }
        void Update()
        {
            _updateSystems?.Run();
        }

        void OnDestroy()
        {
            EcsPhysicsEvents.ecsWorld = null;
            if (_updateSystems != null)
            {
                _updateSystems.Destroy();
                _updateSystems = null;
            }
            _world.Destroy();
            _world = null;
        }
    }
}