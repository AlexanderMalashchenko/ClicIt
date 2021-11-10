using System;
using UnityEngine;

namespace Client
{
    public class GameState
    {
        public static Action<State> OnGameStateChange;
        public static Action<int> OnScoreChange;
        public static Action<int> OnBestScoreChange;

        public GameState()
        {
            ScoreCount = 0;

        }



        private State _state;
        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnGameStateChange?.Invoke(_state);
            }
        }

        private int _scoreCount;
        public int ScoreCount
        {
            get { return _scoreCount; }
            set
            {
                _scoreCount = value;
                OnScoreChange?.Invoke(_scoreCount);
            }
        }

         private int _bestScoreCount;
        public int BestScoreCount
        {
            get { return _bestScoreCount; }
            set
            {
                _bestScoreCount = value;
                OnBestScoreChange?.Invoke(_bestScoreCount);
            }
        }



    }


    public enum State
    {
        Start,
        End,
        Menu,
        Game
    }

}
