using Core.Enums;

namespace Network
{
    public class SkillServerData
    {
        public ESkill SkillType;
        public int Cooldown;

        public SkillServerData(ESkill skillType, int cooldown)
        {
            SkillType = skillType;
            Cooldown = cooldown;
        }
    }
}