using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Leopotam.Ecs.Ui.Components;

namespace Client
{
    sealed class UiProcessingSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private GameState _gameState = null;
        EcsWorld _world = null;
        [EcsUiNamed("Start")] Canvas _startCanvas;
        [EcsUiNamed("InGame")] Canvas _inGameCanvas;
        [EcsUiNamed("InGameCount")] TMP_Text _inGameCoinsCounter;
        [EcsUiNamed("Image")] Image _inGameCoinImage;
        [EcsUiNamed("HealthImage")] Image _healthBar;
        [EcsUiNamed("Health")] TMP_Text _health;
        [EcsUiNamed("InMenu")] Canvas _inMenuCanvas;
        [EcsUiNamed("EndGame")] Canvas _endGameCanvas;
        [EcsUiNamed("EndGameCount")] TMP_Text _endGameCountCounter;
        [EcsUiNamed("SpaceStartBlink")] CanvasGroup _spaceStartBlink;
        [EcsUiNamed("SpaceEndBlink")] CanvasGroup _spaceEndBlink;

        readonly EcsFilter<EcsUiClickEvent> _clickEvents = null;

        readonly EcsFilter<DamageComponent> _takeDamage = null;

        private Configuration _configuration = null;
        public Dictionary<State, Canvas> screens = new Dictionary<State, Canvas>();


        private float _playerHealth;
        public void Init()
        {
            _spaceStartBlink.DOFade(0, .5f).SetLoops(-1, LoopType.Yoyo);
            _spaceEndBlink.DOFade(0, .5f).SetLoops(-1, LoopType.Yoyo);

            screens = new Dictionary<State, Canvas>()
            {
                {State.Start, _startCanvas },
                {State.Game, _inGameCanvas },
                {State.Menu, _inMenuCanvas },
                {State.End, _endGameCanvas}
           };
            GameState.OnGameStateChange += OnGameStateChange;
            GameState.OnScoreChange += OnScoreChangeInGame;
            GameState.OnBestScoreChange += OnScoreChangeEndGame;
            _inGameCoinsCounter.text = $"Score: 0";

            _playerHealth = _configuration.PlayerHealth;

        }
        public void Run()
        {
            foreach (var idx in _clickEvents)
            {
                ref EcsUiClickEvent data = ref _clickEvents.Get1(idx);


                if (data.WidgetName == "Menu") { _gameState.State = State.Menu; UnityEngine.Time.timeScale = 0; }
                if (data.WidgetName == "Yes")
                {
                    EcsEntity EndGame = _world.NewEntity();
                    EndGame.Get<EndGameEvent>();
                }
                if (data.WidgetName == "No") { _gameState.State = State.Game; UnityEngine.Time.timeScale = 1; }
            }

            foreach (var idx in _takeDamage)
            {

                var DamageEntity = _takeDamage.GetEntity(idx);
                ref var damageValue = ref _takeDamage.Get1(idx);
                _playerHealth -= damageValue.DamageValue;
                _health.text = $"{_playerHealth}";
                _healthBar.fillAmount = Mathf.Clamp(_playerHealth / _configuration.PlayerHealth, 0, 1f);

                if (_playerHealth <= 0)
                {
                    var EndGame = _world.NewEntity();
                    EndGame.Get<EndGameEvent>();
                }

            }


        }

        public void OnGameStateChange(State state)
        {
            foreach (KeyValuePair<State, Canvas> screen in screens)
            {
                if (screen.Key != state)
                    screen.Value.enabled = false;
                else
                    screen.Value.enabled = true;
            }
        }
        private void OnScoreChangeInGame(int coins)
        {
            _inGameCoinsCounter.text = $"Score: {coins}";
            _inGameCoinImage.rectTransform.DOPunchScale(Vector3.one * .4f, .1f, 2);
            _inGameCoinImage.rectTransform.localScale = Vector3.ClampMagnitude(_inGameCoinImage.rectTransform.localScale, 1.5f);
        }

        private void OnScoreChangeEndGame(int score)
        {

            _endGameCountCounter.text = $"Best score: {score}";

        }

        public void Destroy()
        {
            GameState.OnGameStateChange -= OnGameStateChange;
            GameState.OnScoreChange -= OnScoreChangeInGame;
            GameState.OnBestScoreChange -= OnScoreChangeEndGame;
            DOTween.KillAll();
        }
    }

}

