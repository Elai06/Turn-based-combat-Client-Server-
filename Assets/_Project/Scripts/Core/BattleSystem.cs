using System;
using Core.Enums;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Core
{
    public class BattleSystem : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private Unit _enemy;
        [FormerlySerializedAs("FinishedStep")] [FormerlySerializedAs("IsMove")] public ReactiveProperty<EUnit> StartStep = new();

        public readonly ReactiveCommand StartedRound = new();
        public readonly ReactiveCommand FinishedRound = new();
        public ReactiveProperty<int> RoundValue = new();

        private void Awake()
        {
            StartRound();
            _player.OnUsedSkill.Subscribe(OnPlayerMadeMove).AddTo(gameObject);
            _enemy.OnUsedSkill.Subscribe(OnEnemyMadeMove).AddTo(gameObject);
        }

        private void StartRound()
        {
            StartStep.Value = EUnit.Player;
            _player.StartStep();

            StartedRound.Execute();
        }

        private async void OnPlayerMadeMove(ESkill skill)
        {
            StartStep.Value = EUnit.Enemy;

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            _enemy.StartStep();

            if (_enemy.IsDied)
            {
                SceneManager.LoadScene(0);
            }
        }

        private void OnEnemyMadeMove(ESkill skill)
        {
            FinishRound();

            if (_player.IsDied)
            {
                SceneManager.LoadScene(0);
            }
        }

        private async void FinishRound()
        {
            ChillSkills();

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

            RoundValue.Value++;
            StartRound();

            FinishedRound.Execute();
        }

        private void ChillSkills()
        {
            _player.UnitUnitSkillsManager.ActivateAllBuff();
            _player.UnitUnitSkillsManager.SkillsReduceCooldown();
            _enemy.UnitUnitSkillsManager.ActivateAllBuff();
            _enemy.UnitUnitSkillsManager.SkillsReduceCooldown();
        }
    }
}