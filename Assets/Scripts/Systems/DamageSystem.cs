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
    sealed class DamageSystem : IEcsRunSystem
    {
        EcsWorld _world = null;
        [EcsUiNamed("HealthImage")] Image _healthBar;
        [EcsUiNamed("Health")] TMP_Text _health;
        readonly EcsFilter<DamageComponent> _takeDamage = null;
        private SceneData _sceneData = null;
        private Configuration _configuration = null;
        
        public void Run()
        {
            ref var _playerHealth = ref _sceneData.CurrentHealth;
            foreach (var idx in _takeDamage)
            {
                var DamageEntity = _takeDamage.GetEntity(idx);
                ref var damageValue = ref _takeDamage.Get1(idx);
                _playerHealth -= damageValue.DamageValue;
                _health.text = $"{_playerHealth}";
                _healthBar.fillAmount = Mathf.Clamp(_playerHealth / _configuration.PlayerMaxHealth, 0, 1f);
                if (_playerHealth <= 0)
                {
                    var EndGame = _world.NewEntity();
                    EndGame.Get<EndGameEvent>();
                }
            }
        }
    }
}
