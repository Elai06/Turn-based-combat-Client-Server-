using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        private int _maxHealth;
        
        public void Initialize(IHealth health)
        {
            health.Health.Subscribe(OnHealthChanger).AddTo(gameObject);
            _maxHealth = health.Health.Value;
            OnHealthChanger(_maxHealth);
        }

        private void OnHealthChanger(int health)
        {
            _slider.value = (float)health / _maxHealth;
        }
    }
}