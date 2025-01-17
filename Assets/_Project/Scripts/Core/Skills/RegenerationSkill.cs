using Core.Enums;
using Core.Skills.Effects;

namespace Core.Skills
{
    public class RegenerationSkill : ActiveSkill
    {
        private readonly UnitSkillManager _unitSkillManager;
        private readonly IHealth _target;

        public RegenerationSkill(UnitSkillManager unitSkillManager, IHealth target, int value, int cooldown = 0) 
            : base(ESkill.Regeneration, value, cooldown)
        {
            _unitSkillManager = unitSkillManager;
            _target = target;
        }

        public override void Activate()
        {
            base.Activate();

            _unitSkillManager.AddBuff(new RegenerationEffect(2,Value, _target));
        }
    }
}