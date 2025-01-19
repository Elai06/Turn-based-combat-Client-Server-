using UnityEngine;
using UnityEngine.UI;

namespace Core.Views
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private BattleSystem _battleSystem;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnRestart);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnRestart);
        }

        private void OnRestart()
        {
           _battleSystem.RequestRestart();
        }
    }
}