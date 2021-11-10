using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class InputSystem : IEcsRunSystem
    {
        private SceneData _sceneData = null;
        private GameState _gameState = null;

        public void Run()
        {
            if (Input.GetMouseButtonDown(0) && _gameState.State == State.Start)
            {
                _gameState.State = State.Game;
            }

            if (Input.GetMouseButtonDown(0) && _gameState.State == State.Game)
            {
                var mousePosition = _sceneData.Camera.ScreenToWorldPoint(Input.mousePosition);
                var origin = new Vector2(mousePosition.x, mousePosition.y);
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero);
                if (hit.collider != null)
                {
                    var go = hit.collider.gameObject;

                    var entityRef = go.GetComponent<EntityRef>();
                    var entity = entityRef.Entity;


                    var ballComponent = entity.Get<BallComponent>();
                    Color ballColor = ballComponent.Color;
                    Vector3 position = go.transform.position;

                    _sceneData.ExplosionInstantiate(position, ballColor);


                    entity.Get<ClearEvent>();
                   
                    _gameState.ScoreCount = ballComponent.Score + _gameState.ScoreCount;

                    
                }
            }

            if (Input.GetMouseButtonDown(0) && _gameState.State == State.End)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }


    }
}
