using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Skills.Effects;
using UniRx;

namespace Core.Skills
{
    public class UnitSkillManager
    {
        public readonly ReactiveCommand<ESkill> UsedSkill = new();

        public Dictionary<ESkill, ActiveSkill> Skills { get; private set; } = new();
        public ReactiveDictionary<EEffect, Effect> Effects { get; private set; } = new();

        public readonly ReactiveCommand EffectsChanged = new();

        public void ActivateSkill(ESkill eSkill)
        {
            Skills[eSkill].Activate();

            UsedSkill.Execute(eSkill);
        }

        public void ActivateAllBuff()
        {
            for (var i = 0; i < Effects.Count; i++)
            {
                var effect = Effects.Values.ToList()[i];
                if (effect.Step > 0)
                {
                    effect.Activate();
                    EffectsChanged.Execute();
                }
                else
                {
                    Effects.Remove(effect.Type);
                }
            }
            
        }

        public void SkillsReduceCooldown()
        {
            foreach (var skill in Skills.Values)
            {
                if (skill.Step > 0)
                {
                    skill.ReduceCooldown();
                }
            }
        }

        public void AddSkill(ActiveSkill skill)
        {
            Skills.TryAdd(skill.Type, skill);
        }

        public void AddBuff(Effect effect)
        {
            Effects.TryAdd(effect.Type, effect);
            EffectsChanged.Execute();
        }

        public void DeactivateBuff(EEffect eEffect)
        {
            if (Effects.ContainsKey(eEffect))
            {
                Effects.Remove(eEffect);
                EffectsChanged.Execute();
            }
        }
    }
}