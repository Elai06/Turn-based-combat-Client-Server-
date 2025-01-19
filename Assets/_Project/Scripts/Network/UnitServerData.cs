using System.Collections.Generic;

namespace Network
{
    public class UnitServerData
    {
        public int Health;
        public List<SkillServerData> SkillsServerData = new();
        public List<EffectServerData> EffectsServerData = new();

        public UnitServerData(int health)
        {
            Health = health;
        }
    }
}