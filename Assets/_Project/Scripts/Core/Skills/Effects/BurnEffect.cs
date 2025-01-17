using Core.Enums;

namespace Core.Skills.Effects
{
    public class BurnEffect : Effect
    {
        private readonly IHealth _health;
        
        public BurnEffect( int value, int step, IHealth health) : base(EEffect.Burn, value, step)
        {
            _health = health;
        }

        public override void Activate()
        {
            base.Activate();
            
            _health.RemoveHealth(Value);
        }
    }
}