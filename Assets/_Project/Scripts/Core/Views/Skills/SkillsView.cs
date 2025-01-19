using System.Collections.Generic;
using Core.Enums;
using Core.Skills;
using UniRx;
using UnityEngine;

namespace Core.Views.Skills
{
    public class SkillsView : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private BattleSystem _battleSystem;
        [SerializeField] private GameObject _skillsContainer;
        [SerializeField] private SkillSubView _skillPrefab;

        private Dictionary<ESkill, SkillSubView> _skillSubViews = new();

        private UnitSkillManager _unitSkillManager;

        private void Start()
        {
            _unitSkillManager = _player.UnitSkillManager;
            _battleSystem.FinishedRound.Subscribe(_ => OnFinishedRound()).AddTo(gameObject);
            _battleSystem.StartStep.Subscribe(OnFinishedStep).AddTo(gameObject);
            _battleSystem.RestartedGame.Subscribe(_ => OnRestartedGame()).AddTo(gameObject);

            CreateSubViews();
        }

        private void OnFinishedStep(EUnit chooseUnit)
        {
            _skillsContainer.SetActive(chooseUnit == EUnit.Player);
        }

        private void OnFinishedRound()
        {
            UpdateSkillSubViews();
        }

        private void UpdateSkillSubViews()
        {
            foreach (var skill in _unitSkillManager.Skills.Values)
            {
                if (_skillSubViews.TryGetValue(skill.Type, out var view))
                {
                    view.Init(skill.Type, skill.Step);
                }
            }
        }

        private void CreateSubViews()
        {
            foreach (var skill in _unitSkillManager.Skills)
            {
                var skillSubView = Instantiate(_skillPrefab, _skillsContainer.transform);
                skillSubView.Init(skill.Key, skill.Value.Step);
                skillSubView.OnClick += OnSelectedSkill;
                _skillSubViews.Add(skill.Key, skillSubView);
            }
        }

        private void OnDestroy()
        {
            foreach (var skillSubView in _skillSubViews)
            {
                skillSubView.Value.OnClick -= OnSelectedSkill;
            }
        }

        private void OnSelectedSkill(ESkill skill)
        {
            _unitSkillManager.ActivateSkill(skill);
        }
        
        private void OnRestartedGame()
        {
            UpdateSkillSubViews();
        }
    }
}