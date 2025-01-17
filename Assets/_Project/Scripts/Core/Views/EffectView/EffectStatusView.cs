using System.Collections.Generic;
using Core.Enums;
using Core.Skills.Effects;
using UniRx;
using UnityEngine;

namespace Core.Views.EffectView
{
    public class EffectStatusView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private EffectSubView _prefab;
        [SerializeField] private Transform _subViewContainer;

        private readonly Dictionary<EEffect, EffectSubView> _effectSubViews = new();

        private void Start()
        {
            _unit.UnitUnitSkillsManager.EffectsChanged.Subscribe(_ => EffectsChanged()).AddTo(gameObject);
            _unit.UnitUnitSkillsManager.Effects.ObserveAdd().Subscribe(_ => OnAdded(_.Value)).AddTo(gameObject);
            _unit.UnitUnitSkillsManager.Effects.ObserveRemove().Subscribe(_ => OnRemove(_.Value)).AddTo(gameObject);
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
            foreach (var effect in _unit.UnitUnitSkillsManager.Effects.Values)
            {
                if (_effectSubViews.TryGetValue(effect.Type, out var subView))
                {
                    subView.Init(effect.Type, effect.Step);
                }
            }
        }

        private EffectSubView CreateSubView(Effect effect)
        {
            var subView = Instantiate(_prefab, _subViewContainer);
            subView.Init(effect.Type, effect.Step);
            return subView;
        }
    }
}