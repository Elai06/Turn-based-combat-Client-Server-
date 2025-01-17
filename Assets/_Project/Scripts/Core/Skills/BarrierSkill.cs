using Core.Enums;
using Core.Skills.Buffs;

namespace Core.Skills
{
    public class BarrierSkill : ActiveSkill
    {
        private UnitSkillManager _unitSkillManager;
        public BarrierSkill(UnitSkillManager unitSkillManager,int value, int cooldown) : base(ESkill.Barrier, value, cooldown)
        {
            _unitSkillManager = unitSkillManager;
        }

        public override void Activate()
        {
            base.Activate();
            
            _unitSkillManager.AddBuff(new BarrierEffect(5, Value));
        }
    }
}