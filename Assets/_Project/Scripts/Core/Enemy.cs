using System.Linq;
using Core.Enums;
using UnityEngine;

namespace Core
{
    public class Enemy : Unit
    {
        public override void StartStep()
        {
            base.StartStep();

            var skill = GetRandomSkill();
            UnitSkillManager.ActivateSkill(skill);
            
            Debug.Log($"Enemy use {skill}");
        }

        private ESkill GetRandomSkill()
        {
            var skills = UnitSkillManager.Skills.Values
                .Where(skill => skill.Step == 0).ToList();
            var randomRange = Random.Range(0, skills.Count);
            return skills[randomRange].Type;
        }
    }
}