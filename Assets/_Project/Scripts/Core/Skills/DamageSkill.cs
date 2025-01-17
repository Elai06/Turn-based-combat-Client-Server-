using Core.Enums;

namespace Core.Skills
{
    public class DamageSkill : ActiveSkill
    {
        private IHealth _target;

        public DamageSkill(int value, IHealth target) : base(ESkill.Damage, value, 0)
        {
            _target = target;
        }

        public override void Activate()
        {
            base.Activate();

            _target.RemoveHealth(Value);
        }
    }
}