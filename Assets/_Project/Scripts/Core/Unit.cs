using System;
using Core.Enums;
using Core.Skills;
using Core.Views;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Core
{
    public abstract class Unit : MonoBehaviour, IHealth
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Unit _damageTarget;

        public bool IsDied { get; private set; }

        private ReactiveProperty<EUnitState> UnitState { get; set; } = new();
        private readonly UnitSkillManager _unitUnitSkillManager = new();

        public ReactiveProperty<int> Health { get; private set; } = new();
        public readonly ReactiveCommand<ESkill> OnUsedSkill = new();
        public UnitSkillManager UnitUnitSkillsManager => _unitUnitSkillManager;

        private void Awake()
        {
            Health.Value = 100;

            _healthBar.Initialize(this);

            _unitUnitSkillManager.AddSkill(new DamageSkill(8, _damageTarget));
            _unitUnitSkillManager.AddSkill(new BarrierSkill(_unitUnitSkillManager, 2, 4));
            _unitUnitSkillManager.AddSkill(new RegenerationSkill(_unitUnitSkillManager, this, 2, 5));
            _unitUnitSkillManager.AddSkill(new FireballSkill(_damageTarget, _damageTarget.UnitUnitSkillsManager, 5, 6));
            _unitUnitSkillManager.AddSkill(new CleansingSkill(_unitUnitSkillManager, 1, 5));

            _unitUnitSkillManager.UsedSkill.Subscribe(UsedSkill).AddTo(gameObject);
        }

        public virtual void StartStep()
        {
            UnitState.Value = EUnitState.HisMove;
        }

        private void UsedSkill(ESkill skill)
        {
            OnUsedSkill.Execute(skill);
            UnitState.Value = EUnitState.Wait;
        }

        public void AddHealth(int health)
        {
            Health.Value += health;
        }

        public void RemoveHealth(int damage)
        {
            if (_unitUnitSkillManager.Effects.TryGetValue(EEffect.Barrier, out var effect))
            {
                damage -= effect.Value;
                if (damage <= 0) return;
            }

            Health.Value -= damage;

            if (Health.Value <= 0)
            {
                IsDied = true;
                UnitState.Value = EUnitState.Died;
            }
        }
    }
}