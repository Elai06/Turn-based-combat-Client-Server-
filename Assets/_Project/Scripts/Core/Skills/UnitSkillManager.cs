using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Skills.Effects;
using Network;
using UniRx;

namespace Core.Skills
{
    public class UnitSkillManager
    {
        public Dictionary<ESkill, ActiveSkill> Skills { get; private set; } = new();
        public ReactiveDictionary<EEffect, Effect> Effects { get; private set; } = new();

        public ReactiveCommand EffectsChanged { get; private set; } = new();
        public ReactiveCommand<ActiveSkill> UsedSkill { get; private set; } = new();


        public void ActivateSkill(ESkill skillType)
        {
            var skill = Skills[skillType];
            skill.Activate();
            UsedSkill.Execute(skill);
        }

        public void ActivateAllBuff()
        {
            for (var i = 0; i < Effects.Count; i++)
            {
                var effect = Effects.Values.ToList()[i];
                if (effect.Step > 0)
                {
                    effect.Activate();
                }
                else
                {
                    Effects.Remove(effect.Type);
                }
            }

            EffectsChanged.Execute();
        }

        public void EffectsReduceStep(List<EffectServerData> effectsServerData)
        {
            foreach (var effectServerData in effectsServerData)
            {
                if (Effects.TryGetValue(effectServerData.EEffect, out var effect))
                {
                    effect.UpdateStep(effectServerData.Cooldown);
                }
            }
        }

        public void SkillsReduceCooldown(List<SkillServerData> skillsServerData)
        {
            foreach (var skill in skillsServerData)
            {
                Skills[skill.SkillType].Step = skill.Cooldown;
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
            }
        }

        public void Reset()
        {
            Effects.Clear();
            EffectsChanged.Execute();
            foreach (var skill in Skills.Values)
            {
                skill.Step = 0;
            }
        }
    }
}