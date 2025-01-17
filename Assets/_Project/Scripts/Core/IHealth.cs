using UniRx;

namespace Core
{
    public interface IHealth
    {
        public ReactiveProperty<int> Health { get; }
        public void AddHealth(int health);
        public void RemoveHealth(int damage);
    }
}