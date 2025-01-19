using System;
using Core.Enums;
using Cysharp.Threading.Tasks;
using Network;
using UniRx;
using UnityEngine;

namespace Core
{
    public class BattleSystem : MonoBehaviour
    {
        public ReactiveCommand RestartedGame = new();

        [SerializeField] private Player _player;
        [SerializeField] private Unit _enemy;

        public ReactiveProperty<EUnit> StartStep = new();
        public ReactiveProperty<int> RoundValue = new();
        public readonly ReactiveCommand FinishedRound = new();

        private readonly IGameAdapter _gameAdapter = new LocalGameAdapter();

        private void Start()
        {
            _player.OnUsedSkill.Subscribe(OnPlayerMadeMove).AddTo(gameObject);
            _enemy.OnUsedSkill.Subscribe(OnEnemyMadeMove).AddTo(gameObject);
            _gameAdapter.OnRestartGame.Subscribe(RestartGame).AddTo(gameObject);
            _enemy.Initialize(_gameAdapter);
            _player.Initialize(_gameAdapter);
            StartRound();
        }

        private void StartRound()
        {
            StartStep.Value = EUnit.Player;
            _player.StartStep();
        }

        private async void OnPlayerMadeMove(ESkill skill)
        {
            StartStep.Value = EUnit.Enemy;

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
            _enemy.StartStep();
        }

        private void OnEnemyMadeMove(ESkill skill)
        {
            FinishRound();
        }

        private async void FinishRound()
        {
            _gameAdapter.FinishRound(_player, _enemy);

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

            RoundValue.Value++;
            StartRound();

            FinishedRound.Execute();
        }

        private void RestartGame(GameServerData gameServerData)
        {
            _enemy.UnitSkillManager.Reset();
            _player.UnitSkillManager.Reset();

            RestartedGame.Execute();
        }

        public void RequestRestart()
        {
            _gameAdapter.RequestRestart();
        }
    }
}