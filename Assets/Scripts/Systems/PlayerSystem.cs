using Components;
using Leopotam.Ecs;

namespace Systems
{
    public class PlayerSystem : IEcsSystem
    {
        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _filter = null;
    }
}