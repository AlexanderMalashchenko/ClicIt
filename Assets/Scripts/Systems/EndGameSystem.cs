using Leopotam.Ecs;
using UnityEngine;
namespace Client
{
    public class EndGameSystem : IEcsRunSystem
    {

        private GameState _gameState = null;
        private EcsFilter<EndGameEvent> _filter = null;
        public void Run()
        {
            if (!_filter.IsEmpty())
            {
                int BestScore = 0;
                if (PlayerPrefs.HasKey("ScoreCount"))
                {
                    BestScore = PlayerPrefs.GetInt("ScoreCount");
                    _gameState.BestScoreCount = BestScore;
                }

                if (BestScore < _gameState.ScoreCount)
                {
                    PlayerPrefs.SetInt("ScoreCount", _gameState.ScoreCount);
                    PlayerPrefs.Save();
                    _gameState.BestScoreCount = _gameState.ScoreCount;
                }
                _gameState.State = State.End;
            }
        }
    }
}