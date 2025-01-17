using Core.Enums;
using Core.Skills.Effects;

namespace Core.Skills.Buffs
{
    public class BarrierEffect : Effect
    {
        public BarrierEffect(int value, int step) : base(EEffect.Barrier, value, step)
        {
        }

        public override void Activate()
        {
            base.Activate();
        }
    }
}