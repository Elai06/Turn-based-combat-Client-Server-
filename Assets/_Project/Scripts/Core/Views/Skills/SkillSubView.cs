using System;
using Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.Skills
{
    public class SkillSubView : MonoBehaviour
    {
        public event Action<ESkill> OnClick;

        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private Button _button;

        private ESkill _eSkill;

        private int _cooldown;

        public void Init(ESkill skill, int cooldown)
        {
            _eSkill = skill;
            _nameText.text = _eSkill.ToString();

            UpdateCooldownText(cooldown);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Click);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Click);
        }

        private void UpdateCooldownText(int cooldown)
        {
            _button.interactable = cooldown <= 0;

            _cooldown = cooldown;
            _cooldownText.gameObject.SetActive(cooldown > 0);
            _cooldownText.text = $"{cooldown}";
        }

        private void Click()
        {
            OnClick?.Invoke(_eSkill);
        }
    }
}