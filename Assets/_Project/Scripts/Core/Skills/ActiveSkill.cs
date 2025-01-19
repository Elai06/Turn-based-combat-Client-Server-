using Core.Enums;

namespace Core.Skills
{
    public abstract class ActiveSkill : Skill
    {
        public int Cooldown;
        public int Step;

        protected ActiveSkill(ESkill type, int value, int cooldown) : base(type, value)
        {
            Cooldown = cooldown;
        }

        public override void Activate()
        {
            base.Activate();
        }

        public void ReduceCooldown()
        {
            Step--;
        }
    }
}