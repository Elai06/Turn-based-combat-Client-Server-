using Core.Enums;

namespace Network
{
    public class EffectServerData
    {
        public EEffect EEffect;
        public int Cooldown;

        public EffectServerData(EEffect effectType, int cooldown)
        {
            EEffect = effectType;
            Cooldown = cooldown;
        }
    }
}