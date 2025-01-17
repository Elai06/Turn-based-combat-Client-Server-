using Core.Enums;
using Core.Skills.Effects;

namespace Core.Skills
{
    public class FireballSkill : ActiveSkill
    {
        private readonly IHealth _target;
        private readonly UnitSkillManager _unitSkillManager;

        public FireballSkill(IHealth target,UnitSkillManager skillManager, int value, int cooldown = 0) 
            : base(ESkill.FireBall, value, cooldown)
        {
            _target = target;
            _unitSkillManager = skillManager;
        }

        public override void Activate()
        {
            base.Activate();
            _unitSkillManager.AddBuff(new BurnEffect(1, 5, _target));
            _target.RemoveHealth(Value);
        }
    }
}