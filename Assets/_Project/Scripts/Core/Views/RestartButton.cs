using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Views
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

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
            SceneManager.LoadScene(0);
        }
    }
}