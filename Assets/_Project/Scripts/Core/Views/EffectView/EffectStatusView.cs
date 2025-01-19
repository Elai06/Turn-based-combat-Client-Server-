using System.Collections.Generic;
using Core.Enums;
using Core.Skills.Effects;
using UniRx;
using UnityEngine;

namespace Core.Views.EffectView
{
    public class EffectStatusView : MonoBehaviour
    {
        [SerializeField] private BattleSystem _battleSystem;
        [SerializeField] private Unit _unit;
        [SerializeField] private EffectSubView _prefab;
        [SerializeField] private Transform _subViewContainer;

        private readonly Dictionary<EEffect, EffectSubView> _effectSubViews = new();

        private void Start()
        {
            _unit.UnitSkillManager.EffectsChanged.Subscribe(_ => EffectsChanged()).AddTo(gameObject);
            _unit.UnitSkillManager.Effects.ObserveAdd().Subscribe(_ => OnAdded(_.Value)).AddTo(gameObject);
            _unit.UnitSkillManager.Effects.ObserveRemove().Subscribe(_ => OnRemove(_.Value)).AddTo(gameObject);
            _battleSystem.RestartedGame.Subscribe(_ => RestartedGame()).AddTo(gameObject);
        }

        private void OnAdded(Effect effect)
        {
            if (_effectSubViews.TryGetValue(effect.Type, out var subView))
            {
                if (!subView.gameObject.activeSelf)
                {
                    subView.gameObject.SetActive(true);
                }

                subView.Init(effect.Type, effect.Step);
            }
            else
            {
                _effectSubViews.Add(effect.Type, CreateSubView(effect));
            }
        }

        private void OnRemove(Effect effect)
        {
            if (_effectSubViews.TryGetValue(effect.Type, out var subView))
            {
                subView.Init(effect.Type, effect.Step);

                if (subView.gameObject.activeSelf)
                {
                    subView.gameObject.SetActive(false);
                }
            }
        }

        private void EffectsChanged()
        {
            foreach (var effect in _unit.UnitSkillManager.Effects.Values)
            {
                if (_effectSubViews.TryGetValue(effect.Type, out var subView))
                {
                    subView.Init(effect.Type, effect.Step);

                    if (effect.Step == 0)
                    {
                        subView.gameObject.SetActive(false);
                    }
                }
            }
        }

        private EffectSubView CreateSubView(Effect effect)
        {
            var subView = Instantiate(_prefab, _subViewContainer);
            subView.Init(effect.Type, effect.Step);
            return subView;
        }

        private void RestartedGame()
        {
            foreach (var subView in _effectSubViews.Values)
            {
                subView.gameObject.SetActive(false);
            }
        }
    }
}