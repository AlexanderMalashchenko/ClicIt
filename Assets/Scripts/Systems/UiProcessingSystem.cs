using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Leopotam.Ecs.Ui.Components;
using LocalIdents;

namespace Client
{
    sealed class UiProcessingSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private GameState _gameState = null;
        EcsWorld _world = null;
        [EcsUiNamed(Ui.Start)] Canvas _startCanvas;
        [EcsUiNamed(Ui.InGame)] Canvas _inGameCanvas;
        [EcsUiNamed(Ui.InGameCount)] TMP_Text _inGameCoinsCounter;
        [EcsUiNamed(Ui.InGameImage)] Image _inGameCoinImage;
        [EcsUiNamed(Ui.HealthBar)] Image _healthBar;
        [EcsUiNamed(Ui.Health)] TMP_Text _health;
        [EcsUiNamed(Ui.InMenu)] Canvas _inMenuCanvas;
        [EcsUiNamed(Ui.EndGame)] Canvas _endGameCanvas;
        [EcsUiNamed(Ui.EndGameCount)] TMP_Text _endGameCountCounter;
        [EcsUiNamed(Ui.TapStartBlink)] CanvasGroup _spaceStartBlink;
        [EcsUiNamed(Ui.TapEndBlink)] CanvasGroup _spaceEndBlink;
        readonly EcsFilter<EcsUiClickEvent> _clickEvents = null;
        public Dictionary<State, Canvas> screens = new Dictionary<State, Canvas>();

        public void Init()
        {
            _spaceStartBlink.DOFade(0, .5f).SetLoops(-1, LoopType.Yoyo);
            _spaceEndBlink.DOFade(0, .5f).SetLoops(-1, LoopType.Yoyo);
            screens = new Dictionary<State, Canvas>()
            {
                {State.Start, _startCanvas},
                {State.Game, _inGameCanvas},
                {State.Menu, _inMenuCanvas},
                {State.End, _endGameCanvas}
            };
            GameState.OnGameStateChange += OnGameStateChange;
            GameState.OnScoreChange += OnScoreChangeInGame;
            GameState.OnBestScoreChange += OnScoreChangeEndGame;
            _inGameCoinsCounter.text = $"Score: 0";
        }

        public void Run()
        {
            foreach (var idx in _clickEvents)
            {
                ref EcsUiClickEvent data = ref _clickEvents.Get1(idx);

                switch (data.WidgetName)
                {
                    case Ui.ButtonMenu:
                        _gameState.State = State.Menu;
                        UnityEngine.Time.timeScale = 0;
                        break;
                    case Ui.ButtonYes:
                        EcsEntity EndGame = _world.NewEntity();
                        EndGame.Get<EndGameEvent>();
                        UnityEngine.Time.timeScale = 1;
                        break;
                    case Ui.ButtonNo:
                        _gameState.State = State.Game;
                        UnityEngine.Time.timeScale = 1;
                        break;
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
            _inGameCoinImage.rectTransform.localScale =
                Vector3.ClampMagnitude(_inGameCoinImage.rectTransform.localScale, 1.5f);
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