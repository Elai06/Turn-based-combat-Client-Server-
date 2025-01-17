using Core.Enums;

namespace Core.Skills
{
    public class Skill
    {
        public ESkill Type;
        private protected int Value;

        protected Skill(ESkill type, int value)
        {
            Type = type;
            Value = value;
        }

        public virtual void Activate(){}

    }
}