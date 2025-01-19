using Core;
using Core.Enums;
using UniRx;
using Unit = Core.Unit;

namespace Network
{
    public interface IGameAdapter
    {
        public void SendAddHealth(EUnit unit, int health);
        public void SendRemoveHealth(EUnit unit, int health);
        public ReactiveCommand<GameServerData> OnStateUpdate { get; }
        ReactiveCommand<GameServerData> OnRestartGame { get; }
        GameServerData GameServerData { get; }
        void RequestRestart();
        void SendAddedEffect(EUnit unitType, EEffect effectType, int cooldown);
        void SendUsedSkill(EUnit unitType, ESkill skillType, int cooldown);
        void FinishRound(Player player, Unit enemy);
    }
}