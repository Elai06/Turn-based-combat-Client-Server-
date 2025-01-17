using Core.Enums;
using TMPro;
using UnityEngine;

namespace Core.Views
{
    public class EffectSubView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _effectText;
        [SerializeField] private TextMeshProUGUI _stepText;

        public void Init(EEffect eEffect, int step)
        {
            _effectText.text = eEffect.ToString();
            _stepText.text = $"{step}";
        }
    }
}