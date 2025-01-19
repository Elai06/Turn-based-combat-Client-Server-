using System;
using Core;
using Core.Enums;
using UniRx;
using Unit = Core.Unit;

namespace Network
{
    public class LocalGameAdapter : IGameAdapter
    {
        public GameServerData GameServerData { get; private set; }

        public ReactiveCommand<GameServerData> OnStateUpdate { get; } = new();

        public ReactiveCommand<GameServerData> OnRestartGame { get; } = new();

        public LocalGameAdapter()
        {
            RestartGame();
        }

        public void FinishRound(Player player, Unit enemy)
        {
            SendReduceCooldown(GameServerData.PlayerServerData);
            SendReduceCooldown(GameServerData.EnemyServerData);

            OnStateUpdate.Execute(GameServerData);
        }

        private void SendReduceCooldown(UnitServerData unitServerData)
        {
            for (var i = 0; i < unitServerData.EffectsServerData.Count; i++)
            {
                var effectData = unitServerData.EffectsServerData[i];

                if (effectData.Cooldown == 0) continue;

                effectData.Cooldown--;
            }

            foreach (var skillServerData in unitServerData.SkillsServerData)
            {
                if (skillServerData.SkillType == ESkill.Damage) continue;

                skillServerData.Cooldown--;
            }
        }

        public void SendUsedSkill(EUnit unitType, ESkill skillType, int cooldown)
        {
            var unitData = unitType == EUnit.Player
                ? GameServerData.PlayerServerData
                : GameServerData.EnemyServerData;

            var skill = unitData.SkillsServerData.Find(x => x.SkillType == skillType);
            if (skill != null)
            {
                skill.Cooldown = cooldown;
            }
            else
            {
                unitData.SkillsServerData.Add(new SkillServerData(skillType, cooldown));
            }
        }

        public void SendAddedEffect(EUnit unitType, EEffect effectType, int cooldown)
        {
            var unitData = unitType == EUnit.Player
                ? GameServerData.PlayerServerData
                : GameServerData.EnemyServerData;

            var effectData = unitData.EffectsServerData.Find(x => x.EEffect == effectType);
            if (effectData != null)
            {
                effectData.Cooldown = cooldown;
            }
            else
            {
                unitData.EffectsServerData.Add(new EffectServerData(effectType, cooldown));
            }
        }

        public void SendAddHealth(EUnit unit, int health)
        {
            if (unit == EUnit.Player)
            {
                GameServerData.PlayerServerData.Health += health;
            }
            else
            {
                GameServerData.EnemyServerData.Health += health;
            }

            OnStateUpdate?.Execute(GameServerData);
        }

        public void SendRemoveHealth(EUnit unit, int health)
        {
            if (unit == EUnit.Player)
            {
                GameServerData.PlayerServerData.Health -= health;
            }
            else
            {
                GameServerData.EnemyServerData.Health -= health;
            }

            CheckGameOver();

            OnStateUpdate?.Execute(GameServerData);
        }

        public void RequestRestart()
        {
            RestartGame();
            OnStateUpdate?.Execute(GameServerData);
            OnRestartGame?.Execute(GameServerData);
        }

        private void CheckGameOver()
        {
            if (GameServerData.EnemyServerData.Health <= 0 || GameServerData.PlayerServerData.Health <= 0)
            {
                GameServerData.GameOver = true;

                RequestRestart();
            }
        }

        private void RestartGame()
        {
            GameServerData = new GameServerData
            {
                PlayerServerData = new UnitServerData(100),
                EnemyServerData = new UnitServerData(100),
                GameOver = false
            };
        }
    }

    [Serializable]
    public class GameServerData
    {
        public UnitServerData PlayerServerData;
        public UnitServerData EnemyServerData;
        public bool GameOver;
    }
}