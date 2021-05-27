using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class MovableSystem : IEcsSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<MovableComponent> _filter = null;
    }
}