using Core.Enums;

namespace Core.Skills.Effects
{
    public class RegenerationEffect : Effect
    {
        private readonly IHealth _health;

        
        public RegenerationEffect(int value, int step, IHealth health) : base(EEffect.Regeneration, value, step)
        {
            _health = health;
        }

        public override void Activate()
        {
            base.Activate();
            _health.AddHealth(Value);
        }
    }
}