using System;
using Core.Enums;
using Core.Skills;
using Core.Skills.Effects;
using Core.Views;
using Network;
using UniRx;
using UnityEngine;

namespace Core
{
    public abstract class Unit : MonoBehaviour, IHealth
    {
        [SerializeField] private EUnit _eUnit;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private Unit _damageTarget;

        private IGameAdapter _gameAdapter;

        public bool IsDied { get; private set; }

        private ReactiveProperty<EUnitState> UnitState { get; set; } = new();
        public UnitSkillManager UnitSkillManager { get; private set; }

        public ReactiveProperty<int> Health { get; private set; } = new();
        public readonly ReactiveCommand<ESkill> OnUsedSkill = new();

        private void Awake()
        {
            UnitSkillManager = new UnitSkillManager();
        }

        public void Initialize(IGameAdapter gameAdapter)
        {
            _gameAdapter = gameAdapter;

            UpdateData(gameAdapter.GameServerData);
            _healthBar.Initialize(this);

            UnitSkillManager.AddSkill(new DamageSkill(8, _damageTarget));
            UnitSkillManager.AddSkill(new BarrierSkill(UnitSkillManager, 2, 4));
            UnitSkillManager.AddSkill(new RegenerationSkill(UnitSkillManager, this, 2, 5));
            UnitSkillManager.AddSkill(new FireballSkill(_damageTarget, _damageTarget.UnitSkillManager, 5, 6));
            UnitSkillManager.AddSkill(new CleansingSkill(UnitSkillManager, 1, 5));

            UnitSkillManager.UsedSkill.Subscribe(UsedSkill).AddTo(gameObject);
            UnitSkillManager.Effects.ObserveAdd().Subscribe(_ => AddedEffect(_.Value)).AddTo(gameObject);
            _gameAdapter.OnStateUpdate.Subscribe(UpdateData).AddTo(gameObject);
        }

        public virtual void StartStep()
        {
            UnitState.Value = EUnitState.HisMove;

            UnitSkillManager.ActivateAllBuff();
        }

        private void UsedSkill(ActiveSkill skill)
        {
            _gameAdapter.SendUsedSkill(_eUnit, skill.Type, skill.Cooldown);
            OnUsedSkill.Execute(skill.Type);
            UnitState.Value = EUnitState.Wait;
        }

        private void AddedEffect(Effect effect)
        {
            _gameAdapter.SendAddedEffect(_eUnit, effect.Type, effect.Step);
        }

        public void AddHealth(int health)
        {
            _gameAdapter.SendAddHealth(_eUnit, health);
        }

        public void RemoveHealth(int damage)
        {
            if (UnitSkillManager.Effects.TryGetValue(EEffect.Barrier, out var effect))
            {
                damage -= effect.Value;
                if (damage <= 0) return;
            }

            _gameAdapter.SendRemoveHealth(_eUnit, damage);
        }

        private void UpdateData(GameServerData gameServerData)
        {
            var unitServerData = _eUnit == EUnit.Player
                ? gameServerData.PlayerServerData
                : gameServerData.EnemyServerData;

            Health.Value = unitServerData.Health;

            if (Health.Value <= 0)
            {
                IsDied = true;
                UnitState.Value = EUnitState.Died;
            }

            UnitSkillManager.SkillsReduceCooldown(unitServerData.SkillsServerData);
            UnitSkillManager.EffectsReduceStep(unitServerData.EffectsServerData);
        }
    }
}