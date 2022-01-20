using Leopotam.Ecs;

namespace UI
{
    public interface IController : IEcsInitSystem, IEcsDestroySystem
    {
        void Show();
        void Hide();
    }
}