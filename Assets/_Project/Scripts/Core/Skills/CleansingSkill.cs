using Core.Enums;

namespace Core.Skills
{
    public class CleansingSkill : ActiveSkill
    {
        private UnitSkillManager _unitSkillManager;

        public CleansingSkill(UnitSkillManager unitSkillManager, int value, int cooldown = 0) 
            : base(ESkill.Cleansing, value, cooldown)
        {
            _unitSkillManager = unitSkillManager;
        }

        public override void Activate()
        {
            base.Activate();
            
            _unitSkillManager.DeactivateBuff(EEffect.Burn);
        }
    }
}