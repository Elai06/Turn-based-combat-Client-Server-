using Core.Enums;

namespace Core.Skills.Effects
{
    public abstract class Effect
    {
        public EEffect Type { get; private set; }
        public int Value { get; private set; }
        public int Step { get; private set; }

        protected Effect(EEffect type, int value, int step)
        {
            Type = type;
            Value = value;
            Step = step;
        }

        public virtual void Activate()
        {
            Step--;
        }
    }
}